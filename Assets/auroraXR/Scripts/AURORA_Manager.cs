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
using System.Threading;
using System.IO;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class AURORA_ManagerEvent : UnityEngine.Events.UnityEvent<AURORA_Manager> { }

[System.Serializable]
public class AURORA_ManagerRpcEvent : UnityEngine.Events.UnityEvent<AURORA_Manager, AURORA.ClientAPI.AuroraInterface, AURORA.ClientAPI.RpcRequest> { }

[System.Serializable]
public class AURORA_InterfaceEvent : UnityEngine.Events.UnityEvent<AURORA_Manager, AURORA.ClientAPI.AuroraInterface> { }
//public class AuroraInterfaceEventQueue : AURORA.ClientAPI.AuroraEventQueue<AURORA.ClientAPI.AuroraInterface> { }
//public class FlaggedChnagedEventQueue : AURORA.ClientAPI.AuroraEventQueue<AURORA.ClientAPI.AuroraInterface, bool> { }
[System.Serializable]
public class AURORA_InterfaceFlagChangeEvent : UnityEngine.Events.UnityEvent<AURORA_Manager, AURORA.ClientAPI.AuroraInterface, bool> { }

[System.Serializable]
public class OptionalAuroraManagerEvent
    : OptionalSubstitutionEventQueue1i1oPanelBase<AURORA_ManagerEvent, AURORA.ClientAPI.AuroraInterface, AURORA_Manager>
{ }

[System.Serializable]
public class AuroraManagerEvent
    : SubstitutionEventQueue1i1oPanelBase<AURORA_ManagerEvent, AURORA.ClientAPI.AuroraInterface, AURORA_Manager>
{ }

[System.Serializable]
public class OptionalAuroraManagerRpcEvent
    : OptionalAugmentedEventQueue2i3oPanelBase<AURORA_ManagerRpcEvent, AURORA.ClientAPI.AuroraInterface, AURORA.ClientAPI.RpcRequest, AURORA_Manager>
{ }

[System.Serializable]
public class AuroraManagerRpcEvent
    : AugmentedEventQueue2i3oPanelBase<AURORA_ManagerRpcEvent, AURORA.ClientAPI.AuroraInterface, AURORA.ClientAPI.RpcRequest, AURORA_Manager>
{ }


[System.Serializable]
public class OptionalAuroraInterfaceEvent
    : OptionalAugmentedEventQueue1i2oPanelBase<AURORA_InterfaceEvent, AURORA.ClientAPI.AuroraInterface, AURORA_Manager>
{ }

[System.Serializable]
public class AuroraInterfaceFlagChangedEvent
    : AugmentedEventQueue2i3oPanelBase<AURORA_InterfaceFlagChangeEvent, AURORA.ClientAPI.AuroraInterface, bool, AURORA_Manager>
{ }

public enum PlatformModes
{
    PLATFORM_MODE_UNITY_EDITOR_32,
    PLATFORM_MODE_UNITY_EDITOR_64,
    PLATFORM_MODE_PLAYER,
    PLATOFRM_MODE_UNITY_ANDROID
}



[System.Serializable]
public class ConnectionEvents
{
    private bool m_atLeastOneActive = false;

    [Tooltip("Indicates that the server connection process has been started. Does not mean a connection is established, " +
        "only that everything is running as required to attempt a connection")]
    public AuroraManagerEvent ServerConnectionStarted;

    [Tooltip("Indicates that the server connection process has been stopped")]
    public AuroraManagerEvent ServerConnectionStopped;
    
    public AuroraManagerEvent ConnectionEstablished;

    public AuroraManagerEvent ConnectionFailed;
    
    public AuroraInterfaceFlagChangedEvent ConnectionStatusChanged;

    public ConnectionEvents()
    {
        m_atLeastOneActive = false;
    }

    public void Initialize(AURORA_Manager auroraManager)
    {
        ServerConnectionStarted.Initialize(auroraManager);
        auroraManager.AURORA_Interface.ServerConnectionStartedHandler += ServerConnectionStarted.InEvent;

        ServerConnectionStopped.Initialize(auroraManager);
        auroraManager.AURORA_Interface.ServerConnectionStoppedHandler += ServerConnectionStopped.InEvent;

        ConnectionEstablished.Initialize(auroraManager);
        auroraManager.AURORA_Interface.ConnectionEstablishedHandler += ConnectionEstablished.InEvent;

        ConnectionFailed.Initialize(auroraManager);
        auroraManager.AURORA_Interface.ConnectionFailedHandler += ConnectionFailed.InEvent;

        ConnectionStatusChanged.Initialize(auroraManager);
        auroraManager.AURORA_Interface.ConnectionStatusChangedHandler += ConnectionStatusChanged.InEvent;

        m_atLeastOneActive = (
            true
            //ServerConnectionStarted.Enabled
            //||
            //ServerConnectionStopped.Enabled
            //||
            //ConnectionEstablished.Enabled
            //||
            //ConnectionFailed.Enabled
            //||
            //ConnectionStatusChanged.Enabled
        );
    }

