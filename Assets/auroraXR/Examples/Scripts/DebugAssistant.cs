using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAssistant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogDebug(string msg)
    {
        Debug.Log(msg);
    }

    public void LogWarning(string msg)
    {
        Debug.LogWarning(msg);
    }

    public void LogError(string msg)
    {
        Debug.LogError(msg);
    }

}
