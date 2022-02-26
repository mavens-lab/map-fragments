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

using AURORA.ClientAPI;
//using AURORA.Protobuf.ServerData;

[System.Serializable]
public class AURORA_RpcRequestEvent : UnityEngine.Events.UnityEvent<AURORA.ClientAPI.RpcRequest> { }

[System.Serializable]
public class AURORA_GameObjectRpcRequestEvent : UnityEngine.Events.UnityEvent<AURORA_GameObject, AURORA.ClientAPI.AuroraObject, AURORA.ClientAPI.RpcRequest> { }

[System.Serializable]
public class AURORA_GameObjectMessageRoundTripTimeEvent : UnityEngine.Events.UnityEvent<AURORA_GameObject, AURORA.ClientAPI.AuroraObject, System.TimeSpan> { }

[System.Serializable]
public class AURORA_GameObjectEvent : UnityEngine.Events.UnityEvent<AURORA_GameObject> { }

[System.Serializable]
public class AURORA_ContextEvent : UnityEngine.Events.UnityEvent<AURORA_Context> { }

[System.Serializable]
public class AURORA_GameObjectMetadataWrapperEvent : UnityEngine.Events.UnityEvent<AURORA_GameObject, AURORA.ClientAPI.MetadataCollection, string, AURORA.Protobuf.MetadataWrapper> { }

[System.Serializable]
public class AURORA_GameObjectMetadataKeyEvent : UnityEngine.Events.UnityEvent<AURORA_GameObject, AURORA.ClientAPI.MetadataCollection, string> { }

[System.Serializable]
public class RpcRequestGameObjectEventQueuePanel
    : AugmentedEventQueue2i3oPanelBase<AURORA_GameObjectRpcRequestEvent, AURORA.ClientAPI.AuroraObject, AURORA.ClientAPI.RpcRequest, AURORA_GameObject>
{ }

[System.Serializable]
public class AuroraObjectEventQueuePanel
    : SubstitutionEventQueue1i1oPanelBase<AURORA_GameObjectEvent, AURORA.ClientAPI.AuroraObject, AURORA_GameObject>
{ }

[System.Serializable]
public class OptionalRpcRequestGameObjectEventQueuePanel 
    : OptionalAugmentedEventQueue2i3oPanelBase<AURORA_GameObjectRpcRequestEvent, AURORA.ClientAPI.AuroraObject, AURORA.ClientAPI.RpcRequest, AURORA_GameObject> { }

[System.Serializable]
public class OptionalAuroraObjectMessageRoundTripTimeEventQueuePanel
    : OptionalAugmentedEventQueue2i3oPanelBase<AURORA_GameObjectMessageRoundTripTimeEvent, AURORA.ClientAPI.AuroraObject, System.TimeSpan, AURORA_GameObject>
{ }


[System.Serializable]
public class OptionalAuroraObjectEventQueuePanel 
    : OptionalSubstitutionEventQueue1i1oPanelBase<AURORA_GameObjectEvent, AURORA.ClientAPI.AuroraObject, AURORA_GameObject> { }

[System.Serializable]
public class OptionalMetadataWrapperEventQueuePanel
    : OptionalAugmentedEventQueue3i4oPanelBase<AURORA_GameObjectMetadataWrapperEvent, AURORA.ClientAPI.MetadataCollection, string, AURORA.Protobuf.MetadataWrapper, AURORA_GameObject>
{ }

[System.Serializable]
public class OptionalMetadataKeyEventQueuePanel 
    : OptionalAugmentedEventQueue2i3oPanelBase<AURORA_GameObjectMetadataKeyEvent, AURORA.ClientAPI.MetadataCollection, string, AURORA_GameObject> { }

//[System.Serializable]
//public class AuroraObjectRpcRequestGameObjectEventQueuePanel 
//    : OptionalAugmentedEventQueue2i3oPanelBase<AURORA.ClientAPI.AuroraObject, AURORA.ClientAPI.RpcRequest, AURORA_GameObject> { }

[System.Serializable]
public class SceneObjectSpecificEvents
{
    protected bool m_atLeastOneActive;

    public OptionalRpcRequestGameObjectEventQueuePanel RegisterSceneObjectSuccessful;
    public OptionalRpcRequestGameObjectEventQueuePanel RegisterSceneObjectFailed;
    public OptionalRpcRequestGameObjectEventQueuePanel RegisterSceneObjectTimedOut;

    public void Initialize(AURORA_GameObject ago)
    {
        RegisterSceneObjectSuccessful.Initialize(ago);
        ago.AURORA_Object.RegisterSceneObjectSuccessfulHandler += RegisterSceneObjectSuccessful.InEvent;

        RegisterSceneObjectFailed.Initialize(ago);
        ago.AURORA_Object.RegisterSceneObjectFailedHandler += RegisterSceneObjectFailed.InEvent;

        RegisterSceneObjectTimedOut.Initialize(ago);
        ago.AURORA_Object.RegisterSceneObjectTimedOutHandler += RegisterSceneObjectTimedOut.InEvent;

        m_atLeastOneActive = (
            RegisterSceneObjectSuccessful.Enabled
            ||
            RegisterSceneObjectFailed.Enabled
            ||
            RegisterSceneObjectTimedOut.Enabled
        );
    }

    public void Invoke()
    {
        if(m_atLeastOneActive)
        {
            RegisterSceneObjectSuccessful.Invoke();
            RegisterSceneObjectFailed.Invoke();
            RegisterSceneObjectTimedOut.Invoke();
        }
    }
}

[System.Serializable]
public class SpawnableObjectSpecificEvents
{
    protected bool m_atLeastOneActive;
    
    public OptionalRpcRequestGameObjectEventQueuePanel ObjectSpawnSuccessful;
    public OptionalRpcRequestGameObjectEventQueuePanel ObjectSpawnFailed;
    public OptionalRpcRequestGameObjectEventQueuePanel ObjectSpawnTimedOut;

    public void Initialize(AURORA_GameObject ago)
    {
        ObjectSpawnSuccessful.Initialize(ago);
        ago.AURORA_Object.ObjectSpawnSuccessfulHandler += ObjectSpawnSuccessful.InEvent;

        ObjectSpawnFailed.Initialize(ago);
        ago.AURORA_Object.ObjectSpawnFailedHandler += ObjectSpawnFailed.InEvent;

        ObjectSpawnTimedOut.Initialize(ago);
        ago.AURORA_Object.ObjectSpawnTimedOutHandler += ObjectSpawnTimedOut.InEvent;

        m_atLeastOneActive = (
            ObjectSpawnSuccessful.Enabled
            ||
            ObjectSpawnFailed.Enabled
            ||
            ObjectSpawnTimedOut.Enabled
        );
    }

    public void Invoke()
    {
        if (m_atLeastOneActive)
        {
            ObjectSpawnSuccessful.Invoke();
            ObjectSpawnFailed.Invoke();
            ObjectSpawnTimedOut.Invoke();
        }
    }
}

[System.Serializable]
public class ObjectDeletionEvents
{
    protected bool m_atLeastOneActive;

    public OptionalAuroraObjectEventQueuePanel ServerRequestForObjectDestruction;
    public OptionalAuroraObjectEventQueuePanel ObjectDestroyed;
    public OptionalRpcRequestGameObjectEventQueuePanel DeleteObjectSuccessful;
    public OptionalRpcRequestGameObjectEventQueuePanel DeleteObjectFailed;
    public OptionalRpcRequestGameObjectEventQueuePanel DeleteObjectTimedOut;

    public void Initialize(AURORA_GameObject ago)
    {
        ServerRequestForObjectDestruction.Initialize(ago);
        ago.AURORA_Object.ObjectDestroyedOnServerHandler += ServerRequestForObjectDestruction.InEvent;

        ObjectDestroyed.Initialize(ago);
        ago.AURORA_Object.ObjectDestroyedHandler += ObjectDestroyed.InEvent;

        DeleteObjectSuccessful.Initialize(ago);
        ago.AURORA_Object.DeleteObjectSuccessfulHandler += DeleteObjectSuccessful.InEvent;

        DeleteObjectFailed.Initialize(ago);
        ago.AURORA_Object.DeleteObjectFailedHandler += DeleteObjectFailed.InEvent;

        DeleteObjectTimedOut.Initialize(ago);
        ago.AURORA_Object.DeleteObjectTimedOutHandler += DeleteObjectTimedOut.InEvent;

        m_atLeastOneActive = (
            ServerRequestForObjectDestruction.Enabled
            ||
            ObjectDestroyed.Enabled
            ||
            DeleteObjectSuccessful.Enabled
            ||
            DeleteObjectFailed.Enabled
            ||
            DeleteObjectTimedOut.Enabled
        );
    }

