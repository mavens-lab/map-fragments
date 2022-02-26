
#if USE_RWT

using System.Collections;
using System.Collections.Generic;
using AURORA.Protobuf.GeoSpatial;
using UnityEngine;

public class RwtGeoCoordinateAdapter : AURORA_GeoCoordinateAdapter
{
    [SerializeField]
    InfinityCode.RealWorldTerrain.RealWorldTerrainContainer m_realWorldTerrainContainer;

    public void Awake()
    {
        if(m_realWorldTerrainContainer == null)
        {
            m_realWorldTerrainContainer = GetComponentInParent<InfinityCode.RealWorldTerrain.RealWorldTerrainContainer>();
        }
        if(m_realWorldTerrainContainer == null)
        {
            // If still null, look elsewhere in scene.
            InfinityCode.RealWorldTerrain.RealWorldTerrainContainer[] rwts = Object.FindObjectsOfType<InfinityCode.RealWorldTerrain.RealWorldTerrainContainer>();

            if (rwts.Length > 0)
            {
                m_realWorldTerrainContainer = rwts[0];
            }

        }
    }

    private void Reset()
    {
        m_realWorldTerrainContainer = GetComponentInParent<InfinityCode.RealWorldTerrain.RealWorldTerrainContainer>();
    }

    public override bool GeoCoordinateToWorldPosition(GeoCoordinate geoCoordinate, out Vector3 worldPosition)
    {
        if (m_realWorldTerrainContainer != null && geoCoordinate.LatLon != null)
        {
            if (geoCoordinate.Altitude != null)
            {
                return m_realWorldTerrainContainer.GetWorldPosition(geoCoordinate.LatLon.Longitude, geoCoordinate.LatLon.Latitude, geoCoordinate.Altitude.Meters, out worldPosition);
            }
            else
            {
                return m_realWorldTerrainContainer.GetWorldPosition(geoCoordinate.LatLon.Longitude, geoCoordinate.LatLon.Latitude, out worldPosition);
            }
        }

        worldPosition = Vector3.zero;
        return false;
    }

    public override bool WorldPositionToGeoCoordinate(Vector3 worldPosition, out GeoCoordinate geoCoordinate)
    {
        if (m_realWorldTerrainContainer != null)
        {
            bool result = m_realWorldTerrainContainer.GetCoordinatesByWorldPosition(worldPosition, out double longitude, out double latitude, out double altitude);

            if (result)
            {
                geoCoordinate = new GeoCoordinate();
                geoCoordinate.LatLon = new LatLon();
                geoCoordinate.Altitude = new Altitude();
                geoCoordinate.LatLon.Latitude = latitude;
                geoCoordinate.LatLon.Longitude = longitude;
                geoCoordinate.Altitude.Meters = altitude;
            }
            else
            {
                geoCoordinate = null;
            }

            return result;
        }

        geoCoordinate = null;
        return false;
    }

    public override bool GeoCoordinateWithinTerrainBounds(GeoCoordinate geoCoordinate)
    {
        if (geoCoordinate.LatLon.Longitude < m_realWorldTerrainContainer.leftLongitude)
            return false;

        if (geoCoordinate.LatLon.Longitude > m_realWorldTerrainContainer.rightLongitude)
            return false;

        if (geoCoordinate.LatLon.Latitude > m_realWorldTerrainContainer.topLatitude)
            return false;

        if (geoCoordinate.LatLon.Latitude < m_realWorldTerrainContainer.bottomLatitude)
            return false;

        return true;
    }

}

# endif