    public void Invoke()
    {
        if (m_atLeastOneActive)
        {
            ServerConnectionStarted.Invoke();
            ServerConnectionStopped.Invoke();
            ConnectionEstablished.Invoke();
            ConnectionFailed.Invoke();
            ConnectionStatusChanged.Invoke();
        }
    }
}

[System.Serializable]
class AuthenticationEvents
{
    private bool m_atLeastOneActive = false;

    public AuroraManagerRpcEvent AuthenticationSuccessful;

    public AuroraManagerRpcEvent AuthenticationFailed;

    public AuroraManagerRpcEvent AuthenticationTimedOut;

    public AuthenticationEvents()
    {
        m_atLeastOneActive = false;
    }

    public void Initialize(AURORA_Manager auroraManager)
    {
        AuthenticationSuccessful.Initialize(auroraManager);
        auroraManager.AURORA_Interface.AuthenticationSuccessfulHandler += AuthenticationSuccessful.InEvent;

        AuthenticationFailed.Initialize(auroraManager);
        auroraManager.AURORA_Interface.AuthenticationFailedHandler += AuthenticationFailed.InEvent;

        AuthenticationTimedOut.Initialize(auroraManager);
        auroraManager.AURORA_Interface.AuthenticationTimedOutHandler += AuthenticationTimedOut.InEvent;

        m_atLeastOneActive = true;
    }

    public void Invoke()
    {
        if (m_atLeastOneActive)
        {
            AuthenticationSuccessful.Invoke();
            AuthenticationFailed.Invoke();
            AuthenticationTimedOut.Invoke();
        }
    }
}

#if UNITY_EDITOR_32 || UNITY_EDITOR_64
[UnityEditor.InitializeOnLoad]
#endif
[RequireComponent(typeof(PluginPathInitializer))]
public class AURORA_Manager : MonoBehaviour
{
    private static AURORA_Manager s_auroraInstance;

    private static List<AURORA.ClientAPI.Subscriber> s_auroraAppSubscribers;

	Mutex m_mutex;

    //[SerializeField]
    //PluginPathSettings m_pluginPathSettings;

    //AURORA.ClientAPI.ServerConnection m_serverConnection;
    AURORA.ClientAPI.AuroraInterface m_aurora;

	[Tooltip("If checked, will be initialized on start.  Otherwise, you must call Initialize() on your own.")]
	[SerializeField]
	bool m_initializeOnStart = true;

    [SerializeField]
    bool m_connectOnStart;

    bool m_isInitialized;

    [SerializeField]
    AURORA_Logger m_auroraLogger;

    [SerializeField]
    string m_serverAddress;

    [SerializeField] TMP_Dropdown m_serverSelectionDropdown;

    [Tooltip("Available substitutions: %HOME% => user's home folder. %APPDATA% => Unity's Application.dataPath. %PERSISTENTDATA% => Application.persistentDataPath")]
    [SerializeField]
	string m_curveCertificatePath;

	[SerializeField]
	string m_curveCryptoClientCertName;

	[SerializeField]
	string m_curveCryptoServerCertName;

    [SerializeField]
    bool m_installServerCertificate = false;

    [Tooltip("If Install Server Certificate is specified and a server certificate does not exist, a new certificate file will be created with this public key specified here.")]
    [SerializeField]
    string m_serverCertificatePublicKey = "";

	[SerializeField]
    bool m_offlineMode;

    [SerializeField]
    bool m_enableConnectionChecks = true;

    [SerializeField]
    [Range(30, 90)]
    int m_connectionCheckIntervalSeconds = 5;

    float m_nextConnectionCheckTime = 0f;

    [SerializeField]
    [Tooltip("If this value is greater than 'Refresh Session When Minutes Remain Under' then session might expire between checks.")]
    [Range(1, 10)]
    int m_minutesBetweenSessionStatusChecks;

