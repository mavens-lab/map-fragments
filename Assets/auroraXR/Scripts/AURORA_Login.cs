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
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class AuroraConnectionEvents
{
    protected bool m_atLeastOneActive;

    public OptionalRpcRequestGameObjectEventQueuePanel ObjectSpawnSuccessful;
    public OptionalRpcRequestGameObjectEventQueuePanel ObjectSpawnFailed;
    public OptionalRpcRequestGameObjectEventQueuePanel ObjectSpawnTimedOut;

    public void Initialize(AURORA_GameObject ago)
    {
        ObjectSpawnSuccessful.Initialize(ago);
        ago.AURORA_Object.ObjectSpawnSuccessfulHandler += ObjectSpawnSuccessful.InEvent;

        ObjectSpawnFailed.Initialize(ago);
        ago.AURORA_Object.ObjectSpawnFailedHandler += ObjectSpawnFailed.InEvent;

        ObjectSpawnTimedOut.Initialize(ago);
        ago.AURORA_Object.ObjectSpawnTimedOutHandler += ObjectSpawnTimedOut.InEvent;

        m_atLeastOneActive = (
            ObjectSpawnSuccessful.Enabled
            ||
            ObjectSpawnFailed.Enabled
            ||
            ObjectSpawnTimedOut.Enabled
        );
    }

    public void Invoke()
    {
        if (m_atLeastOneActive)
        {
            ObjectSpawnSuccessful.Invoke();
            ObjectSpawnFailed.Invoke();
            ObjectSpawnTimedOut.Invoke();
        }
    }
}

[System.Serializable]
public class StringEvent : UnityEngine.Events.UnityEvent<string>
{
}

public class AURORA_Login : MonoBehaviour
{
    public enum AuthenticatorStatus
    {
        NOT_AUTHENTICATED,
        AUTHENTICATION_REQUEST_SENT,
        AUTHENTICATION_SUCCESSFUL,
        AUTHENTICATION_FAILED,
        AUTHENTICATION_TIMED_OUT
    }

    [SerializeField] AURORA_Manager m_auroraManager;
	[SerializeField] bool m_autoAuthenticate;

    [SerializeField] InputField m_usernameInputField;

    //[SerializeField] TextElipseAnimation m_textElipseAnimation;

    //[SerializeField] GameObject m_objectToActivateOnceAuthenticated;

    //[SerializeField] UnityEngine.Events.UnityEvent m_authenticationSuccessfulEvents;
    //[SerializeField] UnityEngine.Events.UnityEvent m_authenticationFailedEvents;

    //   protected AURORA.ClientAPI.AuroraEventQueue<AURORA.ClientAPI.AuroraInterface> m_connectionEstablishedEventQueue;
    //   protected AURORA.ClientAPI.AuroraEventQueue<AURORA.ClientAPI.AuroraInterface> m_authenticationSuccessfulEventQueue;
    //   protected AURORA.ClientAPI.AuroraEventQueue<AURORA.ClientAPI.AuroraInterface> m_authenticationFailedEventQueue;

    //[SerializeField] TMPro.TMP_Text m_authenticationResponseMessageText;
    [SerializeField] StringEvent m_authenticationMessageHandler;

    private AURORA.ClientAPI.RpcRequest m_authenticationRequest;

    private AuthenticatorStatus m_authenticatorStatus;

	bool m_sceneLoadTriggered;

    public AuthenticatorStatus AuthenticatorStatus1 { get => m_authenticatorStatus; set => m_authenticatorStatus = value; }

    //SessionPanelRefs m_sessionPanelRefs;

