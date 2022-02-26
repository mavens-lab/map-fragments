using System.Collections;
using System.Collections.Generic;
using AURORA.Protobuf.GeoSpatial;
using UnityEngine;
using Google.Protobuf;

[System.Serializable]
public class AURORA_GeoCoordinateLatLonPanel
{
    [SerializeField]
    public AURORA.Protobuf.GeoSpatial.GeodeticDatum geodeticDatum;

    public double latitude;
    public double longitude;
}

[System.Serializable]
public class AURORA_GeoCoordinateUtmPanel
{
    [SerializeField]
    public AURORA.Protobuf.GeoSpatial.GeodeticDatum geodeticDatum;

    public int utmZone;
    public string latitudeBand;
    public double easting;
    public double northing;
}

[System.Serializable]
public class AURORA_GeoCoordinateMgrsPanel
{
    [SerializeField]
    public AURORA.Protobuf.GeoSpatial.GeodeticDatum geodeticDatum;

    public AURORA.Protobuf.GeoSpatial.MgrsLetteringSchema m_mgrsLetteringSchema;

    public string gridZoneDesignator;
    public string gridSquareId;
    public int easting;
    public int northing;
}

[System.Serializable]
public class AURORA_GeoCoordinateAltitudePanel
{
    [SerializeField]
    public AURORA.Protobuf.GeoSpatial.GeodeticDatum geodeticDatum;

    [SerializeField]
    public double meters;
}

[System.Serializable]
public class AURORA_GeoCoordinatePanel
{
    [SerializeField]
    public AURORA_GeoCoordinateLatLonPanel latitudeLongitude;
}

[System.Serializable]
class GeoBoundaryInclusionChangeEvent : UnityEngine.Events.UnityEvent<GameObject>
{ }

[RequireComponent(typeof(AURORA_GameObject))]
public class AURORA_GeoPosition : MonoBehaviour
{

    [SerializeField]
    AURORA_GeoCoordinateAdapter m_auroraGeoCoordinateAdapter;

    private AURORA_GeoCoordinateAdapter m_previousAuroraGeoCoordinateAdapter;

    //    [SerializeField]
    private AURORA_GameObject m_auroraGameObject;

    [SerializeField]
    private AURORA_GeoCoordinateLatLonPanel m_latitudeLongitude;

    [SerializeField]
    private AURORA_GeoCoordinateUtmPanel m_utm;

    [SerializeField]
    private AURORA_GeoCoordinateMgrsPanel m_mgrs;

    [SerializeField]
    private AURORA_GeoCoordinateAltitudePanel m_altitude;

    [SerializeField]
    GeoBoundaryInclusionChangeEvent m_geoBoundaryEnteredEvents;

    [SerializeField]
    GeoBoundaryInclusionChangeEvent m_geoBoundaryExitedEvents;

    private bool m_doStartupBoundaryChecks;

    private AURORA.Protobuf.GeoSpatial.GeoCoordinate m_geoCoordinate;

    // Used to detect changes in the object moving in and out of geo boundary
    private bool m_previousWithinGeoBoundaryState = false;


