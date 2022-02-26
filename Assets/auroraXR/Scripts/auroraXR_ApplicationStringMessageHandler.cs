using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AuroraXrApplicationStringEvent : UnityEngine.Events.UnityEvent<string, string> { }

public class auroraXR_ApplicationStringMessageHandler : MonoBehaviour
{
    public AuroraXrApplicationStringEvent m_auroraXrApplicationStringEvent;

    public void HandleAppMessage(string topic, byte[] payload)
    {
        Google.Protobuf.ByteString p = Google.Protobuf.ByteString.CopyFrom(payload);

        try
        {
            m_auroraXrApplicationStringEvent.Invoke(topic, p.ToStringUtf8());
        }
        catch(System.Exception ex)
        {
            Debug.LogException(ex);
        }

        Debug.Log("[" + topic + "] " + p.ToStringUtf8());
    }
}