    private void Start()
    {
        m_authenticatorStatus = AuthenticatorStatus.NOT_AUTHENTICATED;
        
		if(m_auroraManager == null)
		{
			m_auroraManager = GetComponent<AURORA_Manager>();
		}

        m_auroraManager.ConnectionEvents.ConnectionEstablished.EventHandlers.AddListener(OnAuroraConnectionEstablished);
        m_auroraManager.ConnectionEvents.ConnectionFailed.EventHandlers.AddListener(OnAuroraConnectionFailed);

        m_auroraManager.AuthenticationEvents.AuthenticationSuccessful.EventHandlers.AddListener(OnAuthenticationSuccessful);
        m_auroraManager.AuthenticationEvents.AuthenticationFailed.EventHandlers.AddListener(OnAuthenticationFailed);
        m_auroraManager.AuthenticationEvents.AuthenticationTimedOut.EventHandlers.AddListener(OnAuthenticationTimedOut);

        m_sceneLoadTriggered = false;
    }

    private void Reset()
    {
        var am = GetComponent<AURORA_Manager>();
        if (am != null)
        {
            m_auroraManager = am;
        }
    }

	// This is a test authenticator that logs into the test server.
	public void Authenticate()
	{
        if(m_authenticatorStatus == AuthenticatorStatus.AUTHENTICATION_SUCCESSFUL)
        {
            return;
        }
        else if(m_authenticatorStatus == AuthenticatorStatus.AUTHENTICATION_REQUEST_SENT)
        {
            Debug.LogWarning("Received authentication request while existing request is outstanding.");
        }
        else
        {
            string u;
            if (m_usernameInputField.text.Trim().Length == 0)
            {
                u = "Default-User";
            }
            else
            {
                u = m_usernameInputField.text;
            }
            string p = "abc123";

            Authenticate(u, p);

            m_authenticatorStatus = AuthenticatorStatus.AUTHENTICATION_REQUEST_SENT;
        }
    }

	public void Authenticate(string username, string password)
    {
        if (m_authenticatorStatus == AuthenticatorStatus.AUTHENTICATION_SUCCESSFUL)
        {
            Debug.LogWarning("Already authenticated");
            return;
        }
        if (m_auroraManager != null)
        {
            if (m_authenticatorStatus == AuthenticatorStatus.AUTHENTICATION_REQUEST_SENT)
            {
                Debug.LogWarning("Received authentication request while existing request is outstanding.");
            }
            else
            {
                m_auroraManager.Initialize();

                if (!m_auroraManager.IsConnected)
                    m_auroraManager.Connect();

                if (!m_auroraManager.IsConnected)
                {
                    try
                    {
                        m_authenticationMessageHandler.Invoke("Could not connect to AURORA Server.");
                    }
                    catch(System.Exception ex)
                    {
                        m_auroraManager.AuroraLogger.LogException(ex);
                    }
                    return;
                }

                //Use our random node name as the component name in logger.
                m_auroraManager.AuroraLogger.DefaultComponent = "AURORA XR - " + username;

                m_authenticationRequest = m_auroraManager.Authenticate(username, password);
                m_authenticatorStatus = AuthenticatorStatus.AUTHENTICATION_REQUEST_SENT;
            }
        }
        else
        {
            Debug.LogError("AURORA Manager not specified on AURORA_Login component!");
        }
    }

    //public void EvaluateAuthResponseMessage(AURORA_Manager auroraManager, AURORA.ClientAPI.AuroraInterface auroraInterface, AURORA.ClientAPI.RpcRequest rpcRequest)
    //{
    //    if(rpcRequest.RpcResponseMessage.ResponseCase == AURORA.Protobuf.ServerRPC.RpcResponse.ResponseOneofCase.Authentication)
    //    {
    //        m_authenticationMessageHandler.Invoke(rpcRequest.RpcResponseMessage.Authentication.AuthenticationResponseMessage);
    //    }
    //}

    protected void OnAuthenticationSuccessful(AURORA_Manager auroraManager, AURORA.ClientAPI.AuroraInterface auroraInterface, AURORA.ClientAPI.RpcRequest rpcRequest)
    {
        if (rpcRequest.RpcResponseMessage.ResponseCase == AURORA.Protobuf.ServerRPC.RpcResponse.ResponseOneofCase.Authentication)
        {
            try
            {
                m_authenticationMessageHandler.Invoke(rpcRequest.RpcResponseMessage.Authentication.AuthenticationResponseMessage);
            }
            catch (System.Exception ex)
            {
                m_auroraManager.AuroraLogger.AuroraLogger.LogException(ex);
            }
            m_authenticatorStatus = AuthenticatorStatus.AUTHENTICATION_SUCCESSFUL;
        }
    }

