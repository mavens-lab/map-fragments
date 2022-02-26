/**
 * AURORA-NET Unity API
 * 
 * Provides management of all known Aurora Objects.
 * 
 * Developer: Stormfish Scientific Corporation
 * Author: Theron T. Trout
 * https://www.stormfish.io
 * 
 * 
 * Copyright (C) 2019, 2020 by Stormfish Scientific Corporation
 * All Rights Reserved
 *
 * See LICENSE file for Terms of Use.
 * 
 * THERE IS NO WARRANTY FOR THE PROGRAM, TO THE EXTENT PERMITTED BY APPLICABLE
 * LAW. EXCEPT WHEN OTHERWISE STATED IN WRITING THE COPYRIGHT HOLDERS AND/OR
 * OTHER PARTIES PROVIDE THE PROGRAM “AS IS” WITHOUT WARRANTY OF ANY KIND,
 * EITHER EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. THE
 * ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE PROGRAM IS WITH YOU.
 * SHOULD THE PROGRAM PROVE DEFECTIVE, YOU ASSUME THE COST OF ALL NECESSARY
 * SERVICING, REPAIR OR CORRECTION. YOU ARE SOLELY RESPONSIBLE FOR DETERMINING
 * THE APPROPRIATENESS OF USING OR REDISTRIBUTING THE WORK AND ASSUME ANY
 * RISKS ASSOCIATED WITH YOUR EXERCISE OF PERMISSIONS UNDER THIS LICENSE.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AURORA_Tools
{
    public const int POSITION_MASK = 0x1;
    public const int ROTATION_MASK = 0x2;
    public const int SCALE_MASK = 0x4;
    public const int METADATA_MASK = 0x8;
    public const int GEOPOSITION_MASK = 0x16;
    public const int ALL_MASK = POSITION_MASK | ROTATION_MASK | SCALE_MASK | METADATA_MASK | GEOPOSITION_MASK;

    public static AURORA.Protobuf.Transform MakeAuroraTransform(Transform unityTranform, AURORA.Protobuf.GeoSpatial.GeoCoordinate geoCoordinate=null,
        bool useLocal=true, bool useEulerAngles=false, bool useLossyScale=false, int transformMask = ALL_MASK)
    {
        AURORA.Protobuf.Transform auroraTransform = new AURORA.Protobuf.Transform();

        if (useLocal)
        {
            if ((transformMask & ROTATION_MASK) == ROTATION_MASK)
            {
                if (useEulerAngles)
                {
                    auroraTransform.LocalEulerAngles = MakeAuroraVector3(unityTranform.localEulerAngles);
                }
                else
                {
                    auroraTransform.LocalRotation = MakeAuroraQuaternion(unityTranform.localRotation);
                }
            }

            if ((transformMask & POSITION_MASK) == POSITION_MASK)
            {
                auroraTransform.LocalPosition = MakeAuroraVector3(unityTranform.localPosition);
            }

            if ((transformMask & SCALE_MASK) == SCALE_MASK)
            {
                auroraTransform.LocalScale = MakeAuroraVector3(unityTranform.localScale);
            }
        }
        else // Use Global
        {
            if ((transformMask & ROTATION_MASK) == ROTATION_MASK)
            {
                if (useEulerAngles)
                {
                    auroraTransform.EulerAngles = MakeAuroraVector3(unityTranform.eulerAngles);
                }
                else
                {
                    auroraTransform.Rotation = MakeAuroraQuaternion(unityTranform.rotation);
                }
            }

            if ((transformMask & POSITION_MASK) == POSITION_MASK)
            {
                auroraTransform.Position = MakeAuroraVector3(unityTranform.position);
            }

            if ((transformMask & SCALE_MASK) == SCALE_MASK)
            {
                if (useLossyScale)
                {
                    auroraTransform.LossyScale = MakeAuroraVector3(unityTranform.lossyScale);
                }
                else
                {
                    auroraTransform.LocalScale = MakeAuroraVector3(unityTranform.localScale);
                }
            }
        }

        if((transformMask & GEOPOSITION_MASK) == GEOPOSITION_MASK)
        {
            if (geoCoordinate != null)
            {
                if(auroraTransform.GeoCoordinate == null)
                {
                    auroraTransform.GeoCoordinate = new AURORA.Protobuf.GeoSpatial.GeoCoordinate();
                }
                auroraTransform.GeoCoordinate.MergeFrom(geoCoordinate);
            }
        }

        return auroraTransform;
    }

    public static AURORA.Protobuf.Vector2 MakeAuroraVector2(Vector2 unityVector)
    {
        AURORA.Protobuf.Vector2 auroraVector = new AURORA.Protobuf.Vector2();
        auroraVector.X = unityVector.x;
        auroraVector.Y = unityVector.y;

        return auroraVector;
    }

    public static AURORA.Protobuf.Vector3 MakeAuroraVector3(Vector3 unityVector)
    {
        AURORA.Protobuf.Vector3 auroraVector = new AURORA.Protobuf.Vector3();
        auroraVector.X = unityVector.x;
        auroraVector.Y = unityVector.y;
        auroraVector.Z = unityVector.z;

        return auroraVector;
    }

    public static AURORA.Protobuf.Vector4 MakeAuroraVector4(Vector4 unityVector)
    {
        AURORA.Protobuf.Vector4 auroraVector = new AURORA.Protobuf.Vector4();
        auroraVector.X = unityVector.x;
        auroraVector.Y = unityVector.y;
        auroraVector.Z = unityVector.z;
        auroraVector.W = unityVector.w;

        return auroraVector;
    }

    public static AURORA.Protobuf.Quaternion MakeAuroraQuaternion(Quaternion unityQuaternion)
    {
        AURORA.Protobuf.Quaternion auroraQuaternion = new AURORA.Protobuf.Quaternion();
        auroraQuaternion.X = unityQuaternion.x;
        auroraQuaternion.Y = unityQuaternion.y;
        auroraQuaternion.Z = unityQuaternion.z;
        auroraQuaternion.W = unityQuaternion.w;

        return auroraQuaternion;
    }

    /// <summary>
    /// Copies a transform including fields which are set but have default value.
    /// </summary>
    /// <param name="dest">Destination transform</param>
    /// <param name="src">Source transform</param>
    public static void CopyTransform(ref Transform dest, AURORA.Protobuf.Transform src)
    {
        Vector3 tempV3 = Vector3.zero;
        Quaternion tempQ = Quaternion.identity;

        if (src.EulerAngles != null)
        {            
            tempV3.x = src.EulerAngles.X;
            tempV3.y = src.EulerAngles.Y;
            tempV3.z = src.EulerAngles.Z;

            dest.eulerAngles = tempV3;
        }

        //if (src.Forward != null)
        //{
        //    throw new NotImplementedException();
        //}

        //if (src.GeoCoordinate != null)
        //{
        //    if (dest.GeoCoordinate == null)
        //    {
        //        dest.GeoCoordinate = new Protobuf.GeoSpatial.GeoCoordinate();
        //    }

        //    if (src.GeoCoordinate.Altitude != null)
        //    {
        //        if (dest.GeoCoordinate.Altitude == null)
        //        {
        //            dest.GeoCoordinate.Altitude = new Protobuf.GeoSpatial.Altitude();
        //        }

        //        dest.GeoCoordinate.Altitude.GeodeticDatum = src.GeoCoordinate.Altitude.GeodeticDatum;
        //        dest.GeoCoordinate.Altitude.Meters = src.GeoCoordinate.Altitude.Meters;
        //    }
        //    else
        //    {
        //        if (dest.GeoCoordinate.Altitude != null)
        //        {
        //            dest.GeoCoordinate.Altitude = null;
        //        }
        //    }

        //    if (src.GeoCoordinate.LatLon != null)
        //    {
        //        if (dest.GeoCoordinate.LatLon == null)
        //        {
        //            dest.GeoCoordinate.LatLon = new Protobuf.GeoSpatial.LatLon();
        //        }

        //        dest.GeoCoordinate.LatLon.Altitude = src.GeoCoordinate.LatLon.Altitude;
        //        dest.GeoCoordinate.LatLon.GeodeticDatum = src.GeoCoordinate.LatLon.GeodeticDatum;
        //        dest.GeoCoordinate.LatLon.Latitude = src.GeoCoordinate.LatLon.Latitude;
        //        dest.GeoCoordinate.LatLon.Longitude = src.GeoCoordinate.LatLon.Longitude;
        //    }
        //    else
        //    {
        //        if (dest.GeoCoordinate.LatLon != null)
        //        {
        //            dest.GeoCoordinate.LatLon = null;
        //        }
        //    }

        //    if (src.GeoCoordinate.Mgrs != null)
        //    {
        //        throw new NotImplementedException();
        //    }
        //    if (src.GeoCoordinate.Ups != null)
        //    {
        //        throw new NotImplementedException();
        //    }
        //    if (src.GeoCoordinate.Utm != null)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
        //else
        //{
        //    if (dest.GeoCoordinate != null)
        //    {
        //        dest.GeoCoordinate = null;
        //    }
        //}

        //if (src.GeoDirection != null)
        //{
        //    throw new NotImplementedException();
        //}

        //src.Handedness = dest.Handedness;

        if (src.LocalEulerAngles != null)
        {
            
            tempV3.x = src.LocalEulerAngles.X;
            tempV3.y = src.LocalEulerAngles.Y;
            tempV3.z = src.LocalEulerAngles.Z;

            dest.localEulerAngles = tempV3;
        }

        if (src.LocalPosition != null)
        {
            tempV3.x = src.LocalPosition.X;
            tempV3.y = src.LocalPosition.Y;
            tempV3.z = src.LocalPosition.Z;

            dest.localPosition = tempV3;
        }

        if (src.LocalRotation != null)
        {
            tempQ.x = src.LocalRotation.X;
            tempQ.y = src.LocalRotation.Y;
            tempQ.z = src.LocalRotation.Z;
            tempQ.w = src.LocalRotation.W;

            dest.localRotation = tempQ;
        }

        if (src.LocalScale != null)
        {
            tempV3.x = src.LocalScale.X;
            tempV3.y = src.LocalScale.Y;
            tempV3.z = src.LocalScale.Z;
            dest.localScale = tempV3;
        }

        //if (src.LossyScale != null)
        //{
        //    tempV3.x = src.LossyScale.X;
        //    tempV3.y = src.LossyScale.Y;
        //    tempV3.z = src.LossyScale.Z;

        //    dest.lossyScale = tempV3;
        //}
    }
}
