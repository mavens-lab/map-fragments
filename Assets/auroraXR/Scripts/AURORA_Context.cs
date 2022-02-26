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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AURORA.ClientAPI;
using UnityEngine;


[System.Serializable]
public class SpawnQueueNotificationModeSettings
{
    //[Range(0.1f, 3.0f)]
    //[SerializeField]
    //private float m_spawnCheckInterval = .25f;

    //public float SpawnCheckInterval { get => m_spawnCheckInterval; set => m_spawnCheckInterval = value; }

    [Range(1, 50)]
    [SerializeField]
    private int m_maxSpawnsPerFrame = 5;

    public int MaxSpawnsPerFrame { get => m_maxSpawnsPerFrame; set => m_maxSpawnsPerFrame = value; }
}

//[System.Serializable]
//public class DirectSpawnNotificationEvents
//{
//    private bool m_active;

//    public RequiredFundamentalAuroraObjectEventQueuePanel SpawnObjectEvent;

//    public bool Active { get => m_active; set => m_active = value; }

//    DirectSpawnNotificationEvents() : base()
//    {
//        m_active = true;
//    }

//    public void Initialize()
//    {
//        if (m_active)
//        {
//            SpawnObjectEvent.Initialize()
//        }
//    }

//    public void Invoke()
//    {
//        if(m_active)
//        {
//            SpawnObjectEvent.Invoke();
//        }
//    }
//}


    public class AURORA_Context : MonoBehaviour
{
    [SerializeField]
    private AURORA_Manager m_auroraManager;

    [SerializeField]
    private AURORA_Proxy m_auroraProxy;

    [SerializeField]
    private string m_auroraContextUuid;

	[Tooltip("When true, new objects published by server will be spawned into scene.")]
	[SerializeField]
	protected bool m_spawningEnabled = true;

	[SerializeField]
    private Transform m_defaultSpawnParent;

    [SerializeField]
    private AURORA.ClientAPI.AuroraContext.SpawnNotificationModes m_spawnNotificationMode;

    public SpawnQueueNotificationModeSettings m_spawnQueueNotificationModeSettings;

    //public DirectSpawnNotificationEvents m_directSpawnNotificationEvents;

    //private float m_nextSpawnCheckTime = 0f;

    private AURORA.ClientAPI.AuroraEventQueue<AURORA.ClientAPI.AuroraContext> m_spawnEventQueue;

    [SerializeField]
	List<AURORA_Spawnable> m_spawnPrefabs;

	[Tooltip("Assign this tag to all objects spawned in this context.  Leave blank to leave untagged.")]
	[SerializeField]
	string m_tagForSpawnedObjects;

	private Dictionary<System.Guid, AURORA_Spawnable> m_spawnableDictionary;

	private Dictionary<System.Guid, AURORA_GameObject> m_unityAuroraGameObjects;

    private System.Threading.Mutex m_rpcSpawnRequestMutex;
    private Dictionary<System.Guid, AURORA_GameObject> m_rpcSpawnRequests;

    private System.Guid m_auroraContextGuid;

    private AURORA.ClientAPI.AuroraContext m_auroraContext;

    public AURORA_Logger AuroraLogger
    {
        get
        {
            return m_auroraManager.AuroraLogger;
        }
    }

    public AURORA.ClientAPI.AuroraContext AuroraContext
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

    public AURORA.ClientAPI.AuroraObjectManager AuroraObjectManager
    {
        get
        {
            if (m_auroraContext == null)
                return null;

            return m_auroraContext.AuroraObjectManager;
        }
    }

    public System.Guid ContextGuid
    {
        get
        {
            return AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(m_auroraContext.ContextUid);
        }
        protected set
        {
            m_auroraContext.ContextUid.MergeFrom(AURORA.ClientAPI.Tools.GuidToUniqueIdentifier(value));
        }
    }

    public AURORA.Protobuf.UniqueIdentifier ContextUid
    {
        get
        {
            return m_auroraContext.ContextUid;
        }
    }