    protected void OnAuthenticationFailed(AURORA_Manager auroraManager, AURORA.ClientAPI.AuroraInterface auroraInterface, AURORA.ClientAPI.RpcRequest rpcRequest)
    {
        if (rpcRequest.RpcResponseMessage.ResponseCase == AURORA.Protobuf.ServerRPC.RpcResponse.ResponseOneofCase.Authentication)
        {
            try
            {
                m_authenticationMessageHandler.Invoke(rpcRequest.RpcResponseMessage.Authentication.AuthenticationResponseMessage);
            }
            catch (System.Exception ex)
            {
                m_auroraManager.AuroraLogger.AuroraLogger.LogException(ex);
            }
            m_authenticatorStatus = AuthenticatorStatus.AUTHENTICATION_FAILED;
        }
    }

    protected void OnAuthenticationTimedOut(AURORA_Manager auroraManager, AURORA.ClientAPI.AuroraInterface auroraInterface, AURORA.ClientAPI.RpcRequest rpcRequest)
    {
        if (rpcRequest.RpcResponseMessage.ResponseCase == AURORA.Protobuf.ServerRPC.RpcResponse.ResponseOneofCase.Authentication)
        {
            try
            {
                m_authenticationMessageHandler.Invoke(rpcRequest.RpcResponseMessage.Authentication.AuthenticationResponseMessage);
            }
            catch (System.Exception ex)
            {
                m_auroraManager.AuroraLogger.AuroraLogger.LogException(ex);
            }
            m_authenticatorStatus = AuthenticatorStatus.AUTHENTICATION_TIMED_OUT;
        }
    }

    protected void OnAuroraConnectionEstablished(AURORA_Manager auroraManager)
    {
        if(m_authenticatorStatus == AuthenticatorStatus.NOT_AUTHENTICATED)
        {
            if(m_autoAuthenticate)
            {
                Authenticate();
            }
        }
    }

    protected void OnAuroraConnectionFailed(AURORA_Manager auroraManager)
    {

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void Update()
    {
        //if (m_authenticationRequest != null && m_authenticationRequest.RpcRequestStatus != AURORA.ClientAPI.RpcRequest.RpcRequestStatuses.SENT_WAITING_RESPONSE)
        //{
        //    bool authFailed = false;

        //    if (m_authenticationRequest.RpcRequestStatus == AURORA.ClientAPI.RpcRequest.RpcRequestStatuses.RESPONSE_RECEIVED)
        //    {
        //        if (m_auroraManager.Authenticated)
        //        {
        //            Debug.Log("Authentication successful");

        //            m_authenticationMessageHandler.Invoke(m_auroraManager.AuroraSession.AuthenticationResponseMessage);

        //            //m_authenticationSuccessfulEvents.Invoke();
        //        }
        //        else
        //        {
        //            authFailed = true;
        //        }
        //    }
        //    else
        //    {
        //        authFailed = true;
        //    }

        //    if(authFailed)
        //    {
        //        Debug.LogError("Authentication Failed");

        //        m_authenticationMessageHandler.Invoke(m_authenticationRequest.RpcResponseMessage.Authentication.AuthenticationResponseMessage);

        //        //m_authenticationFailedEvents.Invoke();
        //    }

        //    // Clear the authenication request to stop this loop from happening.
        //    m_authenticationRequest = null;
        //}
    }

    //    private string getPath()
    //    {
    //#if UNITY_EDITOR
    //        return Application.dataPath + "/SavedUserInfo/" + "user_info.txt";
    //#else
    //                return Application.dataPath +"/"+"user_info.txt";
    //#endif
    //    }
}