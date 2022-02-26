using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager3D : MonoBehaviour
{
    public Transform m_raycastStartTransform;
    public Material m_hitMaterial;
    public GameObject m_cursorVisual;
    public float m_rayCastLength;
    public string[] m_selectableTags;
    public GameObject m_hitGameObject;
    public Vector3 m_hitWorldPosition;
    public Vector3 m_hitWorldNormal;
    //public GameObject m_spawnableObject;

    [SerializeField] SpawnObjectCopyTransform m_spawnObjectCopyTransform;

    [SerializeField] LaserPointer_3D m_LaserPointer3D;

    private void Update()
    {
        RayCast3D();

        //if(OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        //{
        //    if(OVRInput.GetUp(OVRInput.Button.One))
        //    {
        //        if (m_hitGameObject != null)
        //        {
        //            //SpawnAt3DHitLoc(m_spawnableObject, m_hitWorldPosition);
        //            m_spawnObjectCopyTransform.SpawnInstance();
        //        }
        //    }
        //    else if(OVRInput.GetUp(OVRInput.Button.Two))
        //    {
        //        if (m_hitGameObject != null && m_hitGameObject.CompareTag("Spawned"))
        //            DestroySpawnedObject(m_hitGameObject);
        //    }
        //}
    }

    public void RayCast3D()
    {
        
        RaycastHit hit;
        if(Physics.Raycast(m_raycastStartTransform.position, m_raycastStartTransform.forward, out hit, m_rayCastLength))
        { 
            bool isSelectable = false;
            foreach (string s in m_selectableTags)
            {
                if (hit.collider.gameObject.CompareTag(s))
                {
                    isSelectable = true;
                }
            }
            if (isSelectable)
            {
                m_hitGameObject = hit.collider.gameObject;
                m_hitWorldPosition = hit.point;
                m_hitWorldNormal = hit.normal;
                //m_hitGameObject.GetComponent<MeshRenderer>().material = m_hitMaterial;
                m_LaserPointer3D.SetCursorStartDest(m_raycastStartTransform.position, m_hitWorldPosition);  
            }
            else
            {
                m_hitGameObject = null;
                m_hitWorldPosition = new Vector3(0, 0, 0);
                m_hitWorldNormal = new Vector3(0, 0, 0);
            }
        }
        else
        {
            m_hitGameObject = null;
            m_hitWorldPosition = new Vector3(0, 0, 0);
            m_hitWorldNormal = new Vector3(0, 0, 0);
            m_LaserPointer3D.SetCursorRay(m_raycastStartTransform);
        }
    }

    public void DestroySpawnedObject(GameObject go)
    {
		AURORA_GameObject ago = go.GetComponent<AURORA_GameObject>();

		if (ago != null)
		{
			// Destroy it on the server and remotely, but not locally.
			ago.AuroraDestroy();
		}

		// Destroy thing locally
		Destroy(go);
    }
}