    public string ContextUUID
    {
        get
        {
            return m_auroraContextUuid;
        }
        set
        {
            m_auroraContextUuid = value;
        }
    }

    public Transform DefaultSpawnParent
    {
        get
        {
            return m_defaultSpawnParent;
        }
    }

	public bool CanSpawnType(System.Guid objectTypeGuid)
	{
		return m_spawnableDictionary.ContainsKey(objectTypeGuid);
	}

	public bool CanSpawnType(AURORA.Protobuf.UniqueIdentifier objectTypeUid)
	{
		return CanSpawnType(AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(objectTypeUid));
	}

	public AURORA_Spawnable GetSpawnable(System.Guid objectTypeGuid)
	{
		return m_spawnableDictionary[objectTypeGuid];
	}

	public AURORA_Spawnable GetSpawnable(AURORA.Protobuf.UniqueIdentifier objectTypeUid)
	{
		return GetSpawnable(AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(objectTypeUid));
	}

	private void Reset()
    {
        var go = GameObject.Find("AURORA_Proxy");

        if (go != null)
            m_auroraProxy = go.GetComponent<AURORA_Proxy>();

        m_defaultSpawnParent = this.transform;

		m_spawningEnabled = true;
        m_spawnQueueNotificationModeSettings.MaxSpawnsPerFrame = 5;

		m_tagForSpawnedObjects = "";

        m_auroraContextUuid = System.Guid.NewGuid().ToString();

        m_spawnNotificationMode = AURORA.ClientAPI.AuroraContext.SpawnNotificationModes.USE_SPAWN_QUEUE;
    }

    private void Awake()
    {
        m_rpcSpawnRequestMutex = new System.Threading.Mutex();

        lock(m_rpcSpawnRequestMutex)
        {
            m_rpcSpawnRequests = new Dictionary<Guid, AURORA_GameObject>();
        }

		m_unityAuroraGameObjects = new Dictionary<System.Guid, AURORA_GameObject>();

		if (m_auroraProxy == null && m_auroraManager == null)
        {
            var go = GameObject.Find("AURORA_Proxy");
            if (go != null)
                m_auroraProxy = go.GetComponent<AURORA_Proxy>();
            else
                Debug.LogError("Could not find AURORA Proxy!");
        }

		if (m_auroraManager == null)
        {
            if (m_auroraProxy != null)
            {
                m_auroraManager = m_auroraProxy.AURORA_Manager;
            }
        }

		System.Guid contextGuid = System.Guid.Empty;

		if (m_auroraContextUuid.Length > 0)
		{
			contextGuid = new System.Guid(m_auroraContextUuid);
		}

		if (contextGuid == System.Guid.Empty)
		{
			throw new System.ApplicationException("Context Uuid is not set.");
		}

		m_auroraContext = new AURORA.ClientAPI.AuroraContext(m_auroraManager.AURORA_Interface, contextGuid, m_spawnNotificationMode);

        if (m_auroraContextUuid != null && m_auroraContextUuid.Length > 0)
        {
            ContextGuid = new System.Guid(m_auroraContextUuid);
        }

		//m_auroraManager.AURORA_Interface.AuroraContexts.TryAddContext(m_auroraContext);

		m_auroraManager.RegisterContext(this);

		if (m_spawnPrefabs == null)
		{
			m_spawnPrefabs = new List<AURORA_Spawnable>();
		}

        

		//m_registeredAuroraGameObjects = new Dictionary<int, AURORA_GameObject>();

		m_spawnableDictionary = new Dictionary<System.Guid, AURORA_Spawnable>();

		foreach (var prefab in m_spawnPrefabs)
		{
			if (prefab != null)
			{
				m_spawnableDictionary.Add(prefab.SpawnableTypeUuid, prefab);
			}
		}
	}