    public void Invoke()
    {
        if (m_atLeastOneActive)
        {
            ServerRequestForObjectDestruction.Invoke();
            ObjectDestroyed.Invoke();
            DeleteObjectSuccessful.Invoke();
            DeleteObjectFailed.Invoke();
            DeleteObjectTimedOut.Invoke();
        }
    }
}

[System.Serializable]
public class ObjectOwnershipEvents
{
    protected bool m_atLeastOneActive;

    public OptionalAuroraObjectEventQueuePanel ObjectOwnershipChanged;
    public OptionalAuroraObjectEventQueuePanel ObjectOwnershipAcquired;
    public OptionalAuroraObjectEventQueuePanel ObjectOwnershipLost;
    public OptionalRpcRequestGameObjectEventQueuePanel ObjectOwnershipRequestSuccessful;
    public OptionalRpcRequestGameObjectEventQueuePanel ObjectOwnershipRequestFailed;
    public OptionalRpcRequestGameObjectEventQueuePanel ObjectOwnershipRequestTimedOut;
    
    public void Initialize(AURORA_GameObject ago)
    {
        ObjectOwnershipChanged.Initialize(ago);
        ago.AURORA_Object.ObjectOwnershipChangedHandler += ObjectOwnershipChanged.InEvent;

        ObjectOwnershipAcquired.Initialize(ago);
        ago.AURORA_Object.ObjectOwnershipAcquiredHandler += ObjectOwnershipAcquired.InEvent;

        ObjectOwnershipLost.Initialize(ago);
        ago.AURORA_Object.ObjectOwnershipLostHandler += ObjectOwnershipLost.InEvent;

        ObjectOwnershipRequestSuccessful.Initialize(ago);
        ago.AURORA_Object.ObjectOwnershipRequestSuccessfulHandler += ObjectOwnershipRequestSuccessful.InEvent;

        ObjectOwnershipRequestFailed.Initialize(ago);
        ago.AURORA_Object.ObjectOwnershipRequestFailedHandler += ObjectOwnershipRequestFailed.InEvent;

        ObjectOwnershipRequestTimedOut.Initialize(ago);
        ago.AURORA_Object.ObjectOwnershipRequestTimedOutHandler += ObjectOwnershipRequestTimedOut.InEvent;

        m_atLeastOneActive = (
            ObjectOwnershipChanged.Enabled
            ||
            ObjectOwnershipAcquired.Enabled
            ||
            ObjectOwnershipLost.Enabled
            ||
            ObjectOwnershipRequestSuccessful.Enabled
            ||
            ObjectOwnershipRequestFailed.Enabled
            ||
            ObjectOwnershipRequestTimedOut.Enabled
        );
    }

    public void Invoke()
    {
        if (m_atLeastOneActive)
        {
            ObjectOwnershipChanged.Invoke();
            ObjectOwnershipAcquired.Invoke();
            ObjectOwnershipLost.Invoke();
            ObjectOwnershipRequestSuccessful.Invoke();
            ObjectOwnershipRequestFailed.Invoke();
            ObjectOwnershipRequestTimedOut.Invoke();
        }
    }
}


[System.Serializable]
public class ServerRegistrationEvents
{
    protected bool m_atLeastOneActive;

    public AuroraObjectEventQueuePanel ObjectRegistrationStatusChanged;

    public void Initialize(AURORA_GameObject ago)
    {
        ObjectRegistrationStatusChanged.Initialize(ago);
        ago.AURORA_Object.ObjectRegistrationStatusChangedHandler += ObjectRegistrationStatusChanged.InEvent;

        m_atLeastOneActive = true;
    }

    public void Invoke()
    {
        if (m_atLeastOneActive)
        {
            ObjectRegistrationStatusChanged.Invoke();
        }
    }
}

[System.Serializable]
public class AuroraObjectPropertyChangeEvents
{
    protected bool m_atLeastOneActive;

    public OptionalAuroraObjectEventQueuePanel TransformChanged;
    public OptionalAuroraObjectEventQueuePanel GeoCoordinateChanged;
    public OptionalAuroraObjectEventQueuePanel GeoDirectionChanged;
    public OptionalMetadataWrapperEventQueuePanel MetadataChanged;
    public OptionalMetadataKeyEventQueuePanel MetadataKeyRemoved;

    public void Initialize(AURORA_GameObject ago)
    {
        TransformChanged.Initialize(ago);
        ago.AURORA_Object.ObjectOwnershipChangedHandler += TransformChanged.InEvent;

        GeoCoordinateChanged.Initialize(ago);
        ago.AURORA_Object.GeoCoordinateChangedHandler += GeoCoordinateChanged.InEvent;

        GeoDirectionChanged.Initialize(ago);
        ago.AURORA_Object.GeoDirectionChangedHandler += GeoDirectionChanged.InEvent; 

        MetadataChanged.Initialize(ago);
        ago.AURORA_Object.MetadataCollection.MetadataKeyModifiedEventHandler += MetadataChanged.InEvent;

        MetadataKeyRemoved.Initialize(ago);
        ago.AURORA_Object.MetadataCollection.MetadataKeyDeletedEventHandler += MetadataKeyRemoved.InEvent;

        m_atLeastOneActive = (
            TransformChanged.Enabled
            ||
            GeoCoordinateChanged.Enabled
            ||
            GeoDirectionChanged.Enabled
            ||
            MetadataChanged.Enabled
            ||
            MetadataKeyRemoved.Enabled
        );
    }

    public void Invoke()
    {
        if (m_atLeastOneActive)
        {
            TransformChanged.Invoke();
            GeoCoordinateChanged.Invoke();
            GeoDirectionChanged.Invoke();
            MetadataChanged.Invoke();
        }
    }
}

[System.Serializable]
public class AuroraObjectMiscEvents
{
    protected bool m_atLeastOneActive;

    [Tooltip("Must be enabled before Start is called, otherwise has no effect")]
    public bool EnableRoundTripTimeMeasurements = false;

    public OptionalAuroraObjectMessageRoundTripTimeEventQueuePanel RoundTripTimeMeasurementObtained;

    public void Initialize(AURORA_GameObject ago)
    {
        RoundTripTimeMeasurementObtained.Initialize(ago);
        ago.AURORA_Object.MessageRoundTripTimeRecordedHandler += RoundTripTimeMeasurementObtained.InEvent;

        ago.AURORA_Object.MessageRoundTripTimeSamplesEnabled = EnableRoundTripTimeMeasurements;

        m_atLeastOneActive = (
            RoundTripTimeMeasurementObtained.Enabled
        );
    }

    public void Invoke()
    {
        if (m_atLeastOneActive)
        {
            RoundTripTimeMeasurementObtained.Invoke();
        }
    }
}

[RequireComponent(typeof(AURORA_SpatialAnchor))]
public class AURORA_GameObject : MonoBehaviour
{

    public enum TransformTrackingModes
    {
        /// <summary>
        /// Use local position, rotation, scale
        /// </summary>
        USE_LOCAL,
        /// <summary>
        /// Use global position, rotation, and lossy scale
        /// </summary>
        USE_GLOBAL_LOSSY_SCALE,
        /// <summary>
        /// Use glabal position, rotation, but local scale.  This may avoid object scewing
        /// </summary>
        USE_GLOBAL_LOCAL_SCALE
    }

    public enum CoordinateSystems
    {
        /// <summary>
        /// Uses the virtual world coordinates to place the object
        /// </summary>
        VIRTUAL_WORLD_COORDINATES,
        /// <summary>
        /// Uses the geospatial coordinates to place the object
        /// </summary>
       GEOSPATIAL_CORDINATES
    }

    //[SerializeField]
    //bool m_readOnlyIsLocallyOwned;

    [SerializeField]
    AURORA_Manager m_auroraManager;

    [SerializeField]
    AURORA_Context m_auroraContext;

    [Tooltip("Used to get the AURORA Manager after scene loads.")]
    [SerializeField]
    AURORA_Proxy m_auroraProxy;

    [Tooltip("If true then the object is a synchronized AURORA Object and all changes are communicated to other clients.  Should be falsed for spawnable objects.")]
    [SerializeField]
    private AURORA.Protobuf.ServerData.ObjectTypes m_objectType;

    [SerializeField]
    private AURORA_Spawnable m_auroraSpawnable;

    [SerializeField]
    private bool m_deleteOnServerWhenDestroyed = false;


    [SerializeField]
    TransformTrackingModes m_transformTrackingMode;

    [SerializeField]
    AURORA_SpatialAnchor m_auroraSpatialAnchor;

    [SerializeField]
    CoordinateSystems m_primaryCoordinateSystem;

    public bool m_requireGeoPositionComponent = false;

