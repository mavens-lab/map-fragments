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

public class AURORA_ApplicationInstance : MonoBehaviour
{
    [SerializeField]
    AURORA_Manager m_auroraManager;

    [Tooltip("Used to get the AURORA Manager after scene loads.")]
    [SerializeField]
    AURORA_Proxy m_auroraProxy;

    [SerializeField]
    AURORA_ApplicationRegistration m_auroraApplicationRegistration;

    [Tooltip("If true, then when the application starts it will attempt to get or create an instance of the Application from the Aurora Application Registration provided.")]
    [SerializeField]
    private bool m_getOrCreateApplicationInstanceOnStart = true;

	[Tooltip("Assignes a particular UUID for the application.  Leave blank to have a random UUID generated (not yet implemented).")]
	[SerializeField]
	private string m_applicationInstanceUuid;

    [SerializeField]
    private string m_applicationName;

    [Multiline(5)]
    [SerializeField]
    private string m_applicationDescription;

    [SerializeField]
    private List<string> m_applicationLabels;

    [SerializeField]
    AURORA.Protobuf.Crypto.ENCRYPTION_MODE m_encryptionMode;

    [SerializeField]
    string m_base64SessionKey;


    //[SerializeField]
    //private List<AppProperty> m_applicationProperties;

    private AURORA.ClientAPI.ApplicationInstance m_auroraApplicationInstance;

    public AURORA_Manager AuroraManager
    {
        get
        {
            return m_auroraManager;
        }
    }

    public AURORA.ClientAPI.ApplicationInstance AuroraApplicationInstance
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

    public AURORA.ClientAPI.ApplicationTypeRegistration AuroraApplicationRegistration
    {
        get
        {
            return m_auroraApplicationRegistration.AuroraApplicationRegistration;
        }
    }

    public System.Guid ApplicationInstanceUuid
    {
        get
        {
            return new System.Guid((m_applicationInstanceUuid));
        }
        set
        {
            if (value == null)
                m_applicationInstanceUuid = null;
            else
                m_applicationInstanceUuid = value.ToString();
        }
    }

    private void Reset()
    {
        if(m_auroraApplicationRegistration != null)
        {
            m_applicationName = m_auroraApplicationRegistration.AuroraApplicationRegistration.Name;
            m_applicationDescription = m_auroraApplicationRegistration.AuroraApplicationRegistration.Description;

            // TODO: Duplicate Labels
        }
        m_encryptionMode = AURORA.Protobuf.Crypto.ENCRYPTION_MODE.PlainText;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(m_auroraManager == null)
        {
            if(m_auroraProxy == null)
            {
                throw new System.ApplicationException("Neither AURORA Manager nor Proxy are specified on: " + gameObject.name);
            }

            m_auroraManager = m_auroraProxy.AURORA_Manager;

        }
        if(m_getOrCreateApplicationInstanceOnStart)
        {
			Initialize();
        }
		else
		{
			m_auroraApplicationInstance = null;
		}
    }

	public void Initialize()
	{
		if(m_auroraApplicationInstance != null)
		{
			Debug.LogWarning("Application already initialized.");
			return;
		}

		if(m_applicationInstanceUuid == "")
		{
			m_applicationInstanceUuid = System.Guid.NewGuid().ToString();
		}
		else
		{
			//Make sure they provided a valid uuid;
			m_applicationInstanceUuid = new System.Guid(m_applicationInstanceUuid).ToString();
		}

		m_auroraApplicationInstance = m_auroraApplicationRegistration.GetOrCreateApplicationInstance(new System.Guid(m_applicationInstanceUuid));

        m_auroraApplicationInstance.EncryptionMode = m_encryptionMode;
        if(m_encryptionMode != AURORA.Protobuf.Crypto.ENCRYPTION_MODE.PlainText)
        {
            m_auroraApplicationInstance.SetSessionKeyBase64(m_base64SessionKey);
        }
	}

    private void OnDestroy()
    {
        if(m_auroraApplicationInstance != null)
        {
            Debug.Log("Stopping AURORA Application Instance.");
            m_auroraApplicationInstance.Stop();
            Debug.Log("AURORA Application Instance Stopped.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AURORA.ClientAPI.Subscriber CreateConnectedSubscriber()
    {
        var sub = m_auroraApplicationInstance.CreatedConnectedSubscriber();

        AURORA_Manager.RegisterAuroraAppSubscriber(sub);

        return sub;
    }

    public void PublishAppMessage(byte[] topic, byte[] payload)
    {
		m_auroraApplicationInstance.PublishAppMessage(topic, payload);
    }

    public void PublishAppMessage<T>(string topic, T payload) where T : Google.Protobuf.IMessage, new()
    {
        m_auroraApplicationInstance.PublishAppMessage<T>(topic, 
            payload);
    }

	public void PublishAppMessage<T>(byte[] topic, T payload) where T : Google.Protobuf.IMessage, new()
	{
		m_auroraApplicationInstance.PublishAppMessage<T>(topic,
			payload);
	}


	//public void PublichAppMessage<T>(byte[] topic, T payload) where T : Google.Protobuf.IMessage, new()
	//{
	//    m_auroraApplicationInstance.PublishAppMessage<T>(topic,
	//        payload);
	//}
}
