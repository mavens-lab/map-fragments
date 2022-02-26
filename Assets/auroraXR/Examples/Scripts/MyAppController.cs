using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAppController : MonoBehaviour
{

    //[SerializeField] AURORA_ApplicationTopic m_appTopic;

    //AURORA.ClientAPI.Subscriber m_sub; 

    //// Start is called before the first frame update
    //void Start()
    //{
    //    m_sub = m_appTopic.CreateConnectedSubscriber();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    int limiter = 10;
    //    NetMQ.NetMQMessage receivedAppMsg = new NetMQ.NetMQMessage();

    //    while (limiter > 0 && m_sub.TryGetAuroraMessage(out AURORA.ClientAPI.AuroraMessage am))
    //    {
    //        --limiter; 

    //        if (am.FrameCount > 1)
    //        {
    //            //Debug.Log("[cot] " + am[1].ConvertToString()); 
    //        }
    //    }
    //}
}