    private void OnDestroy()
    {
        if (m_auroraManager != null)
            m_auroraManager.UnregisterContext(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_spawnNotificationMode == AURORA.ClientAPI.AuroraContext.SpawnNotificationModes.DIRECT_SPAWN_NOTIFICATIONS)
        {
            //m_directSpawnNotificationEvents.Active = true;

            //m_directSpawnNotificationEvents.Initialize(this);
        }
        else
        {
            //m_directSpawnNotificationEvents.Active = false;

            m_spawnEventQueue = new AURORA.ClientAPI.AuroraEventQueue<AURORA.ClientAPI.AuroraContext>();
            m_spawnEventQueue.EventHandler += SpawnObjectFromQueue;

            m_auroraContext.NewQueuedObjectReadyToSpawnHandler += m_spawnEventQueue.EnqueueEvent;
        }

        m_auroraContext.Start();
        System.Threading.Thread.Sleep(300);
		m_auroraContext.SubscribeToRealTimeUpdates();
        //m_auroraManager.AURORA_Interface.TrackRealTimeUpdates(this.m_auroraContext);


        //m_nextSpawnCheckTime = Time.time + m_spawnQueueNotificationModeSettings.SpawnCheckInterval;
    }

    protected void SpawnObjectFromQueue(AURORA.ClientAPI.AuroraContext auroraContext)
    {
        if(m_auroraContext.ContextUid.Uuid != auroraContext.ContextUid.Uuid)
        {
            Debug.LogError("Context " + AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(m_auroraContext.ContextUid)
                + " received spawn notification for context " + AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(auroraContext.ContextUid));
            return;
        }

        AURORA.ClientAPI.AuroraObject auroraObjectToSpawn = m_auroraContext.AuroraObjectManager.GetNewObjectToSpawn();

        SpawnObject(auroraObjectToSpawn);
    }

    protected void SpawnObject(AURORA.ClientAPI.AuroraObject auroraObjectToSpawn)
    {
        if (auroraObjectToSpawn != null)
        {
            System.Guid spawnableUuid = AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(auroraObjectToSpawn.SpawnableUid);

            //TODO: Add support for spawning under different parents
            // For now assume context
            Transform spawnParent = DefaultSpawnParent;

            if (spawnParent == null)
            {
                spawnParent = DefaultSpawnParent;
            }

            if (m_spawnableDictionary.ContainsKey(spawnableUuid))
            {
                var spawnedSpawnable = Instantiate(m_spawnableDictionary[spawnableUuid], spawnParent);
                
                RpcRequest spawnRequest = auroraObjectToSpawn.SpawnObjectRpcRequest;

                AURORA_GameObject ago = spawnedSpawnable.GetComponent<AURORA_GameObject>();

                if (ago != null)
                {                  

                    if (spawnRequest != null && spawnRequest.RpcRequestMessage != null &&
                        spawnRequest.RpcRequestMessage.RequestCase == AURORA.Protobuf.ServerRPC.RpcRequest.RequestOneofCase.SpawnObject)
                    {
                        lock(m_rpcSpawnRequestMutex)
                        {
                            m_rpcSpawnRequests.Add(spawnRequest.RequestGuid, ago);
                        }

                        auroraObjectToSpawn.ObjectSpawnSuccessfulHandler += ObjectSpawnSuccessful;
                        auroraObjectToSpawn.ObjectSpawnFailedHandler += ObjectSpawnFailed;
                        auroraObjectToSpawn.ObjectSpawnTimedOutHandler += ObjectSpawnTimedOut;
                    }
                    else
                    {
                        m_auroraManager.AuroraLogger.Warning("After attempt to spawn Spawn Object RPC Request not available");
                    }

                    ago.Initialize(auroraObjectToSpawn, this);

                    ago.AURORA_Manager = m_auroraManager;

                    if (m_tagForSpawnedObjects != "")
                    {
                        ago.gameObject.tag = m_tagForSpawnedObjects;
                    }

                    //RegisterAuroraGameObject(ago);

                    m_auroraManager.AuroraLogger.Info(
                        string.Format(
                            "Spawned new auroraXR Object of type {0}",
                            spawnableUuid
                            )
                        );

                }
                else
                {
                    Debug.LogError("Spawned object does not have an AURORA_GameObject Component");
                }
            }
            else
            {
                this.m_auroraManager.AuroraLogger.Warning("Got request to spawn object of type " + spawnableUuid.ToString() +
                    " which is not a registered AURORA Spawnable");
            }

        }
    }