    [SerializeField]
    AURORA_GeoPosition m_geoPosition;

    [SerializeField]
    protected bool m_useEulerAngles = false;

    protected bool m_useLocalTrackingMode = true;
    protected bool m_useLocalScaleMode = true;

    [SerializeField]
    private string m_uuid;

    private AURORA.Protobuf.UniqueIdentifier m_uid;

    [Tooltip("The number of seconds to wait between checks for object motion.")]
    [Range(0.03f, 1.0f)]
    [SerializeField]
    private float m_secondsBetweenMotionChecks;

    [Range(0.01f, 10.0f)]
    [SerializeField]
    private float m_positionChangeSensitivity;

    [Range(0.01f, 10.0f)]
    [SerializeField]
    private float m_rotationChangeSensitiity;

    [Range(0.01f, 10.0f)]
    [SerializeField]
    private float m_scaleChangeSensitivity;

    [Tooltip("Number of seconds between full updates.  Full updates are when all position, orientation, and scale values are broadcast.  Set to 0.0 to disable.")]
    [Range(0.0f, 30.0f)]
    [SerializeField]
    private float m_fullUpdateInterval;

    [SerializeField]
	private bool m_makeKinematicWhenNotOwned = false;

    [Tooltip("A Ghost Representation is a copy of the object that receives the transform updates echoed " +
        "by the server for this object.  Useful for testing network performance and comparing your objects " +
        "local transform against the transform recevied by remote users.")]
    [SerializeField]
    private AURORA_GameObject m_ghostRepresentation;

    [Tooltip("Read-Only Attribute - identifies the AURORA_GameObject which a ghost represents.")]
    [SerializeField]
    private AURORA_GameObject m_ghostBackReference;

    private bool m_ghostInitialized;

    [SerializeField]
    private bool m_enableIdleDeletion = false;

    [Range(1.0f, 600.0f)]
    [SerializeField]
    private float m_idleDeletionTimeoutSeconds = 60.0f;

    private float m_idleDeleteTime = 0.0f;

    [SerializeField]
    private AuroraObjectPropertyChangeEvents m_auroraObjectPropertyChangeEvents;

    [SerializeField]
    private ObjectOwnershipEvents m_objectOwnershipEvents;

    [SerializeField]
    private SceneObjectSpecificEvents m_sceneObjectSpecificEvents;

    [SerializeField]
    private SpawnableObjectSpecificEvents m_spawnableObjectSpecificEvents;

    [SerializeField]
    private ServerRegistrationEvents m_serverRegistrationEvents;

    [SerializeField]
    private ObjectDeletionEvents m_objectDeletionEvents;

    [SerializeField]
    private AuroraObjectMiscEvents m_miscellaneousEvents;

    

    private bool m_originalKinematicState = false;

	// Used to back off kinematic settings when grabbing is going on.
	//private AURORA_GrabInteractable m_auroraGrabInteractable;

	private float m_nextFullUpdateTime = 0.0f;

    private bool m_doFullUpdates = false;

    private bool m_terrainAdapterChanged = false;

    private int m_changeDetectionMask = 0;

    private float m_nextMotionDetectionTime = 0f;

    private Vector3 m_previousPosition;
    private Quaternion m_previousRotation;
    private Vector3 m_previousScale;

    private AURORA.ClientAPI.AuroraObject m_auroraObject;

    // float m_nextAuroraObjectLookup = 0f;

    private System.UInt32 m_lastAppliedUpdate = 0;

    //private bool m_resetTrackingRequested = false;

    private AuroraObject.ObjectRegistrationStatuses m_prevRegistrationStatus = AuroraObject.ObjectRegistrationStatuses.NOT_REGISTERED_WITH_SERVER;
    private AuroraObject.ObjectRegistrationStatuses m_registrationStatus = AuroraObject.ObjectRegistrationStatuses.NOT_REGISTERED_WITH_SERVER;

    private bool m_isRegistered = false;
	private bool m_isDestroyed = false;

	// Used to track ownership changesprivate System.Guid m_prevOwnerUuid;

	private Rigidbody m_rigidBody;

    private AURORA.ClientAPI.RpcRequest m_takeObjectOwnershipRequest;

    private bool m_serverSpawnAcknowledgementReceived = false;

    //private bool m_objectIsLocallyOwned;

    /// <summary>
    /// Used to force a transform update.  Particularly important when 
    /// ownership is first acquired.  Need to make sure position update
    /// didn't come down with ownership change update.
    /// </summary>
    private bool m_forceTransformUpdate = true;


    //public bool IsRegistered
    //{
    //    get
    //    {
    //        return m_isRegistered;
    //    }

    //    protected set
    //    {
    //        m_isRegistered = value;
    //    }
    //}

    public void Test(AURORA_GameObject ago)
    {
        Debug.LogWarning("AGO Registered " + ago.UUID);
    }

    public bool IsDestroyed
	{
		get
		{
			return m_isDestroyed;
		}
	}

    public AURORA_Context AURORA_Context
    {
        get
        {
            return m_auroraContext;
        }
        set
        {
            m_auroraContext = value;
        }
    }

	public bool MakeKinimaticWhenNotOwned
	{
		get
		{
			return m_makeKinematicWhenNotOwned;
		}
		set
		{
			m_makeKinematicWhenNotOwned = value;
		}
	}

    public System.Guid UUID
    {
        get
        {
            if (m_uid == null || m_uid.Uuid.IsEmpty)
                return System.Guid.Empty;

            return new System.Guid(m_uid.Uuid.ToByteArray());
        }
        set
        {
			m_uid = AURORA.ClientAPI.Tools.GuidToUniqueIdentifier(value);
			m_uuid = value.ToString();
        }
    }

	public float SecondsBetweenMotionChecks
	{
		get
		{
			return m_secondsBetweenMotionChecks;
		}
		set
		{
            if (m_rotationChangeSensitiity < 0.01f)
            {
                Debug.LogWarning("Time between motion checks set less than lower limit of 0.03.  Setting to lower limit.");
                m_secondsBetweenMotionChecks = 0.03f;
            }
            else
            {
                m_secondsBetweenMotionChecks = value;
            }
		}
	}

	public float PositionChangeSensitivity
	{
		get
		{
			return m_positionChangeSensitivity;
		}
		set
		{
            if (m_rotationChangeSensitiity < 0.01f)
            {
                Debug.LogWarning("Position Change Sensitive set less than lower limit of 0.01.  Setting to lower limit.");
                m_positionChangeSensitivity = 0.01f;
            }
            else
            {
                m_positionChangeSensitivity = value;
            }
		}
	}

	public float RotationChangeSensitiity
	{
		get
		{
			return m_rotationChangeSensitiity;
		}
		set
		{
            if(m_rotationChangeSensitiity < 0.01f)
            {
                Debug.LogWarning("Rotation Change Sensitive set less than lower limit of 0.01.  Setting to lower limit.");
                m_rotationChangeSensitiity = 0.01f;
            }
            else
            {
                m_rotationChangeSensitiity = value;
            }			
		}
	}

	public float ScaleChangeSensitivity
	{
		get
		{
			return m_scaleChangeSensitivity;
		}
		set
		{
            if (m_rotationChangeSensitiity < 0.01f)
            {
                Debug.LogWarning("Scale Change Sensitive set less than lower limit of 0.01.  Setting to lower limit.");
                m_scaleChangeSensitivity = 0.01f;
            }
            else
            {
                m_scaleChangeSensitivity = value;
            }
        }
	}

	protected AURORA_Context FindAuroraContextInParent()
    {
        return gameObject.GetComponentInParent<AURORA_Context>();
    }

    public AURORA_Spawnable AuroraSpawnable
    {
        get
        {
            return m_auroraSpawnable;
        }
    }

    public AURORA.ClientAPI.AuroraObject AURORA_Object
    {
        get
        {
            return m_auroraObject;
        }
        protected set
        {
            m_auroraObject = value;
           
            // Update m_uuid which is only for visibility in the Unity Editor
            if (m_auroraObject.ObjectUid != null)
            {
                if (m_uid == null)
                    m_uid = new AURORA.Protobuf.UniqueIdentifier();

                m_uid.MergeFrom(m_auroraObject.ObjectUid);

                m_uuid = AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(m_auroraObject.ObjectUid).ToString();
            }

            m_auroraObject.ObjectSpawnSuccessfulHandler = OnSpawnSuccessful;

            //m_auroraObject.ObjectOwnershipChangedHandler += UpdateObjectOwnership;
        }
    }

    public void OnSpawnSuccessful(AURORA.ClientAPI.AuroraObject auroraObject, AURORA.ClientAPI.RpcRequest rpcRequest)
    {
        // Yes, we are using the local m_auroraObject and not the one passed in becuase they should be the same
        // anyway.
        m_uuid = AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(m_auroraObject.ObjectUid).ToString();
    }

