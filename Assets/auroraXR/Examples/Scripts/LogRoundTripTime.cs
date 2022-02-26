using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogRoundTripTime : MonoBehaviour
{
    public void HandleRountTripTimeUpdate(AURORA_GameObject ago, AURORA.ClientAPI.AuroraObject ao, System.TimeSpan rtt)
    {
        Debug.Log("Object " + ago.UUID.ToString() + " Update Round Trip Time = " + rtt.TotalMilliseconds.ToString() + " ms");
    }
}