    private void ObjectSpawnTimedOut(AuroraObject auroraObject, RpcRequest rpcRequest)
    {
        m_auroraManager.AuroraLogger.Warning("Spawn request " + rpcRequest.RequestGuid.ToString() + " timed out.");
        lock (m_rpcSpawnRequestMutex)
        {
            if (m_rpcSpawnRequests.ContainsKey(rpcRequest.RequestGuid))
            {
                m_rpcSpawnRequests.Remove(rpcRequest.RequestGuid);
            }
        }
    }

    private void ObjectSpawnFailed(AuroraObject auroraObject, RpcRequest rpcRequest)
    {
        m_auroraManager.AuroraLogger.Warning("Spawn request " + rpcRequest.RequestGuid.ToString() + " failed.  Message: " + rpcRequest.RpcResponseMessage);
        lock (m_rpcSpawnRequestMutex)
        {
            if (m_rpcSpawnRequests.ContainsKey(rpcRequest.RequestGuid))
            {
                m_rpcSpawnRequests.Remove(rpcRequest.RequestGuid);
            }
        }
    }

    private void ObjectSpawnSuccessful(AuroraObject auroraObject, RpcRequest rpcRequest)
    {
        lock(m_rpcSpawnRequestMutex)
        {
            if(!m_rpcSpawnRequests.ContainsKey(rpcRequest.RequestGuid))
            {
                m_auroraManager.AuroraLogger.Warning("Spawn successful, but unable to find record of spawn rpc request " + rpcRequest.RequestGuid);
            }
            else
            {
                m_auroraManager.AuroraLogger.Warning("Spawn successful for spawn rpc request " + rpcRequest.RequestGuid);
                AURORA_GameObject ago = m_rpcSpawnRequests[rpcRequest.RequestGuid];
                
                m_rpcSpawnRequests.Remove(rpcRequest.RequestGuid);

                RegisterAuroraGameObject(ago);
                //m_unityAuroraGameObjects.Add(ago.UUID, ago);
            }
        }
    }



    // Update is called once per frame
    void Update()
	{
        if (m_spawningEnabled)
        {
            if (m_spawnNotificationMode == AURORA.ClientAPI.AuroraContext.SpawnNotificationModes.USE_SPAWN_QUEUE)
            {
                try
                {
                    m_spawnEventQueue.Invoke(m_spawnQueueNotificationModeSettings.MaxSpawnsPerFrame);
                }
                catch(System.Exception ex)
                {
                    m_auroraContext.AuroraLogger.LogException(ex);
                }
            }
            else if(m_spawnNotificationMode == AURORA.ClientAPI.AuroraContext.SpawnNotificationModes.DIRECT_SPAWN_NOTIFICATIONS)
            {
                Debug.LogError("DIRECT_SPAWN_NOTIFICATIONS Not Currently Supported");   
                //m_directSpawnNotificationEvents.Invoke();
            }
            else
            {
                Debug.LogError("Unknown spawn notificaiton mode: " + m_spawnNotificationMode.ToString());
            }
        }
	}

	//public AURORA_GameObject PrepareSceneObject(AURORA_GameObject ago)
	//{
 //       ago.AURORA_Object = m_auroraContext.RegisterSceneObject(ago.UID, ago.GetAuroraTransform());

 //       return ago;
	//}


	public void RegisterAuroraGameObject(AURORA_GameObject ago)
    {
        if (ago == null || ago.UUID == System.Guid.Empty)
        {
            m_auroraContext.AuroraLogger.Error("Unable to register ago named " + ago.gameObject.name + " for lack of object UID");
        }
        else
        {
            m_unityAuroraGameObjects.Add(ago.UUID, ago);
        }
    }

