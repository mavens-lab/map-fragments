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

public class AURORA_Proxy : MonoBehaviour
{
    [SerializeField]
    private AURORA_Manager m_auroraManager;

	[Tooltip("If checked, will be initialized on start.  Otherwise, you must call Initialize() on your own.")]
	[SerializeField]
	private bool m_initializeOnStart = true;

	[SerializeField]
    protected string m_AuroraManagerGameObjectName = "AURORA";

    [SerializeField]
    AURORA_Context m_primaryContext;

    [Tooltip("If checked and no auroraXR manager is found, load the login scene identified below.")]
    [SerializeField]
    bool m_loadLoginSceneIfNoAuroraManager = true;

    [SerializeField]
    int m_loginSceneIndex = 0;

    private bool m_isInitialized = false;

    public enum ModifyAuroraManagerSpawningModes
    {
        LEAVE_UNCHANGED,
        ENABLE_SPAWNING,
        DISABLE_SPAWNING
    }

    //[SerializeField]
    //protected ModifyAuroraManagerSpawningModes m_modifyAuroraManagerSpawningMode = ModifyAuroraManagerSpawningModes.LEAVE_UNCHANGED;

    private void Reset()
    {
        m_AuroraManagerGameObjectName = "auroraXR";
       // m_modifyAuroraManagerSpawningMode = ModifyAuroraManagerSpawningModes.LEAVE_UNCHANGED;
    }

	private void Awake()
	{
		m_isInitialized = false;

        if (m_auroraManager == null)
        {
            AttemptToFindAuroraXrManager();
        }

        if(m_auroraManager == null && m_loadLoginSceneIfNoAuroraManager)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(m_loginSceneIndex);
        }
	}

	private void Start()
	{
		if(m_initializeOnStart)
			Initialize();
	}

	public bool IsInitialized
	{
		get
		{
			return m_isInitialized;
		}
	}

    protected virtual AURORA_Manager AttemptToFindAuroraXrManager()
    {
        var go = GameObject.Find(m_AuroraManagerGameObjectName);
        if (m_auroraManager == null && go != null)
        {
            m_auroraManager = go.GetComponent<AURORA_Manager>();
            Debug.Log("Found auroraXR Manager by name: " + m_AuroraManagerGameObjectName);
        }

        if(m_auroraManager == null)
        {
            Debug.Log("Could not find auroraXR Manager with name provided.  Attempting to find any auroraXR Managers in the scene...");

            var auroraXRManagers = FindObjectsOfType<AURORA_Manager>();

            if (auroraXRManagers.Length == 1)
            {
                m_auroraManager = auroraXRManagers[0];

                Debug.Log("Found auroraXR Manager with name " + m_auroraManager.gameObject.name);
            }
            else if(auroraXRManagers.Length > 1)
            {
                Debug.Log("Found more than one auroraXR Manager in the scene.  Using first available.  Please specify an auroraXR GameObject name to control which one to use.");
                m_auroraManager = auroraXRManagers[0];
            }
            else
            {
                Debug.LogError("Unable to find any auroraXR Managers in the scene.");
            }
        }


        return m_auroraManager;
    }

	public virtual void Initialize()
    {
		if(m_isInitialized)
		{
			Debug.Log("AURORA Proxy already initialized.");
			return;
		}
		
		if (m_primaryContext == null)
        {
            Debug.LogWarning("Primary Context Not Set in AURORA Proxy");
        }

        if (m_auroraManager == null)
        {
            AttemptToFindAuroraXrManager();

            if (m_auroraManager == null)
            {
                Debug.LogError("Unable to find AURORA_Manager with name \"" + m_AuroraManagerGameObjectName + "\"");
            }
            else
            {
                m_isInitialized = true;
            }
        }
		
    }

    public AURORA_Context PrimaryAuroraContext
    {
        get
        {
            return m_primaryContext;
        }
        set
        {
            m_primaryContext = value;
        }
    }

    public AURORA_Logger AuroraLogger
    {
        get
        {
            if(m_auroraManager != null)
                return m_auroraManager.AuroraLogger;
            return null;
        }
    }

    public AURORA_Manager AURORA_Manager
    {
        get
        {
			if (!m_isInitialized)
				Initialize();

            return m_auroraManager;
        }
    }

    public AURORA.ClientAPI.AuroraInterface AURORA
    {
        get
        {
            if (m_auroraManager == null)
			{
				Initialize();
				if (m_auroraManager == null)
					return null;
			}

            if(m_auroraManager != null)
                return m_auroraManager.AURORA_Interface;

            return null;
        }
    }
}