    public AURORA.Protobuf.UniqueIdentifier UID
    {
        get
        {
            return m_uid;
        }
    }

    public TransformTrackingModes TransformTrackingMode
    {
        get
        {
            return m_transformTrackingMode;
        }
        set
        {
            if(value == TransformTrackingModes.USE_LOCAL)
            {
                m_useLocalTrackingMode = true;
                m_useLocalScaleMode = true;
            }
            else if(value == TransformTrackingModes.USE_GLOBAL_LOCAL_SCALE)
            {
                m_useLocalTrackingMode = false;
                m_useLocalScaleMode = true;
            }
            else
            {
                if(value == TransformTrackingModes.USE_GLOBAL_LOSSY_SCALE)
                {
                    m_useLocalTrackingMode = false;
                    m_useLocalScaleMode = false;
                }
            }
        }
    }

    public AURORA_Manager AURORA_Manager
    {
        get
        {
            if (m_auroraManager == null)
                return m_auroraProxy.AURORA_Manager;

            return m_auroraManager;
        }
        set
        {
            m_auroraManager = value;
        }
    }

    public AURORA_Proxy AURORA_Proxy
    {
        get => m_auroraProxy;
        set => m_auroraProxy = value;
    }

    public bool DeleteOnServerWhenDestroyed
    {
        get
        {
            return m_deleteOnServerWhenDestroyed;
        }
        set
        {
            m_deleteOnServerWhenDestroyed = value;
        }
    }

    public float FullUpdateInterval
    {
        get
        {
            return m_fullUpdateInterval;
        }
        set
        {
            m_fullUpdateInterval = value;
            if (m_fullUpdateInterval < 0.0)
            {
                m_fullUpdateInterval = 0;
                Debug.LogWarning("Full Update Interval may not be set to a negative value.");
            }

            if(m_fullUpdateInterval > 30.0f)
            {
                m_fullUpdateInterval = 30.0f;
                Debug.LogWarning("Full Update Interval must be <= 30.0 seconds");
            }

            if(m_fullUpdateInterval == 0)
            {
                m_doFullUpdates = false;
            }
            else
            {
                m_doFullUpdates = true;
            }
        }
    }

    private void Reset()
    {
        m_auroraSpatialAnchor = GetComponent<AURORA_SpatialAnchor>();

        m_objectType = AURORA.Protobuf.ServerData.ObjectTypes.SpawnableObject;
        m_auroraContext = FindAuroraContextInParent();

		var proxygo = GameObject.Find("AURORA_Proxy");
		if (proxygo != null)
			m_auroraProxy = proxygo.GetComponent<AURORA_Proxy>();

		FindAuroraContextInParent();
        //m_uuid = System.Guid.NewGuid().ToString();

        m_secondsBetweenMotionChecks = 0.06f;
        
        m_positionChangeSensitivity = 0.01f;

        m_rotationChangeSensitiity = 0.01f;

        m_scaleChangeSensitivity = 0.01f;

        m_fullUpdateInterval = 5.0f;

        m_enableIdleDeletion = false;

        m_idleDeletionTimeoutSeconds = 30.0f;
    }

    private void Awake()
    {
        m_isRegistered = false;
        m_registrationStatus = AuroraObject.ObjectRegistrationStatuses.NOT_REGISTERED_WITH_SERVER;
		m_isDestroyed = false;

        if(m_auroraSpatialAnchor == null)
        {
            m_auroraSpatialAnchor = GetComponent<AURORA_SpatialAnchor>();

            if(m_auroraSpatialAnchor == null)
            {
                m_auroraSpatialAnchor = gameObject.AddComponent<AURORA_SpatialAnchor>();
            }
        }

        if (m_geoPosition == null)
        {
            m_geoPosition = GetComponent<AURORA_GeoPosition>();

            if (m_geoPosition == null && m_requireGeoPositionComponent == true)
            {
                m_geoPosition = gameObject.AddComponent<AURORA_GeoPosition>();
            }
        }

        if (m_auroraProxy == null)
        {
            var go = GameObject.Find("AURORA_Proxy");
            if (go != null)
                m_auroraProxy = go.GetComponent<AURORA_Proxy>();
            else
                Debug.LogError("Could not find AURORA Proxy!");
        }

		if (m_auroraContext == null)
		{
			m_auroraContext = FindAuroraContextInParent();

			// If that didn't work, use proxy default
			if (m_auroraContext == null && m_auroraProxy != null)
			{
				m_auroraContext = m_auroraProxy.PrimaryAuroraContext;
			}
		}

		m_uid = new AURORA.Protobuf.UniqueIdentifier();

        //Over uuid if this was spawnable
        if (m_objectType == AURORA.Protobuf.ServerData.ObjectTypes.SpawnableObject)
        {
            // Will be set by server when spawned
            UUID = System.Guid.Empty;
        }
        else if(m_objectType == AURORA.Protobuf.ServerData.ObjectTypes.SceneObject || m_objectType == AURORA.Protobuf.ServerData.ObjectTypes.GhostObject)
        {
            // If a uuid value is specified in the editor, copy it over here.
            if (m_uuid != "")
                UUID = new System.Guid(m_uuid);
        }
        else
        {
            throw new AURORA.AuroraException("Unknown object type");
        }
        
        if (m_auroraManager == null)
        {
            if(m_auroraProxy != null)
            {
                m_auroraManager = m_auroraProxy.AURORA_Manager;
            }
        }
        m_auroraObject = null;

        // If spawnable is not set see if we have one available on this object.
        if(m_auroraSpawnable == null)
        {
            m_auroraSpawnable = GetComponent<AURORA_Spawnable>();
        }

        //if(m_auroraGrabInteractable == null)
    	//	m_auroraGrabInteractable = GetComponent<AURORA_GrabInteractable>();

		m_rigidBody = GetComponent<Rigidbody>();

		if(m_rigidBody != null)
		{
			m_originalKinematicState = m_rigidBody.isKinematic;
		}
        //InitializeAuroraObject();
    } 
    // Start is called before the first frame update
    void Start()
    {
        m_serverSpawnAcknowledgementReceived = false;

        if (m_positionChangeSensitivity < 0.01f)
        {
            Debug.LogWarning("On " + name + " position change sensitivity below lower limit.  Setting to lower limit.");
            m_positionChangeSensitivity = 0.01f;
        }
        if(m_rotationChangeSensitiity < 0.01f)
        {
            Debug.LogWarning("On " + name + " rotation change sensitivity below lower limit.  Setting to lower limit.");
            m_rotationChangeSensitiity = 0.01f;
        }
        if(m_scaleChangeSensitivity < 0.01f)
        {
            Debug.LogWarning("On " + name + " scale change sensitivity below lower limit.  Setting to lower limit.");
            m_scaleChangeSensitivity = 0.01f;
        }
        if(m_secondsBetweenMotionChecks < 0.03f)
        {
            Debug.LogWarning("On " + name + " seconds between motion checks below lower limit.  Setting to lower limit.");
            m_secondsBetweenMotionChecks = 0.03f;
        }

        m_ghostInitialized = false;

		if (m_auroraProxy == null)
		{
			var go = GameObject.Find("AURORA_Proxy");
			if (go != null)
				m_auroraProxy = go.GetComponent<AURORA_Proxy>();
			else
				Debug.LogError("Could not find AURORA Proxy!");
		}

        if (m_auroraContext == null)
        {
            m_auroraContext = FindAuroraContextInParent();

            // If that didn't work, use proxy default
            if (m_auroraContext == null && m_auroraProxy != null)
            {
                m_auroraContext = m_auroraProxy.PrimaryAuroraContext;
            }
        }

		m_nextMotionDetectionTime = m_secondsBetweenMotionChecks;
        //m_nextAuroraObjectLookup = 0f;

        m_changeDetectionMask = AURORA_Tools.ALL_MASK;

        m_previousPosition = Vector3.zero;
        m_previousRotation = Quaternion.identity;
        m_previousScale = Vector3.one;

        if (m_fullUpdateInterval > 0.0)
        {
            m_doFullUpdates = true;
            // Use a random start time to prevent all items from doing updates at the same time
            // AKA be nice to the network
            m_nextFullUpdateTime = Time.time + Random.Range(0.0f, m_fullUpdateInterval);
        }
        else
        {
            m_doFullUpdates = false;
        }

		InitializeAuroraObject();

        m_idleDeleteTime = Time.time + m_idleDeletionTimeoutSeconds;
	}


	//private void OnApplicationQuit()
	private void OnDestroy()
	{
        AuroraShutdown();
    }

