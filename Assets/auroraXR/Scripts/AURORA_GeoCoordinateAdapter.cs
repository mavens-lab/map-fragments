using System.Collections;
using System.Collections.Generic;
using AURORA.Protobuf.GeoSpatial;
using UnityEngine;


public class AURORA_GeoCoordinateAdapter : MonoBehaviour
{

    //public bool GeoDirectionToWorldRotation(out Quaternion worldDirection)
    //{
    //    throw new System.NotImplementedException();
    //}

    public virtual bool GeoCoordinateToWorldPosition(GeoCoordinate geoCoordinate, out Vector3 worldPosition)
    {
        throw new System.NotImplementedException();

        //if (m_geoCoordinateAdapter != null)
        //{
        //    return m_geoCoordinateAdapter.GeoCoordinateToWorldPosition(out localPosition);
        //}

        //localPosition = Vector3.zero;

        //return false;
    }

    public virtual bool WorldPositionToGeoCoordinate(Vector3 worldPosition, out GeoCoordinate geoCoordinate)
    {
        throw new System.NotImplementedException();
        //if(m_geoCoordinateAdapter != null)
        //{
        //    return m_geoCoordinateAdapter.WorldPositionToGeoCoordinate(latlon, mgrs, utm, out geoCoordinate);
        //}

        //geoCoordinate = null;
        //return false;
    }

    public virtual bool GeoCoordinateWithinTerrainBounds(GeoCoordinate geoCoordinate)
    {
        throw new System.NotImplementedException();
    }

    //public bool WorldRotationToGeoDirection(out GeoDirection geoDirection)
    //{
    //    throw new System.NotImplementedException();
    //}
}