    public AURORA_GameObject FindAURORAGameobjectByUID(AURORA.Protobuf.UniqueIdentifier objectUID)
    {
		//return m_auroraContext.AuroraObjectManager.GetObject(AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(objectUID));
		return FindAURORAGameobjectByUID(AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(objectUID));
        //return new AURORA_GameObject();
    }

	public AURORA_GameObject FindAURORAGameobjectByUID(System.Guid objectUuid)
	{
		//return m_auroraContext.AuroraObjectManager.GetObject(AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(objectUID));
		if (m_unityAuroraGameObjects.ContainsKey(objectUuid))
			return m_unityAuroraGameObjects[objectUuid];

		return null;

		//throw new System.NotImplementedException();
		//return new AURORA_GameObject();
	}

	//public AURORA.ClientAPI.RpcRequest RequestObjectOwnership(AURORA_GameObject ago)
 //   {
 //       //Always return true when offline
 //       if (m_auroraManager.OfflineMode)
 //           return null;

 //       ago.AURORA_Object.RequestObjectOwnership();

 //       return m_takeObjectOwnershipRequest;
 //   }

    public void UnregisterAuroraGameObject(AURORA_GameObject ago)
    {
		if (ago != null)
		{
			if (m_unityAuroraGameObjects.ContainsKey(ago.UUID))
			{
				m_unityAuroraGameObjects.Remove(ago.UUID);
			}
			//m_auroraManager.UnregisterAuroraGameObject(ago);
		}
    }

    public bool SpawnRemoteOnly(AURORA_GameObject ago)
    {
        //if (CanSpawnType(ago.AuroraSpawnable.SpawnableTypeUuid))
        //{
        AURORA.Protobuf.UniqueIdentifier spawnUid = new AURORA.Protobuf.UniqueIdentifier();
        spawnUid.Uuid = AURORA.ClientAPI.Tools.GuidToByteString(ago.AuroraSpawnable.SpawnableTypeUuid);

        try
        {
            AuroraObject ao = m_auroraContext.SpawnObject(
                spawnUid, AURORA_Tools.MakeAuroraTransform(ago.transform)
                );

            RpcRequest spawnRequest = ao.SpawnObjectRpcRequest;

            if (spawnRequest != null && spawnRequest.RpcRequestMessage != null &&
                    spawnRequest.RpcRequestMessage.RequestCase == AURORA.Protobuf.ServerRPC.RpcRequest.RequestOneofCase.SpawnObject)
            {
                lock (m_rpcSpawnRequestMutex)
                {
                    m_rpcSpawnRequests.Add(spawnRequest.RequestGuid, ago);
                }

                ao.ObjectSpawnSuccessfulHandler += ObjectSpawnSuccessful;
                ao.ObjectSpawnFailedHandler += ObjectSpawnFailed;
                ao.ObjectSpawnTimedOutHandler += ObjectSpawnTimedOut;
            }
            else
            {
                m_auroraManager.AuroraLogger.Warning("After attempt to Spawn Object RPC Request not available");
            }

            ago.Initialize(
                ao,
                this
            );

            if (ago.AURORA_Object == null)
            {
                m_auroraContext.Logger.Warning("Error spawning object of type " +
                    ago.AuroraSpawnable.SpawnableTypeUuid.ToString(), "AURORA_Context");
                return false;
            }
            else
            {
                ago.AURORA_Manager = m_auroraManager;
                //ago.TransformTrackingMode = AURORA_GameObject.TransformTrackingModes.USE_LOCAL;

                return true;
            }
        }
        catch (System.ApplicationException aex)
        {
            m_auroraContext.Logger.Warning("Could not spawn object: " + aex.Message, "AURORA_Context");
        }
        //}

        return false;
    }

    public AURORA_GameObject Spawn(System.Guid spawnableGuid, Vector3 localPosition)
    {
        return DoSpawn(spawnableGuid, localPosition, Quaternion.identity, Vector3.one, true);
    }