    [SerializeField]
    [Tooltip("Refresh the session when the number of minutes remaining falls under this value.")]
    [Range(2, 10)]
    int m_refreshSessionWhenMinutesRemainUnder;

    //[SerializeField]
    //Transform m_object;

    //[SerializeField]
    //int m_numThreads = 16;

    //[Tooltip("Controls the amount of time the Transform Update Publisher thread sleeps between transmitting local changes to the server.")]
    //[Range(10,5000)]
    //[SerializeField]
    //int m_transformUpdatePublishSleepMilliSeconds = 50;

    //[Range(0.1f, 3.0f)]
    //[SerializeField]
    //private float m_spawnCheckInterval = .25f;

    //private float m_nextSpawnCheckTime = 0f;

    //[SerializeField]
    //List<AURORA_Spawnable> m_spawnPrefabs;

    [SerializeField]
    List<AURORA_Context> m_contexts;

    [SerializeField]
    ConnectionEvents m_connectionEvents;

    [SerializeField]
    AuthenticationEvents m_authenticationEvents;

    //private Dictionary<System.Guid, AURORA_Spawnable> m_spawnableDictionary;

    //public bool m_done = false;

    //bool m_authenticated = false;

    //[Tooltip("When true, new objects published by server will be spawned into scene.")]
    //[SerializeField]
    //protected bool m_spawningEnabled = true;

    float m_restartTime = 0f;

    //Dictionary<int, AURORA_GameObject> m_registeredAuroraGameObjects;

    bool m_resetRequested = false;
    bool m_deferAvg = true;

    float m_testTime = 0f;

    public static void RegisterAuroraAppSubscriber(AURORA.ClientAPI.Subscriber sub)
    {
        if (s_auroraAppSubscribers == null)
        {
            s_auroraAppSubscribers = new List<AURORA.ClientAPI.Subscriber>();
        }

        s_auroraAppSubscribers.Add(sub);
    }

	public bool IsInitialized
	{
		get
		{
			return m_isInitialized;
		}
	}

	public AURORA.ClientAPI.AuroraInterface AURORA_Interface
    {
        get
        {
            return m_aurora;
        }
    }

    public bool OfflineMode
    {
        get
        {
            return m_offlineMode;
        }
        set
        {
            m_offlineMode = value;
        }
    }

    /// <summary>
    /// This is to enable hooking to UnityEvents.
    /// </summary>
    /// <param name="value"></param>
    public void SetWorkOffline(bool value)
    {
        OfflineMode = value;
    }

    public bool ServerConnectionActive
    {
        get
        {
            return m_aurora.IsConnected;
        }
    }

    //public bool SpawningEnabled
    //{
    //    get
    //    {
    //        return m_spawningEnabled;
    //    }
    //    set
    //    {
    //        m_spawningEnabled = value;
    //    }
    //}

	public string ServerAddress
	{
		get
		{
			return m_serverAddress;
		}

		set
		{
			m_serverAddress = value;
		}
	}

	public string CurveCryptoServerCertName
	{
		get
		{
			return m_curveCryptoServerCertName;
		}
		set
		{
			m_curveCryptoServerCertName = value;
		}
	}
    
    //public bool CanSpawnType(System.Guid objectTypeGuid)
    //{
    //    return m_spawnableDictionary.ContainsKey(objectTypeGuid);
    //}

    //public bool CanSpawnType(AURORA.Protobuf.UniqueIdentifier objectTypeUid)
    //{
    //    return CanSpawnType(AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(objectTypeUid));
    //}

    //public AURORA_Spawnable GetSpawnable(System.Guid objectTypeGuid)
    //{
    //    return m_spawnableDictionary[objectTypeGuid];
    //}

    //public AURORA_Spawnable GetSpawnable(AURORA.Protobuf.UniqueIdentifier objectTypeUid)
    //{
    //    return GetSpawnable(AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(objectTypeUid));
    //}

    public AURORA_Logger AuroraLogger
    {
        get
        {
            return m_auroraLogger;
        }
    }

    public bool RegisterContext(AURORA_Context context)
    {
        if(!m_contexts.Contains(context))
        {
            m_contexts.Add(context);
            return true;
        }
        return false;
    }

    public bool UnregisterContext(AURORA_Context context)
    {
        if (m_contexts.Contains(context))
        {
            m_contexts.Remove(context);
            return true;
        }
        return false;
    }


    //public bool RegisterAuroraGameObject(AURORA_GameObject ago)
    //{
    //    if (!m_registeredAuroraGameObjects.ContainsKey(ago.GetHashCode()))
    //    {
    //        m_registeredAuroraGameObjects.Add(ago.GetHashCode(), ago);
    //        return true;
    //    }
    //    return false;
    //}

