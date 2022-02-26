using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ApplicationBytesToUtf8 : MonoBehaviour
{
    [SerializeField]
    StringEvent m_onStringConverted;

    public void HandleAppMessage(string topic, byte[] payload)
    {
        m_onStringConverted.Invoke(Google.Protobuf.ByteString.CopyFrom(payload).ToStringUtf8());
    }
}
