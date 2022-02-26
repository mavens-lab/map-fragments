using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class SpawnObject_VR : MonoBehaviour
{
    public AURORA_Spawnable m_Prefab;
    public Vector3 Point; 
    [SerializeField] AURORA_Proxy m_auroraProxy; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnObject()
    {
        var newobject = m_auroraProxy.PrimaryAuroraContext.Spawn(m_Prefab.SpawnableTypeUuid, Point);

        var ago = newobject.GetComponent<AURORA_GameObject>();

        if (ago != null)
        {
            //ago.TakeObjectOwnership();
        }

        newobject.transform.position = Point;
    }

    
}