    public void AuroraShutdown()
    {
		if (!m_isDestroyed)
		{
            if (m_auroraContext != null)
			{
				if (m_deleteOnServerWhenDestroyed)
				{
					AuroraDestroy();
				}
				m_auroraContext.UnregisterAuroraGameObject(this);
				//m_isRegistered = false;
				m_isDestroyed = true;
			}
		}
	}

	public bool AuroraDestroy()
	{
		if (!m_isDestroyed && m_auroraObject != null && m_auroraObject.ObjectType != AURORA.Protobuf.ServerData.ObjectTypes.GhostObject)
		{
            // TODO Rethink how to do this.
            //if (RequestObjectOwnership())
            m_auroraObject.DestroyObject();
            //m_auroraContext.AuroraContext.DestroyObject(m_auroraObject.UUID);
		}

		return false;
	}

	public AURORA.Protobuf.Transform GetAuroraTransform()
    {
        if (m_geoPosition != null)
        {
            return AURORA_Tools.MakeAuroraTransform(m_auroraSpatialAnchor.transform, m_geoPosition.GeoCoordinate,
                m_useLocalTrackingMode, m_useEulerAngles, !m_useLocalScaleMode, m_changeDetectionMask);
        }
        else
        {
            return AURORA_Tools.MakeAuroraTransform(m_auroraSpatialAnchor.transform, null,
                m_useLocalTrackingMode, m_useEulerAngles, !m_useLocalScaleMode, m_changeDetectionMask);
        }
    }

    public void Initialize(AURORA.ClientAPI.AuroraObject auroraObject, AURORA_Context context)
    {
        if (m_auroraObject != null)
        {
            Debug.LogError("AURORA_GameObject " + gameObject.name + " already initialized with an AuroraObject.");
        }
        else
        {
            if (auroraObject != null)
            {
                m_auroraContext = context;
                m_auroraObject = auroraObject;

                // This is required
                m_serverRegistrationEvents.ObjectRegistrationStatusChanged.EventHandlers.AddListener(this.ObjectRegistrationStatusChanged);

                if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.NOT_REGISTERED_WITH_SERVER
                    ||
                    m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_REQUESTED)
                {
                    if (m_objectType == AURORA.Protobuf.ServerData.ObjectTypes.SceneObject)
                    {
                        m_auroraObject.RegisterSceneObjectSuccessfulHandler += AuroraObjectInititalized;
                    }
                    else if (m_objectType == AURORA.Protobuf.ServerData.ObjectTypes.SpawnableObject)
                    {
                        m_auroraObject.ObjectSpawnSuccessfulHandler += AuroraObjectInititalized;
                    }
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTERED_WITH_SERVER)
                {
                    m_isRegistered = true;
                    m_isDestroyed = false;
                    //m_auroraContext.RegisterAuroraGameObject(this);
                    UUID = m_auroraObject.UUID;
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.DELETED_ON_SERVER)
                {
                    m_isRegistered = true;
                    m_isDestroyed = true;
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_REQUESTED)
                {
                    // We probably got here because of a spawn.  The spawner already wired up the callback for registration... hope it happens.
                    m_isDestroyed = false;
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
                else
                {
                    Debug.LogWarning("AuroraObject " + gameObject.name + " in unexpected registration state " + m_auroraObject.RegistrationStatus.ToString());
                }

                m_auroraObject.ObjectUidChangedHandler += OnAuroraObjectUidChanged;

                // Inititalize Panels
                m_auroraObjectPropertyChangeEvents.Initialize(this);
                m_objectOwnershipEvents.Initialize(this);
                m_sceneObjectSpecificEvents.Initialize(this);
                m_spawnableObjectSpecificEvents.Initialize(this);
                m_serverRegistrationEvents.Initialize(this);
                m_objectDeletionEvents.Initialize(this);
                m_miscellaneousEvents.Initialize(this);
            }
            else
            {
                Debug.LogError("Cannot initialize with a null AuroraObject.");
            }
        }
    }

    protected void InitializeAuroraObject()
    {
        //if (m_registrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTERED_WITH_SERVER)
        //{
        //    Debug.Log("Already registered");
        //    return;
        //}
        //else if(m_registrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_REQUESTED)
        //{
        //    Debug.Log("Waiting for object registration results from server.");
        //    return;
        //}
        //else if(m_registrationStatus == AuroraObject.ObjectRegistrationStatuses.DELETED_ON_SERVER)
        //{
        //    Debug.LogWarning("Object deleted on server.  Cannot reinitialize.  Create a new object instead.");
        //    return;
        //}
        //else if(m_registrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_FAILED)
        //{
        //    Debug.LogError("Registration failed.");
        //    return;
        //}
        //else if(m_registrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_TIMED_OUT)
        //{
        //    Debug.LogError("Registration timed out.");
        //    return;
        //}       

        if (m_auroraObject != null)
        {
            Debug.LogWarning("AURORA already initialized");
        }
        else
        {
            if (m_auroraContext != null)
            {
                if (m_auroraObject == null)
                {
                    // We need to create this object.
                    if (m_objectType == AURORA.Protobuf.ServerData.ObjectTypes.SceneObject)
                    {
                        m_auroraObject = m_auroraContext.AuroraContext.RegisterSceneObject(
                            m_uid, GetAuroraTransform()
                        );

                        m_auroraObject.RegisterSceneObjectSuccessfulHandler += AuroraObjectInititalized;
                    }
                    else if (m_objectType == AURORA.Protobuf.ServerData.ObjectTypes.SpawnableObject)
                    {
                        m_auroraContext.SpawnRemoteOnly(this);

                        m_auroraObject.ObjectSpawnSuccessfulHandler += AuroraObjectInititalized;
                    }
                    else if (m_objectType == AURORA.Protobuf.ServerData.ObjectTypes.GhostObject)
                    {
                        m_auroraObject = m_auroraContext.AuroraContext.PrepareGhostObject(GetAuroraTransform());
                        m_isRegistered = true;
                    }

                    if (m_auroraObject == null)
                    {
                        throw new AURORA.AuroraException("Failed to create AuroraObject");
                    }
                }
                else
                {
                    ProcessRegistrationStatusChanges();
                }

                m_auroraObject.ObjectUidChangedHandler += OnAuroraObjectUidChanged;

                // Inititalize Panels
                m_auroraObjectPropertyChangeEvents.Initialize(this);
                m_objectOwnershipEvents.Initialize(this);
                m_sceneObjectSpecificEvents.Initialize(this);
                m_spawnableObjectSpecificEvents.Initialize(this);
                m_serverRegistrationEvents.Initialize(this);
                m_objectDeletionEvents.Initialize(this);
                m_miscellaneousEvents.Initialize(this);
            }
            else
            {
                Debug.LogError("Cannot create AURORA Game Object " + this.name + ", no context available.");
            }
        }
    }

    protected void OnAuroraObjectUidChanged(AURORA.ClientAPI.AuroraObject ao, System.Guid guid)
    {
        //m_uid = Tools.GuidToUniqueIdentifier(guid);
        UUID = guid;
        m_serverSpawnAcknowledgementReceived = true;
    }

    internal void AuroraObjectInititalized(AURORA.ClientAPI.AuroraObject auroraObject, RpcRequest rpcRequest)
    {
        ProcessRegistrationStatusChanges();
    }

    private void ObjectRegistrationStatusChanged(AURORA_GameObject ago)
    {   
        if(ago.GetHashCode() != this.GetHashCode())
        {
            Debug.LogError(gameObject.name + " got an event for an other object than itself.");
        }
        else
            ProcessRegistrationStatusChanges();
    }

