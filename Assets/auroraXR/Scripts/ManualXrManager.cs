#if USE_UNITYXR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public class ManualXrManager : MonoBehaviour
{
    [SerializeField]
    private bool m_initializeOnStart;

    public bool InitializeOnStart { get => m_initializeOnStart; set => m_initializeOnStart = value; }

    private void Start()
    {
        if(m_initializeOnStart)
        {
            StartXR();
        }
    }

    protected IEnumerator DoStartXR()
    {
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
        if(XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("XR initialization failed.");
        }
        else
        {
            Debug.Log("XR Initializing...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            yield return null;
        }
    }

    public void StartXR()
    {
        StartCoroutine("DoStartXR");
    }

    public void StopXR()
    {
        if(XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            Camera.main.ResetAspect();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }
    }
}
#endif