    public AURORA_GameObject Spawn(System.Guid spawnableGuid, Vector3 localPosition, Quaternion localRotation)
    {
        return DoSpawn(spawnableGuid, localPosition, localRotation, Vector3.one, true, true);
    }

    public AURORA_GameObject Spawn(System.Guid spawnableGuid, Vector3 localPosition, Quaternion localRotation, Vector3 localScale,
        bool applyPosition = false, bool applyRotation = true, bool appyScale = true)
    {
        return DoSpawn(spawnableGuid, localPosition, localRotation, localScale, applyPosition, applyRotation, appyScale);
    }

    protected AURORA_GameObject DoSpawn(System.Guid spawnableGuid, Vector3 localPosition, Quaternion localRotation, Vector3 localScale,
        bool applyPosition = false, bool applyRotation = false, bool appyScale = false)
    {
        if (CanSpawnType(spawnableGuid))
        {
            AURORA_Spawnable newObject = Instantiate(GetSpawnable(spawnableGuid), m_defaultSpawnParent);

            if (applyPosition)
                newObject.transform.localPosition = localPosition;

            if (applyRotation)
                newObject.transform.localRotation = localRotation;

            if (appyScale)
                newObject.transform.localScale = localScale;

            AURORA_GameObject ago = newObject.GetComponent<AURORA_GameObject>();

            try
            {
                AURORA.ClientAPI.AuroraObject serverSpawnedObject = m_auroraContext.SpawnObject(
                    AURORA.ClientAPI.Tools.GuidToUniqueIdentifier(spawnableGuid), AURORA_Tools.MakeAuroraTransform(newObject.transform));

                if (serverSpawnedObject == null)
                {
                    m_auroraContext.Logger.Warning("Error spawning object of type " + spawnableGuid.ToString(), "AURORA_Context");
                    Destroy(newObject);
                }
                else
                {
                    ago.Initialize(serverSpawnedObject, this);
                    ago.AURORA_Manager = m_auroraManager;
                    //RegisterAuroraGameObject(ago);
                    m_auroraContext.Logger.Info(
                        string.Format(
                            "Spawned object {0} of type {1}",
                            ago.UUID,
                            spawnableGuid.ToString()
                            ), 
                        "AURORA_Context"
                    );
                }

                return ago;
            }
            catch (System.ApplicationException aex)
            {
                m_auroraContext.Logger.Warning("Could not spawn object: " + aex.Message, "AURORA_Context");
                Destroy(newObject);
            }
        }

        m_auroraContext.Logger.Warning("Attempt to spawn unknown spawnable type " + spawnableGuid.ToString(), "AURORA_Context");
        return null;
    }

	internal void AuroraShutdown()
	{
		while(m_unityAuroraGameObjects.Count > 0)
		{
			var elem = m_unityAuroraGameObjects.ElementAt(0);

			if (elem.Value != null)
			{
				elem.Value.AuroraShutdown();
			}

			// Make sure it was removed
			if (m_unityAuroraGameObjects.ContainsKey(elem.Key))
			{
				m_unityAuroraGameObjects.Remove(elem.Key);
			}
		}
        m_auroraContext.Stop(true);
    }

    public void RemoveObjectLocally(AURORA_GameObject ago)
    {

        UnregisterAuroraGameObject(ago);

        m_auroraContext.RemoveObjectLocally(ago.AURORA_Object);
            
        Destroy(ago.gameObject);
    }

    //public void RemoveObjectLocally(AURORA.Protobuf.UniqueIdentifier uid)
    //{
    //    RemoveObjectLocally(Tools.UniqueIdentifierToGuid(uid));
    //}

    //public void RemoveObjectLocally(AuroraObject auroraObject)
    //{
    //    RemoveObjectLocally(auroraObject.ObjectUid);
    //}

    //public void RemoveObjectLocally(AURORA_GameObject ago)
    //{
    //    RemoveObjectLocally(ago.AURORA_Object.ObjectUid);
    //}
}