    private void ProcessRegistrationStatusChanges()
    {
        //if(!m_serverSpawnAcknowledgementReceived)
        //{
        //    return;
        //}

        if(m_auroraObject != null)
        {
            m_registrationStatus = m_auroraObject.RegistrationStatus;
        }

        if (m_prevRegistrationStatus != m_registrationStatus)
        {
            // Handle various from<=>state transitions
            // Not all of these will be valid, but since they could happen, we include them all
            // Something else already created the object.
            

            if (m_prevRegistrationStatus == AuroraObject.ObjectRegistrationStatuses.NOT_REGISTERED_WITH_SERVER)
            {
                if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_REQUESTED)
                {
                    m_isRegistered = false;
                    m_isDestroyed = false;
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTERED_WITH_SERVER)
                {
                    m_isRegistered = true;
                    m_isDestroyed = false;

                    // Update our object uuid on the game object to match that of the underlying auroraXR Object
                    UUID = m_auroraObject.UUID;
                    //m_auroraContext.RegisterAuroraGameObject(this);
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_FAILED)
                {
                    m_isRegistered = false;
                    m_isDestroyed = false;
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_TIMED_OUT)
                {
                    m_isRegistered = false;
                    m_isDestroyed = false;
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.DELETED_ON_SERVER)
                {
                    m_isRegistered = false;
                    m_isDestroyed = true;
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
            }
            else if (m_prevRegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_REQUESTED)
            {   
                if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.NOT_REGISTERED_WITH_SERVER)
                {
                    m_isRegistered = false;
                    m_isDestroyed = false;
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTERED_WITH_SERVER)
                {
                    m_isRegistered = true;
                    m_isDestroyed = false;

                    // Update our object uuid on the game object to match that of the underlying auroraXR Object
                    UUID = m_auroraObject.UUID;
                    //m_auroraContext.RegisterAuroraGameObject(this);
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_FAILED)
                {
                    m_isRegistered = false;
                    m_isDestroyed = false;
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_TIMED_OUT)
                {
                    m_isRegistered = false;
                    m_isDestroyed = false;
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.DELETED_ON_SERVER)
                {
                    m_isRegistered = false;
                    m_isDestroyed = true;
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
            }
            else if(m_prevRegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTERED_WITH_SERVER)
            {
                if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.NOT_REGISTERED_WITH_SERVER)
                {
                    m_isRegistered = false;
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_REQUESTED)
                {
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_FAILED)
                {
                    m_auroraContext.UnregisterAuroraGameObject(this);
                    throw new System.NotImplementedException();
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_TIMED_OUT)
                {
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.DELETED_ON_SERVER)
                {
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
            }
            else if(m_prevRegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_FAILED)
            {
                if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.NOT_REGISTERED_WITH_SERVER)
                {
                    throw new System.NotImplementedException();
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_REQUESTED)
                {
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTERED_WITH_SERVER)
                {
                    m_isRegistered = true;
                    UUID = m_auroraObject.UUID;
                    //m_auroraContext.RegisterAuroraGameObject(this);
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_TIMED_OUT)
                {
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.DELETED_ON_SERVER)
                {
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
            }
            else if(m_prevRegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_TIMED_OUT)
            {
                if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.NOT_REGISTERED_WITH_SERVER)
                {

                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_REQUESTED)
                {
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTERED_WITH_SERVER)
                {
                    m_isRegistered = true;
                    UUID = m_auroraObject.UUID;
                    //m_auroraContext.RegisterAuroraGameObject(this);
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_FAILED)
                {
                    m_auroraContext.UnregisterAuroraGameObject(this);
                    throw new System.NotImplementedException();
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.DELETED_ON_SERVER)
                {
                    m_auroraContext.UnregisterAuroraGameObject(this);
                }
            }
            else if(m_prevRegistrationStatus == AuroraObject.ObjectRegistrationStatuses.DELETED_ON_SERVER)
            {
                if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.NOT_REGISTERED_WITH_SERVER)
                {

                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_REQUESTED)
                {
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTERED_WITH_SERVER)
                {
                    m_isRegistered = true;
                    UUID = m_auroraObject.UUID;
                    //m_auroraContext.RegisterAuroraGameObject(this);
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_FAILED)
                {
                    throw new System.NotImplementedException();
                }
                else if (m_auroraObject.RegistrationStatus == AuroraObject.ObjectRegistrationStatuses.REGISTRATION_TIMED_OUT)
                {

                }
            }

            m_prevRegistrationStatus = m_registrationStatus;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //m_readOnlyIsLocallyOwned = m_auroraObject.IsLocallyOwned;

        // Dispatch any queued events.
        try
        {
            m_auroraObjectPropertyChangeEvents.Invoke();
            m_objectOwnershipEvents.Invoke();
            m_sceneObjectSpecificEvents.Invoke();
            m_spawnableObjectSpecificEvents.Invoke();
            m_serverRegistrationEvents.Invoke();
            m_objectDeletionEvents.Invoke();
            m_miscellaneousEvents.Invoke();
        }
        catch(System.Exception ex)
        {
            m_auroraContext.AuroraLogger.AuroraLogger.LogException(ex);
        }

