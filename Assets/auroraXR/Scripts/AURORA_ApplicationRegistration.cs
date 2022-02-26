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

public class AURORA_ApplicationRegistration : MonoBehaviour
{
    public class AppProperty
    {
        public string key;
        public string value;
    }

    [SerializeField]
    AURORA_Manager m_auroraManager;

    [Tooltip("Used to get the AURORA Manager after scene loads.")]
    [SerializeField]
    AURORA_Proxy m_auroraProxy;

    [Tooltip("The parent for instances. Defaults to self.")]
    [SerializeField]
    Transform m_instanceParent;

    [SerializeField]
    private string m_applicationTypeUuid;

    [SerializeField]
    private bool m_limitOneInstancePerRoom;

    [SerializeField]
    private string m_applicationName;

    [Multiline(5)]
    [SerializeField]
    private string m_applicationDescription;

    [SerializeField]
    private List<string> m_applicationLabels;

    [SerializeField]
    private List<AppProperty> m_applicationProperties;

    private AURORA.ClientAPI.ApplicationTypeRegistration m_auroraApplicationRegistration;

    private void Reset()
    {
        m_applicationTypeUuid = System.Guid.NewGuid().ToString();
        m_instanceParent = transform;
        m_limitOneInstancePerRoom = true;
    }

    private void Awake()
    {
        RegisterApplication();       
    }

    protected void RegisterApplication()
    {
        if (m_auroraManager == null)
        {
            m_auroraManager = m_auroraProxy.AURORA_Manager;
        }
        if (m_auroraManager == null)
        {
            Debug.LogError("Unable to find AURORA interface.");
            return;
        }
        if (m_auroraApplicationRegistration == null)
        {
            m_auroraApplicationRegistration = m_auroraManager.AURORA_Interface.RegisterApplicationType(
                new System.Guid(m_applicationTypeUuid),
                m_applicationName,
                m_applicationDescription);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public System.Guid ApplicationTypeUuid
    {
        get
        {
            return new System.Guid((m_applicationTypeUuid));
        }
        set
        {
            if (value == null)
                m_applicationTypeUuid = null;
            else
                m_applicationTypeUuid = value.ToString();
        }
    }

    public AURORA.ClientAPI.ApplicationTypeRegistration AuroraApplicationRegistration
    {
        get
        {
            return m_auroraApplicationRegistration;
        }
    }

    public AURORA.ClientAPI.ApplicationInstance GetOrCreateApplicationInstance(System.Guid applicationInstanceUuid)
    {
        RegisterApplication();
        return m_auroraApplicationRegistration.GetOrCreateInstance(applicationInstanceUuid);
    }

//    public AURORA_ApplicationInstance GetOrCreateInstantiatedApplicationInstance(GameObject template)
//    {
//        GameObject go = Instantiate(template, m_instanceParent);

//        AURORA_ApplicationInstance aai = go.GetComponent<AURORA_ApplicationInstance>();

//        // If template doesn't have an AURORA_ApplicationInstance component on it, add one.
//        if(aai == null)
//        {
//            go.AddComponent<AURORA_ApplicationInstance>();
//        }

//        aai.AuroraApplicationInstance = GetOrCreateApplicationInstance();

//        return aai;
//    }
}
