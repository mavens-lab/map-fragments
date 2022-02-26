using System;
using System.Collections;
using System.Collections.Generic;
using AURORA.ClientAPI;
using UnityEngine;

public class ConnectionStatusDisplayManager : MonoBehaviour
{
    [SerializeField]
    AURORA_Manager m_auroraManager;

    [SerializeField] TMPro.TextMeshProUGUI m_text;

    [SerializeField]
    Color m_connectedColor;

    [SerializeField]
    Color m_disconnectedColor;

    bool m_previouslyConnected = false;

    private void Reset()
    {
        m_auroraManager = GetComponent<AURORA_Manager>();
        m_text = GetComponent<TMPro.TextMeshProUGUI>();
        m_connectedColor = Color.green;
        m_disconnectedColor = Color.red;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_text.color = m_disconnectedColor;
        m_text.text = "Waiting...";
        m_previouslyConnected = false;

        m_auroraManager.ConnectionEvents.ConnectionStatusChanged.EventHandlers.AddListener(OnConnectionStatusChanged);
    }

    protected void OnConnectionStatusChanged(AURORA_Manager am, AURORA.ClientAPI.AuroraInterface ai, bool isConnected)
    {
        if (m_text != null)
        {
            if (isConnected)
            {
                m_previouslyConnected = true;

                m_text.color = m_connectedColor;

                m_text.text = "Connected";
            }
            else
            {
                m_text.color = m_disconnectedColor;

                if (m_previouslyConnected)
                {
                    m_text.text = "Reconnecting...";
                }
                else
                {
                    m_text.text = "Connecting...";
                }
            }
        }
    }
}
