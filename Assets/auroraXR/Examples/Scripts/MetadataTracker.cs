using System;
using System.Collections;
using System.Collections.Generic;
using AURORA.Protobuf;
using UnityEngine;

[System.Serializable]
public class MetadataCollectionEvent : UnityEngine.Events.UnityEvent<AURORA.Protobuf.MetadataCollection>
{ }

[System.Serializable] 
public class MetadataEvent : UnityEngine.Events.UnityEvent<AURORA.Protobuf.MetadataWrapper>
{ }

[System.Serializable]
public class MetadataFieldEvent
{
    [SerializeField]
    private string m_metadataField;

    [SerializeField]
    private MetadataEvent metadataEvent;

    public string MetadataField { get => m_metadataField; set => m_metadataField = value; }
    public MetadataEvent MetadataEvent { get => metadataEvent; }
}

public class MetadataTracker : MonoBehaviour
{
    [SerializeField]
    private AURORA_GameObject m_auroraGameObject;

    //[SerializeField]
    //private MetadataCollectionEvent m_anyMetadataChanged;

    [SerializeField]
    private List<MetadataFieldEvent> m_specificMetadataFieldChanged;

    protected Dictionary<string, MetadataEvent> m_fieldEventLookupTable;

    public AURORA_GameObject AuroraGameObject { get => m_auroraGameObject; set => m_auroraGameObject = value; }
    //public MetadataCollectionEvent AnyMetadataChanged { get => m_anyMetadataChanged; }
    public List<MetadataFieldEvent> SpecificMetadataFieldChanged { get => m_specificMetadataFieldChanged; }

    private void Reset()
    {
        m_auroraGameObject = GetComponent<AURORA_GameObject>();
    }

    private void Awake()
    {
        Initialize();
    }

    protected void Initialize()
    {
        m_fieldEventLookupTable = new Dictionary<string, MetadataEvent>();

        if (m_auroraGameObject == null)
        {
            if(m_auroraGameObject == null)
            {
                m_auroraGameObject = GetComponent<AURORA_GameObject>();
            }

            if (m_auroraGameObject == null)
            {
                Debug.LogError("AURORA GameObject not set");
            }
        }
        else
        {
            m_auroraGameObject.AuroraObjectPropertyChangeEvents.MetadataChanged.EventHandlers.AddListener(OnMetadataKeyChanged);
        }
        foreach(var eventHandler in m_specificMetadataFieldChanged)
        {
            m_fieldEventLookupTable[eventHandler.MetadataField] = eventHandler.MetadataEvent;
        }
        
    }

    public virtual void OnMetadataKeyChanged(AURORA_GameObject auroraGameObject, AURORA.ClientAPI.MetadataCollection mc, string key, AURORA.Protobuf.MetadataWrapper metadataWrapper)
    {
        // Fire off any handler.
        // m_anyMetadataChanged.Invoke(metaDataCollectionChanges);

        if (m_fieldEventLookupTable.ContainsKey(key))
        {
            m_fieldEventLookupTable[key].Invoke(metadataWrapper);
        }
    }

    //protected virtual void OnMetadataChanged(AURORA_GameObject auroraGameObject, MetadataCollection metaDataCollectionChanges)
    //{
    //    // Fire off any handler.
    //    m_anyMetadataChanged.Invoke(metaDataCollectionChanges);

    //    foreach(var metadataField in metaDataCollectionChanges.Items)
    //    {
    //        if(m_fieldEventLookupTable.ContainsKey(metadataField.Key))
    //        {
    //            m_fieldEventLookupTable[metadataField.Key].Invoke(metadataField.Value);
    //        }
    //    }
    //}
}
