using System;
using System.Collections;
using System.Collections.Generic;
using AURORA.ClientAPI;
using AURORA.Protobuf;
using UnityEngine;

[System.Serializable]
public class MetadataChanged : UnityEngine.Events.UnityEvent<string, Metadata>
{ }

[System.Serializable]
public class MetadataDeleted : UnityEngine.Events.UnityEvent<string>
{ }

public class MetadataInterface : MonoBehaviour
{
    public enum MetadataStates
    {
        Unknown,
        HasUnpublishedLocalChanges,
        SynchronizedWithServer,
        DeletedFromServer
    }

    [SerializeField]
    AURORA_GameObject m_auroraGameObject;

    [SerializeField]
    string m_metadataKey;

    [Tooltip("Default content type to use if not set")]
    [SerializeField]
    string m_defaultContentType;

    [SerializeField]
    bool m_autoPublishChanges;

    [Range(1, 50)]
    [SerializeField]
    private int m_maxMetadataChangeEventsPerFrame = 5;

    [SerializeField]
    public MetadataChanged MetadataChanged;

    [Range(1, 50)]
    [SerializeField]
    private int m_maxMetadataDeletedEventsPerFrame = 5;

    [SerializeField]
    public MetadataDeleted MetadataDeleted;

    protected AURORA.ClientAPI.AuroraEventQueue<AURORA.ClientAPI.MetadataCollection, string, MetadataWrapper> m_metadataChangedEventQueue;

    protected AURORA.ClientAPI.AuroraEventQueue<AURORA.ClientAPI.MetadataCollection, string> m_metadataKeyDeletedEventQueue;

    protected AURORA.ClientAPI.AuroraEventQueue<AURORA.ClientAPI.AuroraObject> m_objectChangesPublishedEventQueue;

    private MetadataStates m_metadataState = MetadataStates.Unknown;

    protected AURORA.ClientAPI.MetadataCollection m_metadataCollection;

    public bool AutoPublishUpdates { get => m_autoPublishChanges; set => m_autoPublishChanges = value; }

    public string DefaultContentType { get => m_defaultContentType; set => m_defaultContentType = value; }

    public string MetadataKey { get => m_metadataKey;  }

    public MetadataStates MetadataState { get => m_metadataState; }

    private void Awake()
    {
        m_metadataChangedEventQueue = new AuroraEventQueue<AURORA.ClientAPI.MetadataCollection, string, MetadataWrapper>();
        m_metadataKeyDeletedEventQueue = new AuroraEventQueue<AURORA.ClientAPI.MetadataCollection, string>();
        m_objectChangesPublishedEventQueue = new AuroraEventQueue<AuroraObject>();

        m_metadataChangedEventQueue.EventHandler += OnMetadataModifiedEvent;
        m_metadataKeyDeletedEventQueue.EventHandler += OnMetadataDeletedEvent;
        m_objectChangesPublishedEventQueue.EventHandler += OnMetadataChangesPublishedEvent;
    }

    public AURORA_GameObject AuroraGameObject
    {
        get => m_auroraGameObject;
        set
        {
            m_auroraGameObject = value;
            m_metadataCollection = m_auroraGameObject.AURORA_Object.MetadataCollection;
            SetupEventHandlers();
        }
    }

    public AURORA.Protobuf.Metadata Metadata
    {
        get
        {
            AURORA.Protobuf.MetadataWrapper mdw = null;

            if (m_metadataCollection.HasKey(m_metadataKey))
            {
                mdw = m_metadataCollection.GetItem(m_metadataKey);
            }
            else
            {
                mdw = new AURORA.Protobuf.MetadataWrapper();
            }

            if (mdw.Metadata == null)
            {
                mdw.Metadata = new AURORA.Protobuf.Metadata();
                mdw.Metadata.ContentType = m_defaultContentType;
            }

            m_metadataState = MetadataStates.HasUnpublishedLocalChanges;

            return mdw.Metadata;
        }
        set
        {
            if (m_auroraGameObject.IsLocallyOwned)
            {
                AURORA.Protobuf.MetadataWrapper mdw = null;

                if (m_metadataCollection.HasKey(m_metadataKey))
                {
                    mdw = m_metadataCollection.GetItem(m_metadataKey);
                }
                else
                {
                    mdw = new AURORA.Protobuf.MetadataWrapper();
                }

                if (mdw.Metadata == null)
                {
                    mdw.Metadata = new AURORA.Protobuf.Metadata();

                    mdw.Metadata.ContentType = m_defaultContentType;
                    mdw.Metadata.Data = Google.Protobuf.ByteString.CopyFromUtf8(m_defaultContentType);
                }

                mdw.Metadata.MergeFrom(value);

                m_metadataCollection.SetItem(m_metadataKey, mdw);

                if (m_autoPublishChanges)
                {
                    m_auroraGameObject.AURORA_Object.PublishUpdates();
                }
            }
            else
            {
                Debug.LogWarning("Cannot set metadata. AURORA GameObject is not owned locally");
            }
        }
    }