    //public void UnregisterAuroraGameObject(AURORA_GameObject ago)
    //{
    //    m_registeredAuroraGameObjects.Remove(ago.GetHashCode());
    //}

    public AURORA.ClientAPI.AuroraSession AuroraSession
    {
        get
        {
            try
            {
                if (m_aurora == null)
                {
                    return null;
                }
                else
                    return m_aurora.AuroraSession;
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError("Error: " + ex.ToString());
            }

            return null;
        }
    }

    public bool Authenticated
    {
        get
        {
            if (m_aurora == null)
                return false;

            if (m_aurora.AuroraSession == null)
                return false;

            return m_aurora.AuroraSession.AuthenticationSuccessful;
        }
    }

    //public int TransformUpdatePublishSleepMilliSeconds
    //{
    //    get
    //    {
    //        m_transformUpdatePublishSleepMilliSeconds = m_aurora.TransformUpdatePublishSleepMilliSeconds;
    //        return m_transformUpdatePublishSleepMilliSeconds;
    //    }
    //    set
    //    {
    //        m_transformUpdatePublishSleepMilliSeconds = value;
    //        m_aurora.TransformUpdatePublishSleepMilliSeconds = value;
    //    }
    //}

    private void Reset()
    {
		m_initializeOnStart = true;
#if UNITY_ANDROID
        m_curveCertificatePath = "%APPDATA";
#else
        m_curveCertificatePath = "%HOME%/.curve";
#endif
        m_serverAddress = "connect.auroraxr.net";

        m_curveCryptoClientCertName = "AURORA-XR";

        m_curveCryptoServerCertName = "connect.auroraxr.net";

        m_offlineMode = false;

        m_minutesBetweenSessionStatusChecks = 1;
        m_refreshSessionWhenMinutesRemainUnder = 2;

        m_auroraLogger = GetComponent<AURORA_Logger>();
    }

	private void Awake()
	{
        DontDestroyOnLoad(this.gameObject);

        if (s_auroraInstance == null)
        {
            s_auroraInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }


        //if (s_auroraInstance != null)
        //{
        //    AURORA_Manager original = s_auroraInstance;
        //    s_auroraInstance = this;
        //    Destroy(original.gameObject);
        //}
        //else
        //{
        //    s_auroraInstance = this;       
        //}

        if (m_minutesBetweenSessionStatusChecks < 1)
        {
            m_minutesBetweenSessionStatusChecks = 1;
        }

        if(m_refreshSessionWhenMinutesRemainUnder < 2)
        {
            m_minutesBetweenSessionStatusChecks = 2;
        }

        if(m_connectionCheckIntervalSeconds < 30)
        {
            m_connectionCheckIntervalSeconds = 30;
        }
		//System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(System.Console.Out));
		m_mutex = new Mutex();

		//SetupPluginPath();

		m_isInitialized = false;
	}

	public virtual void Initialize()
	{
		lock (m_mutex)
		{
			//SetupPluginPath();

			//if (m_auroraLogger == null)
			//{
			//	m_auroraLogger = gameObject.AddComponent<AURORA_Logger>();
			//}

			if (m_isInitialized)
			{
				Debug.Log("AURORA Manger already initialized");
				return;
			}

			try
			{
				AURORA.ClientAPI.AuroraZmqContext.Initialize();
			}
			catch(System.Exception ex)
			{
				Debug.LogError(ex.ToString());
				throw ex;
			}

			//if (m_auroraLogger == null)
			//{
			//	m_auroraLogger = gameObject.AddComponent<AURORA_Logger>();
			//}

			//DontDestroyOnLoad(this.gameObject);

			ResetMetrics();

			m_resetRequested = true;
			m_deferAvg = true;

			m_isInitialized = true;
		}
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_initializeOnStart)
        {
            Initialize();

            if (m_connectOnStart)
            {
                Connect();
            }
        }