        if(m_isRegistered)
        {
            bool nonIdlenessDetected = false;

            if (!m_ghostInitialized)
            {
                if (m_ghostRepresentation != null)
                {
                    // If the other object has been configured, set things up here.
                    if (m_ghostRepresentation.AURORA_Object != null)
                    {
                        if (m_ghostRepresentation.ObjectType != AURORA.Protobuf.ServerData.ObjectTypes.GhostObject)
                        {
                            throw new AURORA.AuroraException(m_ghostRepresentation.name + " set as a Ghost Representation for " + name + " but is not set to GHOST_OBJECT");
                        }
                        m_auroraObject.GhostRepresentation = m_ghostRepresentation.AURORA_Object;
                        m_ghostRepresentation.m_ghostBackReference = this;
                        m_ghostInitialized = true;
                    }
                    // Otherwise, try again later.
                }
                else
                {
                    // Ghosting not configured
                    m_ghostInitialized = true;
                }                
            }

            if (m_terrainAdapterChanged)
            {
                m_terrainAdapterChanged = false;
                if (m_geoPosition != null)
                {
                    m_geoPosition.GeoCoordinate = m_auroraObject.Transform.GeoCoordinate;

                    m_geoPosition.UpdateWorldPositionFromGeoCoordinate();

                    nonIdlenessDetected = true;
                }
            }

            if (!m_forceTransformUpdate && IsLocallyOwned)
            {
                // Check for ownership changes if make kinematic when not owned is set
                //if (m_makeKinematicWhenNotOwned)
                //{
                //    if (m_rigidBody != null)
                //    {
                //        if (m_auroraGrabInteractable == null || !m_auroraGrabInteractable.isGrabbed)
                //        {
                //            m_rigidBody.isKinematic = m_originalKinematicState;
                //        }
                //    }
                //}

				//If here we are the owner of the object
				bool forceFullUpdate = false;
                bool changeDetected = false;

                //if (m_doFullUpdates && Time.time >= m_nextFullUpdateTime)
                //{
                //    m_nextFullUpdateTime = Time.time + m_fullUpdateInterval;
                //    forceFullUpdate = true;
                //}

                if (!forceFullUpdate && Time.time >= m_nextMotionDetectionTime)
                {
                    m_nextMotionDetectionTime = Time.time + m_secondsBetweenMotionChecks;

                    if (m_useLocalTrackingMode)
                    {
                        if (Mathf.Abs(m_auroraSpatialAnchor.transform.localPosition.x - m_previousPosition.x) >= m_positionChangeSensitivity)
                        {
                            m_changeDetectionMask |= AURORA_Tools.POSITION_MASK;
                            changeDetected = true;

                            m_previousPosition = m_auroraSpatialAnchor.transform.localPosition;
                        }
                        else if (Mathf.Abs(m_auroraSpatialAnchor.transform.localPosition.y - m_previousPosition.y) >= m_positionChangeSensitivity)
                        {
                            m_changeDetectionMask |= AURORA_Tools.POSITION_MASK;
                            changeDetected = true;

                            m_previousPosition = m_auroraSpatialAnchor.transform.localPosition;
                        }
                        else if (Mathf.Abs(m_auroraSpatialAnchor.transform.localPosition.z - m_previousPosition.z) >= m_positionChangeSensitivity)
                        {
                            m_changeDetectionMask |= AURORA_Tools.POSITION_MASK;
                            changeDetected = true;

                            m_previousPosition = m_auroraSpatialAnchor.transform.localPosition;
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(m_auroraSpatialAnchor.transform.position.x - m_previousPosition.x) >= m_positionChangeSensitivity)
                        {
                            m_changeDetectionMask |= AURORA_Tools.POSITION_MASK;
                            changeDetected = true;

                            m_previousPosition = m_auroraSpatialAnchor.transform.position;
                        }
                        else if (Mathf.Abs(m_auroraSpatialAnchor.transform.position.y - m_previousPosition.y) >= m_positionChangeSensitivity)
                        {
                            m_changeDetectionMask |= AURORA_Tools.POSITION_MASK;
                            changeDetected = true;

                            m_previousPosition = m_auroraSpatialAnchor.transform.position;
                        }
                        else if (Mathf.Abs(m_auroraSpatialAnchor.transform.position.z - m_previousPosition.z) >= m_positionChangeSensitivity)
                        {
                            m_changeDetectionMask |= AURORA_Tools.POSITION_MASK;
                            changeDetected = true;

                            m_previousPosition = m_auroraSpatialAnchor.transform.position;
                        }
                    }

                    if (m_useEulerAngles)
                    {
                        if (m_useLocalTrackingMode)
                        {
                            if (Mathf.Abs(m_auroraSpatialAnchor.transform.localEulerAngles.x - m_previousRotation.x) >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation.x = m_auroraSpatialAnchor.transform.localEulerAngles.x;
                                m_previousRotation.y = m_auroraSpatialAnchor.transform.localEulerAngles.y;
                                m_previousRotation.z = m_auroraSpatialAnchor.transform.localEulerAngles.z;
                            }
                            else if (Mathf.Abs(m_auroraSpatialAnchor.transform.localEulerAngles.y - m_previousRotation.y) >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation.x = m_auroraSpatialAnchor.transform.localEulerAngles.x;
                                m_previousRotation.y = m_auroraSpatialAnchor.transform.localEulerAngles.y;
                                m_previousRotation.z = m_auroraSpatialAnchor.transform.localEulerAngles.z;
                            }
                            else if (Mathf.Abs(m_auroraSpatialAnchor.transform.localEulerAngles.z - m_previousRotation.z) >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation.x = m_auroraSpatialAnchor.transform.localEulerAngles.x;
                                m_previousRotation.y = m_auroraSpatialAnchor.transform.localEulerAngles.y;
                                m_previousRotation.z = m_auroraSpatialAnchor.transform.localEulerAngles.z;
                            }
                        }
                        else
                        {
                            if (m_auroraSpatialAnchor.transform.eulerAngles.x - m_previousRotation.x >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation.x = m_auroraSpatialAnchor.transform.eulerAngles.x;
                                m_previousRotation.y = m_auroraSpatialAnchor.transform.eulerAngles.y;
                                m_previousRotation.z = m_auroraSpatialAnchor.transform.eulerAngles.z;
                            }
                            else if (m_auroraSpatialAnchor.transform.eulerAngles.y - m_previousRotation.y >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation.x = m_auroraSpatialAnchor.transform.eulerAngles.x;
                                m_previousRotation.y = m_auroraSpatialAnchor.transform.eulerAngles.y;
                                m_previousRotation.z = m_auroraSpatialAnchor.transform.eulerAngles.z;
                            }
                            else if (m_auroraSpatialAnchor.transform.eulerAngles.z - m_previousRotation.z >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation.x = m_auroraSpatialAnchor.transform.eulerAngles.x;
                                m_previousRotation.y = m_auroraSpatialAnchor.transform.eulerAngles.y;
                                m_previousRotation.z = m_auroraSpatialAnchor.transform.eulerAngles.z;
                            }
                        }
                    }
                    else
                    {
                        if (m_useLocalTrackingMode)
                        {
                            if (Mathf.Abs(m_auroraSpatialAnchor.transform.localRotation.x - m_previousRotation.x) >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation = m_auroraSpatialAnchor.transform.localRotation;
                            }
                            else if (Mathf.Abs(m_auroraSpatialAnchor.transform.localRotation.y - m_previousRotation.y) >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation = m_auroraSpatialAnchor.transform.localRotation;
                            }
                            else if (Mathf.Abs(m_auroraSpatialAnchor.transform.localRotation.z - m_previousRotation.z) >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation = m_auroraSpatialAnchor.transform.localRotation;
                            }
                            else if (Mathf.Abs(m_auroraSpatialAnchor.transform.localRotation.w - m_previousRotation.w) >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation = m_auroraSpatialAnchor.transform.localRotation;
                            }
                        }
                        else
                        {
                            if (Mathf.Abs(m_auroraSpatialAnchor.transform.rotation.x - m_previousRotation.x) >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation = m_auroraSpatialAnchor.transform.rotation;
                            }
                            else if (Mathf.Abs(m_auroraSpatialAnchor.transform.rotation.y - m_previousRotation.y) >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation = m_auroraSpatialAnchor.transform.rotation;
                            }
                            else if (Mathf.Abs(m_auroraSpatialAnchor.transform.rotation.z - m_previousRotation.z) >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation = m_auroraSpatialAnchor.transform.rotation;
                            }
                            else if (Mathf.Abs(m_auroraSpatialAnchor.transform.rotation.w - m_previousRotation.w) >= m_rotationChangeSensitiity)
                            {
                                m_changeDetectionMask |= AURORA_Tools.ROTATION_MASK;
                                changeDetected = true;

                                m_previousRotation = m_auroraSpatialAnchor.transform.rotation;
                            }
                        }
                    }

                    if (m_useLocalScaleMode)
                    {
                        if (Mathf.Abs(m_auroraSpatialAnchor.transform.localScale.x - m_previousScale.x) >= m_scaleChangeSensitivity)
                        {
                            m_changeDetectionMask |= AURORA_Tools.SCALE_MASK;
                            changeDetected = true;

                            m_previousScale = m_auroraSpatialAnchor.transform.localScale;
                        }
                        else if (Mathf.Abs(m_auroraSpatialAnchor.transform.localScale.y - m_previousScale.y) >= m_scaleChangeSensitivity)
                        {
                            m_changeDetectionMask |= AURORA_Tools.SCALE_MASK;
                            changeDetected = true;

                            m_previousScale = m_auroraSpatialAnchor.transform.localScale;
                        }
                        else if (Mathf.Abs(m_auroraSpatialAnchor.transform.localScale.z - m_previousScale.z) >= m_scaleChangeSensitivity)
                        {
                            m_changeDetectionMask |= AURORA_Tools.SCALE_MASK;
                            changeDetected = true;

                            m_previousScale = m_auroraSpatialAnchor.transform.localScale;
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(m_auroraSpatialAnchor.transform.lossyScale.x - m_previousScale.x) >= m_scaleChangeSensitivity)
                        {
                            m_changeDetectionMask |= AURORA_Tools.SCALE_MASK;
                            changeDetected = true;

                            m_previousScale = m_auroraSpatialAnchor.transform.lossyScale;
                        }
                        else if (Mathf.Abs(m_auroraSpatialAnchor.transform.lossyScale.y - m_previousScale.y) >= m_scaleChangeSensitivity)
                        {
                            m_changeDetectionMask |= AURORA_Tools.SCALE_MASK;
                            changeDetected = true;

                            m_previousScale = m_auroraSpatialAnchor.transform.lossyScale;
                        }
                        else if (Mathf.Abs(m_auroraSpatialAnchor.transform.lossyScale.z - m_previousScale.z) >= m_scaleChangeSensitivity)
                        {
                            m_changeDetectionMask |= AURORA_Tools.SCALE_MASK;
                            changeDetected = true;

                            m_previousScale = m_auroraSpatialAnchor.transform.lossyScale;
                        }
                    }
                }

                // Check for metadata changes
                if(m_auroraObject.MetadataCollection != null)
                {
                    if(m_auroraObject.MetadataCollection.HaveChangesToPublish)
                    {
                        m_changeDetectionMask |= AURORA_Tools.METADATA_MASK;
                    }
                }
                
                if (forceFullUpdate)
                {
                    changeDetected = true;
                    m_changeDetectionMask = AURORA_Tools.ALL_MASK;
                    // Debug.Log("Forced Full Update");
                }

                if (changeDetected)
                {
                    //If we have geoposition allow it to update only if the virutal position has changed
                    if (m_geoPosition != null)
                    {                        
                        if (m_geoPosition.UpdateGeoCoordinateFromWorldPosition())
                        {
                            m_changeDetectionMask |= AURORA_Tools.GEOPOSITION_MASK;
                        }
                    }

                    //Debug.Log("Change Detected");

                    //Apply the transform to the object.
                    m_auroraObject.ApplyTransformUpdate(GetAuroraTransform());

                    //m_auroraObject.ApplyTransformUpdate(GetAuroraTransform());

                    // Schedule for the update to be published to the server
                    // m_auroraManager.AURORA_Interface.AuroraObjectManager.RequestTransformPublishFor(m_auroraObject);
                    m_auroraContext.AuroraObjectManager.RequestAuroraObjectUpdatePublishFor(m_auroraObject);

                    //Clear detection mask
                    m_changeDetectionMask = 0;
                }
            }
            else
            {
#if USE_OVR
				// If the object is grabbable and we had grabbed it, that needs to stop because someone else owns it now.
				if (!m_forceTransformUpdate && m_auroraGrabbable != null && m_auroraGrabbable.isGrabbed)
				{
					m_auroraGrabbable.grabbedBy.ForceRelease(m_auroraGrabbable);
				}
#endif

				// Check for ownership changes if make kinematic when not owned is set
				if (!m_forceTransformUpdate && m_makeKinematicWhenNotOwned)
				{
					// Has the owner changed since last we checked?
					//if(m_prevOwnerUuid != m_auroraObject.UUID)
					//{
						// Yes, owner has changed.  We are in this code block only because the new owner is not us.
						// Update previous owner
						//m_prevOwnerUuid = m_auroraObject.UUID;

						// Keep evaluating only if this GameObject has a rigid body
						if(m_rigidBody != null)
						{
							m_rigidBody.isKinematic = true;
						}
					//}
				}

				// Has the object transform changed since last we checked?
				if (m_auroraObject.TransformUpdatedLast > m_lastAppliedUpdate || m_forceTransformUpdate)
                {
                    nonIdlenessDetected = true;
                    //m_auroraManager.RecordMessageProcessed();
                    //m_auroraManager.RecordMessagesDiscarded(m_auroraObject.TransformUpdatedLast - m_lastAppliedUpdate - 1);

                    //Cache all of these now because the will all be reset on net get.
                    bool localPositionUpdatedSinceLastGet = m_auroraObject.LocalPositionUpdatedLast > m_lastAppliedUpdate;
                    bool localScaleUpdatedSinceLastGet = m_auroraObject.LocalScaleUpdatedLast > m_lastAppliedUpdate;
                    bool rotationUpdatedSinceLastGet = m_auroraObject.RotationUpdatedLast > m_lastAppliedUpdate; ;
                    bool localEulerUpdatesSinceLastGet = m_auroraObject.RotationUpdatedLast > m_lastAppliedUpdate; ;
                    bool localRotationUpdatedSinceLastGet = m_auroraObject.LocalRotationUpdatedLast > m_lastAppliedUpdate;
                    bool geoCoordinateUpdatedSinceLastGet = m_auroraObject.GeoCoordinateUpdatedLast > m_lastAppliedUpdate;

                    //Update last update value
                    m_lastAppliedUpdate = m_auroraObject.TransformUpdatedLast;

                    if(geoCoordinateUpdatedSinceLastGet)
                    {
                        if(m_auroraObject.Transform.GeoCoordinate == null)
                        {
                            m_auroraObject.Transform.GeoCoordinate = new AURORA.Protobuf.GeoSpatial.GeoCoordinate();
                        }

                        // We update the geocoordinate data whether or not we use it to position the object.
                        m_auroraObject.Transform.GeoCoordinate.MergeFrom(m_auroraObject.Transform.GeoCoordinate);
                    }

                    if (m_primaryCoordinateSystem == CoordinateSystems.VIRTUAL_WORLD_COORDINATES)
                    {
                        AURORA.Protobuf.Transform atrans = m_auroraObject.Transform;

                        if (localPositionUpdatedSinceLastGet)
                        {
                            if (atrans.LocalPosition != null)
                            {
                                transform.localPosition = new Vector3(
                                    atrans.LocalPosition.X,
                                    atrans.LocalPosition.Y,
                                    atrans.LocalPosition.Z
                                );
                            }
                        }

                        if (localEulerUpdatesSinceLastGet)
                        {
                            if (atrans.LocalEulerAngles != null)
                            {
                                transform.localEulerAngles = new Vector3(
                                    atrans.LocalEulerAngles.X,
                                    atrans.LocalEulerAngles.Y,
                                    atrans.LocalEulerAngles.Z
                                    );
                            }
                        }


                        if (rotationUpdatedSinceLastGet)
                        {
                            if (atrans.Rotation != null)
                            {
                                transform.rotation = new Quaternion(
                                    atrans.Rotation.X,
                                    atrans.Rotation.Y,
                                    atrans.Rotation.Z,
                                    atrans.Rotation.W
                                    );
                            }
                        }

                        if (localRotationUpdatedSinceLastGet)
                        {
                            if (atrans.LocalRotation != null)
                            {
                                transform.localRotation = new Quaternion(
                                    atrans.LocalRotation.X,
                                    atrans.LocalRotation.Y,
                                    atrans.LocalRotation.Z,
                                    atrans.LocalRotation.W
                                    );
                            }
                        }

                        if (localScaleUpdatedSinceLastGet)
                        {
                            if (atrans.LocalScale != null)
                            {
                                transform.localScale = new Vector3(
                                    atrans.LocalScale.X,
                                    atrans.LocalScale.Y,
                                    atrans.LocalScale.Z
                                    );
                            }
                        }
                    } 
                    else if(m_primaryCoordinateSystem == CoordinateSystems.GEOSPATIAL_CORDINATES)
                    {
                        if(m_geoPosition != null)
                        {
                            m_geoPosition.GeoCoordinate = m_auroraObject.Transform.GeoCoordinate;

                            m_geoPosition.UpdateWorldPositionFromGeoCoordinate();                         
                        }
                    }

                    //It has been done
                    m_forceTransformUpdate = false;
                }

                if (nonIdlenessDetected)
                {
                    m_idleDeleteTime = Time.time + m_idleDeletionTimeoutSeconds;
                }
                else
                {
                    if(m_enableIdleDeletion && Time.time >= m_idleDeleteTime)
                    {
                        m_idleDeleteTime = Time.time + m_idleDeletionTimeoutSeconds;
                        m_auroraContext.RemoveObjectLocally(this);
                    }
                }
            }
        }
        else
        {
            ProcessRegistrationStatusChanges();
        }
    }

    public bool IsLocallyOwned
    {
        get
        {
            if (!m_isRegistered)
            {
                return false; 
            }
            return m_auroraObject.IsLocallyOwned;
        }
    }

    public AuroraObjectPropertyChangeEvents AuroraObjectPropertyChangeEvents { get => m_auroraObjectPropertyChangeEvents;}
    public ObjectOwnershipEvents ObjectOwnershipEvents { get => m_objectOwnershipEvents; }
    public SceneObjectSpecificEvents SceneObjectSpecificEvents { get => m_sceneObjectSpecificEvents; }
    public SpawnableObjectSpecificEvents SpawnableObjectSpecificEvents { get => m_spawnableObjectSpecificEvents;}
    public ServerRegistrationEvents ServerRegistrationEvents { get => m_serverRegistrationEvents; }
    public ObjectDeletionEvents ObjectDeletionEvents { get => m_objectDeletionEvents; }
    public AURORA.Protobuf.ServerData.ObjectTypes ObjectType { get => m_objectType; set => m_objectType = value; }
    public AURORA_SpatialAnchor AuroraSpatialAnchor { get => m_auroraSpatialAnchor; set => m_auroraSpatialAnchor = value; }
    public CoordinateSystems PrimaryCoordinateSystem { get => m_primaryCoordinateSystem; set => m_primaryCoordinateSystem = value; }
    public bool TerrainAdapterChanged { get => m_terrainAdapterChanged; set => m_terrainAdapterChanged = value; }

    //public void UpdateObjectOwnership(AURORA.ClientAPI.AuroraObject auroraObject)
    //{
    //    if (m_auroraObject == null || m_auroraManager == null ||
    //        m_auroraManager.AuroraSession == null || m_auroraManager.AuroraSession.UserSession == null)
    //        m_objectIsLocallyOwned = false;
    //    else
    //    {
    //        m_objectIsLocallyOwned = m_auroraObject.OwnerSessionUid.Uuid == m_auroraManager.AuroraSession.UserSession.SessionUid.Uuid;

    //        if(m_objectIsLocallyOwned)
    //        {
    //            m_auroraManager.AuroraLogger.Debug("Ownership of object " + UUID.ToString() + " aquired");
    //        }
    //        else
    //        {
    //            if (m_auroraObject.OwnerSessionUid != null && m_auroraObject.OwnerSessionUid.Uuid.Length == 16)
    //            {
    //                m_auroraManager.AuroraLogger.Debug("Ownership of object " + UUID.ToString() + " taken by " + AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(m_auroraObject.OwnerSessionUid));
    //            }
    //            else
    //            {
    //                m_auroraManager.AuroraLogger.Debug("Ownership of object " + UUID.ToString() + " taken by no one");
    //            }
    //        }
    //    }        
    //}

    public AURORA.ClientAPI.RpcRequest RequestObjectOwnership()
    {
        if(!IsLocallyOwned && m_auroraObject != null)
            m_takeObjectOwnershipRequest = m_auroraObject.RequestObjectOwnership();

        return m_takeObjectOwnershipRequest;
    }

    public void OnAuroraOwnershipAcquired()
    {
        m_forceTransformUpdate = true;
    }

    public void RequestOwnership()
    {
        m_auroraObject.RequestObjectOwnership();
    }

    public void PublishUpdates()
    {
        m_auroraObject.PublishUpdates();
    }

    public void SetGhostRepresentation(AURORA_GameObject ghost)
    {
        if (ghost != null)
        {
            if (m_ghostRepresentation != ghost)
            {
                m_ghostRepresentation = ghost;
                m_ghostInitialized = false;
            }
        }
        else
        {
            m_ghostRepresentation = null;
            m_auroraObject.GhostRepresentation = null;
        }
    }
}
