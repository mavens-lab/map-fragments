using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnObject : MonoBehaviour, IMixedRealityPointerHandler
{
    public AURORA_Spawnable m_Prefab;
    public int RayDistance = 10;
    private Vector3 Point;
    public float m_ObjectLifetime = 5;

    [SerializeField] AURORA_Proxy m_auroraProxy;
    private void OnEnable()
    {
        CoreServices.InputSystem?.PushFallbackInputHandler(this.gameObject);
    }

    private void OnDisable()
    {
        CoreServices.InputSystem?.PopFallbackInputHandler();
    }

    public void TakeOwnershipAfterSpawn(AURORA.ClientAPI.AuroraObject auroraObject, AURORA.ClientAPI.RpcRequest rpcRequest)
    {
        auroraObject.RequestObjectOwnership();
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        var newobject = m_auroraProxy.PrimaryAuroraContext.Spawn(m_Prefab.SpawnableTypeUuid, Vector3.zero);
        newobject.AURORA_Object.ObjectSpawnSuccessfulHandler += TakeOwnershipAfterSpawn;

        var ago = newobject.GetComponent<AURORA_GameObject>();
        if (ago != null)
        {
            //ago.TakeObjectOwnership();
        }

        newobject.transform.position = eventData.Pointer.Result.Details.Point;

        Destroy(newobject.gameObject, m_ObjectLifetime);
    }
}
