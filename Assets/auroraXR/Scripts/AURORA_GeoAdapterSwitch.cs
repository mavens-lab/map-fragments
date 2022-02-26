using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AURORA.Utils;

public class AURORA_GeoAdapterSwitch : MonoBehaviour
{
    //[SerializeField] AURORA_GeoCoordinateAdapter m_auroraGeoCoordinateAdapter;
    [SerializeField] EnableOneGameObjectInSet m_enableOneGameObjectInSet;

    [Tooltip("All children under this object will receive the updated Geo Coordinate Adapter on terrain switch")]
    [SerializeField] Transform m_geoPositionParent;

    public AURORA_GeoCoordinateAdapter AuroraGeoCoordinateAdapter
    { 
        //get => m_auroraGeoCoordinateAdapter;
        get
        {
            GameObject go = m_enableOneGameObjectInSet.ActiveGameObject;
            if (go == null)
            {
                return null;
            }
            else
            {
                return go.GetComponent<AURORA_GeoCoordinateAdapter>();
            }
        }
    }

    private void Reset()
    {
        m_geoPositionParent = transform;
    }
}