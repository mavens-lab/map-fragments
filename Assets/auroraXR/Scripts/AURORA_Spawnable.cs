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

public class AURORA_Spawnable : MonoBehaviour
{
    [SerializeField]
    private string m_spawnableTypeUuid;

	[Tooltip("This will override a manual type UUID.")]
	[SerializeField]
	private AURORA_Spawnable m_auroraSpawnableReference;

    System.Guid m_spawnableTypeGuid;

    public System.Guid SpawnableTypeUuid
    {
        get
        {
            if (m_spawnableTypeGuid == null || m_spawnableTypeGuid == System.Guid.Empty)
            {
                m_spawnableTypeGuid = new System.Guid(m_spawnableTypeUuid);
            }
            return m_spawnableTypeGuid;
        }
        set
        {
            m_spawnableTypeGuid = value;
			m_spawnableTypeUuid = m_spawnableTypeGuid.ToString();
        }
    }

	private void Awake()
	{
		if (m_auroraSpawnableReference != null)
			SpawnableTypeUuid = m_auroraSpawnableReference.SpawnableTypeUuid;
	}

	private void Reset()
    {
        m_spawnableTypeUuid = System.Guid.NewGuid().ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
