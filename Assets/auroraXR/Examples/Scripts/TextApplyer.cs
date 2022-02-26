using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextApplyer : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_InputField m_inputField;

    [SerializeField]
    MetadataFieldStringInterface m_metadataFieldStringInterface;

    public void Apply()
    {
        m_metadataFieldStringInterface.Text = m_inputField.text;
    }
}
