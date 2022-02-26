using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCTest : MonoBehaviour
{
    public void OnAuroraObjectRPCEvent()//AURORA_GameObject ago)//, AURORA.ClientAPI.RpcRequest rpcRequest)
    {
        Debug.Log("RPC Result "); // + rpcRequest.RpcResponseMessage.ToString());
    }
}