    public string ContentType
    {
        get
        {
            AURORA.Protobuf.Metadata md = Metadata;
            try
            {
                return Metadata.ContentType;
            }
            catch
            {
                return m_defaultContentType;
            }
        }
        set
        {
            AURORA.Protobuf.Metadata md = new AURORA.Protobuf.Metadata();
            md.ContentType = value;

            Metadata = md;
        }
    }

    public Google.Protobuf.ByteString Data
    {
        get
        {
            AURORA.Protobuf.Metadata md = Metadata;
            try
            {
                return Metadata.Data;
            }
            catch
            {
                return null;
            }
        }

        set
        {
            AURORA.Protobuf.Metadata md = new AURORA.Protobuf.Metadata();
            md.Data = value;

            Metadata = md;
        }
    }

    public void SetData(Google.Protobuf.ByteString bs, string contentType)
    {
        AURORA.Protobuf.Metadata md = new Metadata();
        md.ContentType = contentType;
        md.Data = bs;

        Metadata = md;
    }

    public void PublishUpdates()
    {
        if (m_auroraGameObject.IsLocallyOwned)
        {
            if (m_autoPublishChanges)
            {
                m_auroraGameObject.AURORA_Object.PublishUpdates();
            }
        }
        else
        {
            Debug.LogError("Could not publish updates. Object not locally owned.");
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_metadataState = MetadataStates.Unknown;

        if (m_auroraGameObject == null)
        {
            Debug.LogError("AURORA Game Object Not set in Metadata Interface for Game Object " + gameObject.name);
        }
        else
        {
            if (m_metadataCollection == null)
            {
                m_metadataCollection = m_auroraGameObject.AURORA_Object.MetadataCollection;
            }

            SetupEventHandlers();

            //Fire off events to initialize any components that need it
            if (m_metadataCollection.HasKey(m_metadataKey))
            {
                OnMetadataModifiedEvent(m_metadataCollection, m_metadataKey, m_metadataCollection.GetItem(m_metadataKey));
            }
        }
    }

    protected virtual void SetupEventHandlers()
    {
        if(m_metadataCollection.MetadataKeyModifiedEventHandler != null)
        {
            // Make sure we don't add it twice by removing the assignment if it exists
            m_metadataCollection.MetadataKeyModifiedEventHandler -= m_metadataChangedEventQueue.EnqueueEvent;
        }
        m_metadataCollection.MetadataKeyModifiedEventHandler += m_metadataChangedEventQueue.EnqueueEvent;

        if(m_metadataCollection.MetadataKeyDeletedEventHandler != null)
        {
            m_metadataCollection.MetadataKeyDeletedEventHandler -= m_metadataKeyDeletedEventQueue.EnqueueEvent;
        }
        m_metadataCollection.MetadataKeyDeletedEventHandler += m_metadataKeyDeletedEventQueue.EnqueueEvent;

        if(m_auroraGameObject.AURORA_Object.ObjectChangesPublishedHandler != null)
        {
            m_auroraGameObject.AURORA_Object.ObjectChangesPublishedHandler -= m_objectChangesPublishedEventQueue.EnqueueEvent;
        }
        m_auroraGameObject.AURORA_Object.ObjectChangesPublishedHandler += m_objectChangesPublishedEventQueue.EnqueueEvent;
    }
    
    private void OnMetadataModifiedEvent(AURORA.ClientAPI.MetadataCollection metadataCollection, string key, MetadataWrapper metadataWrapper)
    {
        if(key == m_metadataKey)
        {
            if(m_metadataCollection.HaveChangesToPublish)
            {
                m_metadataState = MetadataStates.HasUnpublishedLocalChanges;
            }
            else
            {
                m_metadataState = MetadataStates.SynchronizedWithServer;
            }
            MetadataChanged.Invoke(m_metadataKey, metadataWrapper.Metadata);
        }
    }

    private void OnMetadataDeletedEvent(AURORA.ClientAPI.MetadataCollection metadataCollection, string key)
    {
        if (key == m_metadataKey)
        {
            m_metadataState = MetadataStates.DeletedFromServer;
            MetadataDeleted.Invoke(m_metadataKey);
        }
    }

    private void OnMetadataChangesPublishedEvent(AURORA.ClientAPI.AuroraObject auroraObject)
    {
        m_metadataState = MetadataStates.SynchronizedWithServer;
    }

    protected virtual void Reset()
    {
        m_auroraGameObject = GetComponent<AURORA_GameObject>();
    }

    virtual protected void Update()
    {
        m_metadataChangedEventQueue.Invoke(m_maxMetadataChangeEventsPerFrame);
        m_metadataKeyDeletedEventQueue.Invoke(m_maxMetadataDeletedEventsPerFrame);
        m_objectChangesPublishedEventQueue.Invoke(10);
    }
}