    private void Awake()
    {
        if(m_auroraGameObject == null)
        {
            m_auroraGameObject = GetComponent<AURORA_GameObject>();
        }

        if(m_auroraGeoCoordinateAdapter == null)
        {
            AURORA_GeoAdapterSwitch gas = GetComponentInParent<AURORA_GeoAdapterSwitch>();
            if(gas != null)
            {
                m_auroraGeoCoordinateAdapter = gas.AuroraGeoCoordinateAdapter;
            }
            else
            {
                m_auroraGeoCoordinateAdapter = GetComponent<AURORA_GeoCoordinateAdapter>();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_previousWithinGeoBoundaryState = false;
        m_doStartupBoundaryChecks = true;
    }

    public AURORA_GeoCoordinateAdapter AURORA_GeoCoordinateAdapter
    {
        get => m_auroraGeoCoordinateAdapter;
        set
        {
            m_auroraGeoCoordinateAdapter = value;

            if(m_auroraGeoCoordinateAdapter != m_previousAuroraGeoCoordinateAdapter && m_auroraGameObject != null)
            {
                m_auroraGameObject.TerrainAdapterChanged = true;
            }

            // Force an update
            //if(m_auroraGeoCoordinateAdapter != null)
            //{
            //    UpdateGeoCoordinate();
            //}
        }
    }

    public GeoCoordinate GeoCoordinate {
        get => m_geoCoordinate;
        set
        {
            if (m_geoCoordinate == null)
                m_geoCoordinate = new GeoCoordinate();

            m_geoCoordinate.MergeFrom(value);

            if(m_geoCoordinate.LatLon != null)
            {
                m_latitudeLongitude.latitude = m_geoCoordinate.LatLon.Latitude;
                m_latitudeLongitude.longitude = m_geoCoordinate.LatLon.Longitude;
            }
            if(m_geoCoordinate.Altitude != null)
            {
                m_altitude.meters = m_geoCoordinate.Altitude.Meters;
            }
        }
    }

    internal GeoBoundaryInclusionChangeEvent GeoBoundaryEnteredEvents { get => m_geoBoundaryEnteredEvents; set => m_geoBoundaryEnteredEvents = value; }
    internal GeoBoundaryInclusionChangeEvent GeoBoundaryExitedEvents { get => m_geoBoundaryExitedEvents; set => m_geoBoundaryExitedEvents = value; }

    /// <summary>
    /// Update the geo coordinates for the object.
    /// </summary>
    /// <returns>True of the geocoordinates changed, false otherwise or if update failed.</returns>
    public bool UpdateGeoCoordinate()
    {
        if(m_auroraGeoCoordinateAdapter != null)
        {
            if (m_auroraGameObject.PrimaryCoordinateSystem == AURORA_GameObject.CoordinateSystems.VIRTUAL_WORLD_COORDINATES)
            {
                return UpdateGeoCoordinateFromWorldPosition();
            }
            else if(m_auroraGameObject.PrimaryCoordinateSystem == AURORA_GameObject.CoordinateSystems.GEOSPATIAL_CORDINATES)
            {
                return UpdateWorldPositionFromGeoCoordinate();
            }
        }

        return false;
    }

    public bool UpdateWorldPositionFromGeoCoordinate()
    {
        bool geoPositionChanged = false;

        Vector3 lastPosition = transform.position;

        if (m_geoCoordinate == null || m_geoCoordinate.LatLon == null)
            return false;

        bool result = m_auroraGeoCoordinateAdapter.GeoCoordinateToWorldPosition(m_geoCoordinate, out Vector3 newPosition);

        if (result)
        {
            if (lastPosition != newPosition)
            {
                geoPositionChanged = true;
                transform.position = newPosition;
            }
            else
            {
                geoPositionChanged = false;
            }
        }

        // TODO: May be redundant with checks above
        bool currentlyWithinGeoBoundary = m_auroraGeoCoordinateAdapter.GeoCoordinateWithinTerrainBounds(m_geoCoordinate);

        // If transition from outside to inside geoboundary
        if (currentlyWithinGeoBoundary && !m_previousWithinGeoBoundaryState)
        {
            try
            {
                m_geoBoundaryEnteredEvents.Invoke(gameObject);
            }
            catch (System.Exception ex)
            {
                m_auroraGameObject.AURORA_Context.AuroraLogger.AuroraLogger.LogException(ex);
            }
        }
        // If transition from inside to outside geoboundary
        else if (m_previousWithinGeoBoundaryState && !currentlyWithinGeoBoundary)
        {
            try
            {
                m_geoBoundaryExitedEvents.Invoke(gameObject);
            }
            catch (System.Exception ex)
            {
                m_auroraGameObject.AURORA_Context.AuroraLogger.AuroraLogger.LogException(ex);
            }
        }
        // Only do this once, when the object has been spawned but is not in the current geoboundary
        else if (m_doStartupBoundaryChecks && !currentlyWithinGeoBoundary)
        {
            m_doStartupBoundaryChecks = false;

            try
            {
                m_geoBoundaryExitedEvents.Invoke(gameObject);
            }
            catch (System.Exception ex)
            {
                m_auroraGameObject.AURORA_Context.AuroraLogger.AuroraLogger.LogException(ex);
            }
        }

        // if the terrain is not active
        //else
        //{
        // If transition from inside to outside geoboundary
        //if (m_previousWithinGeoBoundaryState || m_doStartupBoundaryChecks)
        //{
        //    m_geoBoundaryExitedEvents.Invoke(gameObject);
        //    m_doStartupBoundaryChecks = false;
        //}
        //}

        m_previousWithinGeoBoundaryState = currentlyWithinGeoBoundary;
        return geoPositionChanged;
    }

    public bool UpdateGeoCoordinateFromWorldPosition()
    {
        // Currently we only track latitude, longitude, and alitude changes.
        // TODO Add more change tracking
        bool geoPositionChanged = false;

        double last_latitude = 0.0;
        double last_longitude = 0.0;
        double last_altitude = 0.0;

        if (m_geoCoordinate != null)
        {
            if (m_geoCoordinate.LatLon != null)
            {
                last_latitude = m_geoCoordinate.LatLon.Latitude;
                last_longitude = m_geoCoordinate.LatLon.Longitude;
            }

            if (m_geoCoordinate.Altitude != null)
            {
                last_altitude = m_geoCoordinate.Altitude.Meters;
            }
        }

        bool result = m_auroraGeoCoordinateAdapter.WorldPositionToGeoCoordinate(transform.position, out m_geoCoordinate);

        if (result)
        {
            if (m_geoCoordinate.LatLon != null)
            {
                m_latitudeLongitude.latitude = m_geoCoordinate.LatLon.Latitude;
                m_latitudeLongitude.longitude = m_geoCoordinate.LatLon.Longitude;

                if (m_geoCoordinate.LatLon.Latitude != last_latitude || m_geoCoordinate.LatLon.Longitude != last_longitude)
                {
                    geoPositionChanged = true;
                }
            }
            if (m_geoCoordinate.Altitude != null)
            {
                m_altitude.meters = m_geoCoordinate.Altitude.Meters;
                if (m_geoCoordinate.Altitude.Meters != last_altitude)
                {
                    geoPositionChanged = true;
                }
            }
        }
        else
        {
            //Debug.LogWarning("Could not get world position from geocoordinate");
        }

        bool currentlyWithinGeoBoundary = m_auroraGeoCoordinateAdapter.GeoCoordinateWithinTerrainBounds(m_geoCoordinate);

        // If transition from outside to inside geoboundary
        if (currentlyWithinGeoBoundary && !m_previousWithinGeoBoundaryState)
        {
            try
            {
                m_geoBoundaryEnteredEvents.Invoke(gameObject);
            }
            catch (System.Exception ex)
            {
                m_auroraGameObject.AURORA_Context.AuroraLogger.AuroraLogger.LogException(ex);
            }
        }
        // If transition from inside to outside geoboundary
        else if (!currentlyWithinGeoBoundary && m_previousWithinGeoBoundaryState)
        {
            try
            {
                m_geoBoundaryExitedEvents.Invoke(gameObject);
            }
            catch (System.Exception ex)
            {
                m_auroraGameObject.AURORA_Context.AuroraLogger.AuroraLogger.LogException(ex);
            }
        }

        m_previousWithinGeoBoundaryState = currentlyWithinGeoBoundary;
        return geoPositionChanged;
    }
}