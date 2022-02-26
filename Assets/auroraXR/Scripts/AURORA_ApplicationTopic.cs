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

using UnityEngine.Events;

[System.Serializable]
public class AURORA_ApplicationMessageEvent : UnityEvent<string, byte[]> { }

[System.Serializable]
public class AURORA_ApplicationMessageEventPanel
{
    [Tooltip("Enable event handlers")]
    public bool m_enabled;

    [Tooltip("Maximum number of messages to process per frame")]
    [Range(1, 50)]
    public int m_maxMessagePerFrame=5;

    public AURORA_ApplicationMessageEvent m_auroraApplicationMessageHandlers;
}

public class AURORA_ApplicationTopic : MonoBehaviour
{
    [SerializeField]
    AURORA_ApplicationInstance m_auroraApplicationInstance;

	//[Tooltip("If check, then will subscribe to all application topics.  Otherwise, you must specify at least one application topics to subscribe to below.")]
	//[SerializeField]
	//bool m_subscribeAllApplicationTopics;

    [SerializeField]
    string m_applicationTopic;

    [SerializeField]
    AURORA_ApplicationMessageEventPanel m_applicationMessageEvents;

    private AURORA.ClientAPI.Subscriber m_appSubscriber;

    bool m_shuttingDown = false;

    public AURORA_ApplicationInstance AuroraApplicationInstance
    {
        get
        {
            return m_auroraApplicationInstance;
        }
        set
        {
            m_auroraApplicationInstance = value;
        }
    }

    public AURORA_Manager AuroraManager
    {
        get
        {
            return m_auroraApplicationInstance.AuroraManager;
        }
    }

    private void OnDestroy()
    {
        m_shuttingDown = true;
        if(m_appSubscriber != null)
        {
            m_appSubscriber.Close();
            m_appSubscriber = null;
        }
    }


    private void Reset()
    {
        m_auroraApplicationInstance = transform.parent.GetComponent<AURORA_ApplicationInstance>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_applicationMessageEvents.m_enabled)
        {
            m_appSubscriber = CreateConnectedSubscriber();
        }
        m_shuttingDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_applicationMessageEvents.m_enabled)
        {
            if(m_appSubscriber == null && !m_shuttingDown)
            {
                m_appSubscriber = CreateConnectedSubscriber();
            }
            else
            {
                // Don't do anything else as we are shutting down
                return;
            }

            int msgCount = 0;

            while (
                msgCount < m_applicationMessageEvents.m_maxMessagePerFrame
                &&
                m_appSubscriber.TryGetAuroraMessage(out var auroraMessage, 0)
            )
            {
                if (auroraMessage.FrameCount == 2)
                {
                    m_applicationMessageEvents.m_auroraApplicationMessageHandlers.Invoke(auroraMessage.Pop().ConvertToString(), auroraMessage.Pop().ToByteArray());
                }
                else
                {
                    Debug.LogWarning("Got Application message with wrong number of frames (!= 2)");
                }

                msgCount += 1;
            }
        }
    }

    public AURORA.ClientAPI.Subscriber CreateConnectedSubscriber()
    {
		if (m_applicationTopic == "")
			throw new System.ApplicationException("Application topic is required.");

        if (m_shuttingDown)
            return null;

        AURORA.ClientAPI.Subscriber sub = m_auroraApplicationInstance.AuroraApplicationInstance.CreatedConnectedSubscriber();
        sub.Subscribe(m_applicationTopic);

        AURORA_Manager.RegisterAuroraAppSubscriber(sub);

        return sub;
    }

    public void PublishAppMessage(byte[] payload)
    {
        if (m_auroraApplicationInstance != null)
            m_auroraApplicationInstance.AuroraApplicationInstance.PublishAppMessage(
                Google.Protobuf.ByteString.CopyFromUtf8(m_applicationTopic),
                Google.Protobuf.ByteString.CopyFrom(payload));
    }

    public void PublishAppMessage<T>(T payload) where T : Google.Protobuf.IMessage, new()
    {
        if(m_auroraApplicationInstance != null)
            m_auroraApplicationInstance.PublishAppMessage<T>(m_applicationTopic, 
                payload);
    }
}