        m_nextConnectionCheckTime = 0f;
    }

    public void RequestResetMetrics()
    {
        m_resetRequested = true;
        m_auroraLogger.Debug("Metrics Reset Requested");
    }

    protected void ResetMetrics()
    {
        m_restartTime = Time.time;

        m_auroraLogger.Debug("Metrics Reset");
    }

	private void OnApplicationQuit()
	{
		ShutdownAurora();
	}

	private void OnDestroy()
	{
		ShutdownAurora();
	}

	private void ShutdownAurora()
	{
        // Shut down all subscribers
        if(s_auroraAppSubscribers != null)
        {
            foreach(var sub in s_auroraAppSubscribers)
            {
                sub.Close();
            }
        }

		// Tell all contexts to do shutdown activities.
		foreach(var context in m_contexts)
		{
			context.AuroraShutdown();
		}

		AURORA.ClientAPI.AuroraZmqContext.Shutdown();
		
     //   if (m_registeredAuroraGameObjects != null)
     //   {
     //       foreach (var ago in m_registeredAuroraGameObjects)
     //       {
     //           try
     //           {
     //               ago.Value.AuroraShutdown();
     //           }
     //           catch (System.Exception ex)
     //           {
					//System.Console.Error.WriteLine("Error unregistering AURORA Game Object \"" + ago.Value.name + "\": " + ex.ToString());
     //           }
     //       }
     //       m_registeredAuroraGameObjects.Clear();
     //   }

        try
        {
			if (m_aurora != null)
            {
                m_auroraLogger.Stop();
                m_aurora.Stop();
				m_aurora = null;
            }
			AURORA.ClientAPI.AuroraZmqContext.Shutdown();
		}
		catch (System.Exception ex)
        {
			System.Console.Error.WriteLine("Error while shutting down AURORA: " + ex.ToString());
        }
        System.Console.WriteLine("AURORA shutdown complete");

    }

    public bool IsConnected
	{
		get
		{
			try
			{
				if (m_aurora == null)
					return false;

				if (m_aurora.ServerConnection == null)
					return false;

				return m_aurora.ServerConnection.AuroraInterface.IsConnected;
			}
			catch(System.ApplicationException)
			{
				return false;
			}
		}
	}

    public ConnectionEvents ConnectionEvents { get => m_connectionEvents; set => m_connectionEvents = value; }
    internal AuthenticationEvents AuthenticationEvents { get => m_authenticationEvents; set => m_authenticationEvents = value; }

    public void Connect()
    {
        try
        {
			if (!m_isInitialized)
				Initialize();
				//throw new System.ApplicationException("Connect called before AURORA Manager initialized.");

            if (m_aurora != null)
            {
                m_auroraLogger.Debug("Stopping AURORA...");
                m_aurora.Stop();
                m_auroraLogger.Debug("AURORA Shutdown Complete.");
            }

            // Sets the server address to the currently selected dropdown value
            int v = m_serverSelectionDropdown.value;
            m_serverAddress = m_serverSelectionDropdown.options[v].text;
            m_curveCryptoServerCertName = m_serverAddress;
            // Ticket 151 - Handling for other platform without a home folder.
            if(m_curveCertificatePath.Contains("%APPDATA%"))
            {
                m_curveCertificatePath = m_curveCertificatePath.Replace(
                    "%APPDATA%", Application.persistentDataPath);
            }
            if(m_curveCertificatePath.Contains("%PERSISTENTDATA%"))
            {
                m_curveCertificatePath = m_curveCertificatePath.Replace(
                    "%PERSISTENTDATA%",
                    Application.persistentDataPath
                    );                
            }               
            Debug.Log("Cert location: " + m_curveCertificatePath);
            //if(m_installServerCertificate)
            //{
            //    AURORA.ClientAPI.AuroraZmqContext.GetOrCreateCert(m_curveCryptoServerCertName, m_curveCertificatePath,
            //        m_serverCertificatePublicKey, "");
            //}

            if (m_auroraLogger != null)
            {
                m_aurora = new AURORA.ClientAPI.AuroraInterface(m_serverAddress, m_curveCryptoClientCertName, m_curveCryptoServerCertName, m_curveCertificatePath, m_auroraLogger.AuroraLogger);
            }
            else
            {
                m_aurora = new AURORA.ClientAPI.AuroraInterface(m_serverAddress, m_curveCryptoClientCertName, m_curveCryptoServerCertName, m_curveCertificatePath);
            }

            m_connectionEvents.Initialize(this);
            m_authenticationEvents.Initialize(this);

            m_aurora.ServerConnection.SessionTimeRemaingCheckInterval =
                new System.TimeSpan(0, m_minutesBetweenSessionStatusChecks, 0);

            m_aurora.AutoRefreshSessionTimeRemaining = 
                (int) new System.TimeSpan(0, m_refreshSessionWhenMinutesRemainUnder, 0).TotalSeconds;


            m_aurora.Start();

            m_aurora.InitiateConnectionTest();

            // Setup logger
            if(m_auroraLogger != null)
                m_auroraLogger.InitializeLogger();

            // m_aurora.TrackRealTimeUpdates();
            //m_aurora.TransformUpdatePublishSleepMilliSeconds = m_transformUpdatePublishSleepMilliSeconds;
        }
        catch (System.Exception ex)
        {
            m_auroraLogger.Error("Error: " + ex.ToString());
        }
    }

	public AURORA.ClientAPI.RpcRequest Echo()
	{
		return m_aurora.Echo(new byte[] { 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 });
	}

	public AURORA.ClientAPI.RpcRequest Echo(byte[] payload)
	{
		return m_aurora.Echo(payload);
	}

    // Is the Authenticate method here different than m_aurora.Authenticate ??
    public AURORA.ClientAPI.RpcRequest Authenticate(string username, string password)
    {
        try
        {
            if (m_aurora != null)
            {
                AURORA.ClientAPI.RpcRequest authReq = m_aurora.Authenticate(username, password);
                return authReq;
            }
            throw new System.ApplicationException("AURORA object not initialized.");
        }
        catch (System.Exception ex)
        {
            m_auroraLogger.Error("Error: " + ex.ToString());
        }
        return null;
    }

    public void ResetAllUpdateTrackers()
    {
        foreach (var context in m_aurora.AuroraContexts.Values)
        {
            context.AuroraObjectManager.ResetAllUpdateTrackers();

			//context.ResetAllUpdateTrackers();

			//foreach (var ago in m_registeredAuroraGameObjects)
   //         {
   //             ago.Value.ResetUpdateTracker();
   //         }
        }
        m_auroraLogger.Debug("All Update Trackers on Registered Game Objects in all Contexts Reset");
    }

    private void Update()
    {
        try
        {
            m_connectionEvents.Invoke();
            m_authenticationEvents.Invoke();
        }
        catch (System.Exception ex)
        {
            m_auroraLogger.LogException(ex);
        }

        if (m_enableConnectionChecks)
        {
            if(Time.time >= m_nextConnectionCheckTime)
            {
                m_nextConnectionCheckTime = Time.time + (float)(m_connectionCheckIntervalSeconds);

                if(m_aurora != null)
                {
                    m_aurora.InitiateConnectionTest();
                }
            }
        }
    }
    // Update is called once per frame
    //void Update()
    //{
    //    if (m_spawningEnabled && Time.time >= m_nextSpawnCheckTime)
    //    {
    //        m_nextSpawnCheckTime = Time.time + m_spawnCheckInterval;

    //        int perFrameSpawnLimit = 50;

    //        foreach (var context in m_contexts)
    //        {
    //            if (perFrameSpawnLimit <= 0)
    //                break;

    //            if(context.AuroraObjectManager.HaveNewObjectsToSpawn) {
    //              Debug.Log("Have new objects to spawn");
    //            }

    //            while (perFrameSpawnLimit > 0 && context.AuroraObjectManager.HaveNewObjectsToSpawn)
    //            {
    //                AURORA.ClientAPI.AuroraObject auroraObjectToSpawn = context.AuroraObjectManager.GetNewObjectToSpawn();

    //                System.Guid spawnableUuid = AURORA.ClientAPI.Tools.UniqueIdentifierToGuid(auroraObjectToSpawn.SpawnableUid);

    //                if (m_spawnableDictionary.ContainsKey(spawnableUuid))
    //                {
    //                    //TODO: Add support for spawning under different parents
    //                    // For now assume context
    //                    Transform spawnParent = context.DefaultSpawnParent;

    //                    if(spawnParent == null)
    //                    {
    //                        spawnParent = context.transform;
    //                    }

    //                    var spawnedSpawnable = Instantiate(m_spawnableDictionary[spawnableUuid], spawnParent);

    //                    AURORA_GameObject ago = spawnedSpawnable.GetComponent<AURORA_GameObject>();

    //                    ago.AURORA_Object = auroraObjectToSpawn;

    //                    ago.AURORA_Manager = this;
    //                }
    //                else
    //                {
    //                    m_auroraLogger.Warning("Got request to spawn object of type " + spawnableUuid.ToString() +
    //                        " which is not a registered AURORA Spawnable");
    //                }
    //            }
    //        }
    //    }

    //    //if (Input.GetKeyDown(KeyCode.R))
    //    //{
    //    //    RequestResetMetrics();
    //    //    ResetAllUpdateTrackers();
    //    //}
    //}

}
