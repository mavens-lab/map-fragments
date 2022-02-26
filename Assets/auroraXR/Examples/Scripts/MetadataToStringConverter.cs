using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MetadataToStringConverter : MonoBehaviour
{
    [SerializeField]
    private StringMetadataValueChanged m_onMetadataValueChanged;

    public UnityEvent<string> OnMetadataValueChanged { get => m_onMetadataValueChanged; }

    public void ProcessMetadataWrapper(AURORA.Protobuf.MetadataWrapper metadataWrapper)
    {
        try
        {
            if (metadataWrapper.ItemCase == AURORA.Protobuf.MetadataWrapper.ItemOneofCase.Metadata)
            {
                string value = metadataWrapper.Metadata.Data.ToStringUtf8();
                m_onMetadataValueChanged.Invoke(value);
            }
            else
            {
                Debug.LogError("Metadata type " + metadataWrapper.ItemCase + " not supported");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error getting metadata string: " + ex.ToString());
            m_onMetadataValueChanged.Invoke("{ERROR}");
        }
    }
}
