#if USE_RWT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoContextAligner : MonoBehaviour
{
    [SerializeField]
    InfinityCode.RealWorldTerrain.RealWorldTerrainContainer m_realWorldTerrainContainer;

    [SerializeField]
    GameObject m_spatialAlignmentContext;

    // Start is called before the first frame update
    void Start()
    {
        //Vector3 newScale = new Vector3();

        GameObject p0 = new GameObject("southwestcorner");
        WorldCoordinatesToWorldPosition(m_realWorldTerrainContainer.leftLongitude, m_realWorldTerrainContainer.bottomLatitude, 
            out Vector3 p0WorldPosition);

        p0.transform.parent = m_realWorldTerrainContainer.transform;
        p0.transform.position = p0WorldPosition;

        GameObject p1 = new GameObject("northeastcorner");
        WorldCoordinatesToWorldPosition(m_realWorldTerrainContainer.rightLongitude, m_realWorldTerrainContainer.topLatitude,
            out Vector3 p1WorldPosition);
        p1.transform.parent = m_realWorldTerrainContainer.transform;
        p1.transform.position = p1WorldPosition;

        //Debug.Log(m_realWorldTerrainContainer.);
        //newScale.x = 100.0f / (p1.transform.localPosition.x - p0.transform.localPosition.x);
        //newScale.z = 100.0f / (p1.transform.localPosition.z - p0.transform.localPosition.z);

        float scaleFactor = 1.0f / ((m_realWorldTerrainContainer.scale.x + m_realWorldTerrainContainer.scale.z)/2f);

        m_spatialAlignmentContext.transform.localScale = new Vector3(
            scaleFactor,
            scaleFactor,
            scaleFactor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WorldCoordinatesToWorldPosition(double longitude, double latitude, out Vector3 worldPosition)
    {
        //Vector2 coord = new Vector2(longitude, latitude);

        //double m_alt = m_realWorldTerrainContainer.GetAltitudeByCoordinates(longitude, latitude);

        m_realWorldTerrainContainer.GetWorldPosition(longitude, latitude, out worldPosition);
    }

    public void GetObjectCoordinates(out float longitude, out float latitude)
    {
        m_realWorldTerrainContainer.GetCoordinatesByWorldPosition(transform.position, out Vector2 vec);

        longitude = vec.x;
        latitude = vec.y;
    }

}

# endif