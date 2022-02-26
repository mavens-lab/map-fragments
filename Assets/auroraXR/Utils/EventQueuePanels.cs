using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[System.Serializable]
//public class AURORA_GameObjectMetadataEvent : UnityEngine.Events.UnityEvent<AURORA_GameObject, AURORA.ClientAPI.RpcRequest> { }


public class EventQueuePanelBase<UnityEventT, EventParameterT> where UnityEventT : UnityEngine.Events.UnityEvent<EventParameterT>
{
    [Tooltip("The maximum number of events to process in a single frame")]
    [Range(1, 50)]
    public int MaxEventsPerFrame = 5;

    protected bool m_initialized = false;

    public UnityEventT EventHandlers;

    public AURORA.ClientAPI.AuroraEventQueue<EventParameterT> EventQueue;

    public EventQueuePanelBase()
    {
        m_initialized = false;
    }

    public virtual void Initialize()
    {
        EventQueue = new AURORA.ClientAPI.AuroraEventQueue<EventParameterT>();
        EventQueue.EventHandler += EventHandlers.Invoke;

        m_initialized = true;
    }

    public void InEvent(EventParameterT value)
    {
        if (m_initialized)
        {
            EventQueue.EnqueueEvent(value);
        }
    }

    public virtual void Invoke()
    {
        if (m_initialized)
            EventQueue.Invoke(MaxEventsPerFrame);
    }
}

public class OptionalEventQueuePanelBase<UnityEventT, EventParameterT> where UnityEventT : UnityEngine.Events.UnityEvent<EventParameterT>
{
    [Tooltip("To maximize performance, this event is only enabled if this box is checked.")]
    public bool Enabled;

    [Tooltip("The maximum number of events to process in a single frame")]
    [Range(1, 50)]
    public int MaxEventsPerFrame = 5;

    protected bool m_initialized = false;

    public UnityEventT EventHandlers;

    public AURORA.ClientAPI.AuroraEventQueue<EventParameterT> EventQueue;

    public OptionalEventQueuePanelBase()
    {
        m_initialized = false;
    }

    public virtual void Initialize()
    {
        if (Enabled)
        {
            EventQueue = new AURORA.ClientAPI.AuroraEventQueue<EventParameterT>();
            EventQueue.EventHandler += EventHandlers.Invoke;

            m_initialized = true;
        }
    }

    public void InEvent(EventParameterT value)
    {
        if (m_initialized)
        {
            EventQueue.EnqueueEvent(value);
        }
    }

    public virtual void Invoke()
    {
        if (m_initialized)
            EventQueue.Invoke(MaxEventsPerFrame);
    }
}

public class SubstitutionEventQueue1i1oPanelBase<UnityEventT, InTypeT, OutTypeT> where UnityEventT : UnityEngine.Events.UnityEvent<OutTypeT>
{
    [Space(10, order = 10)]
    [Tooltip("The maximum number of events to process in a single frame")]
    [Range(1, 50)]
    public int MaxEventsPerFrame = 5;

    protected bool m_initialized = false;

    [Space(10, order = 20)]
    // public AURORA_GameObjectEvent EventHandlers;
    public UnityEventT EventHandlers;

    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
    public AURORA.ClientAPI.AuroraSubstitutionEventQueue<InTypeT, OutTypeT> EventQueue;

    public SubstitutionEventQueue1i1oPanelBase()
    {
        m_initialized = false;
    }

    public virtual void Initialize(OutTypeT outValue)
    {
        EventQueue = new AURORA.ClientAPI.AuroraSubstitutionEventQueue<InTypeT, OutTypeT>(outValue);
        EventQueue.EventHandler += EventHandlers.Invoke;

        m_initialized = true;
    }

    public void InEvent(InTypeT value)
    {
        if(m_initialized)
        {
            EventQueue.EnqueueEvent(value);
        }
    }

    public virtual void Invoke()
    {
        if (m_initialized)
            EventQueue.Invoke(MaxEventsPerFrame);
    }
}

public class OptionalSubstitutionEventQueue1i1oPanelBase<UnityEventT, InTypeT, OutTypeT> where UnityEventT : UnityEngine.Events.UnityEvent<OutTypeT>
{
    [Tooltip("To maximize performance, this event is only enabled if this box is checked.")]
    public bool Enabled;

    [Tooltip("The maximum number of events to process in a single frame")]
    [Range(1, 50)]
    public int MaxEventsPerFrame = 5;

    protected bool m_initialized = false;

    [Space(10, order = 20)]
    // public AURORA_GameObjectEvent EventHandlers;
    public UnityEventT EventHandlers;

    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
    public AURORA.ClientAPI.AuroraSubstitutionEventQueue<InTypeT, OutTypeT> EventQueue;

    public OptionalSubstitutionEventQueue1i1oPanelBase()
    {
        m_initialized = false;
    }

    public virtual void Initialize(OutTypeT outValue)
    {
        if (Enabled)
        {
            EventQueue = new AURORA.ClientAPI.AuroraSubstitutionEventQueue<InTypeT, OutTypeT>(outValue);
            EventQueue.EventHandler += EventHandlers.Invoke;

            m_initialized = true;
        }
    }

    public void InEvent(InTypeT value)
    {
        if (m_initialized)
        {
            EventQueue.EnqueueEvent(value);
        }
    }

    public virtual void Invoke()
    {
        if (m_initialized)
            EventQueue.Invoke(MaxEventsPerFrame);
    }
}

[System.Serializable]
public class AugmentedEventQueue1i2oPanelBase<UnityEventT, InTypeT, OutTypeT> where UnityEventT : UnityEngine.Events.UnityEvent<OutTypeT, InTypeT>
{
    [Tooltip("The maximum number of events to process in a single frame")]
    [Range(1, 50)]
    public int MaxEventsPerFrame = 5;

    protected bool m_initialized = false;

    // public AURORA_GameObjectEvent EventHandlers;
    public UnityEventT EventHandlers;

    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
    public AURORA.ClientAPI.AuroraAugmentedEventQueue<InTypeT, OutTypeT> EventQueue;

    public AugmentedEventQueue1i2oPanelBase()
    {
        m_initialized = false;
    }

    public virtual void Initialize(OutTypeT outValue)
    {
        EventQueue = new AURORA.ClientAPI.AuroraAugmentedEventQueue<InTypeT, OutTypeT>(outValue);
        EventQueue.EventHandler += EventHandlers.Invoke;

        m_initialized = true;
    }

    public void InEvent(InTypeT value)
    {
        if (m_initialized)
        {
            EventQueue.EnqueueEvent(value);
        }
    }

    public void Invoke()
    {
        if (m_initialized)
            EventQueue.Invoke(MaxEventsPerFrame);
    }
}

[System.Serializable]
public class OptionalAugmentedEventQueue1i2oPanelBase<UnityEventT, InTypeT, OutTypeT> where UnityEventT : UnityEngine.Events.UnityEvent<OutTypeT, InTypeT>
{
    [Tooltip("To maximize performance, this event is only enabled if this box is checked.")]
    public bool Enabled;

    [Tooltip("The maximum number of events to process in a single frame")]
    [Range(1, 50)]
    public int MaxEventsPerFrame = 5;

    protected bool m_initialized = false;

    // public AURORA_GameObjectEvent EventHandlers;
    public UnityEventT EventHandlers;

    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
    public AURORA.ClientAPI.AuroraAugmentedEventQueue<InTypeT, OutTypeT> EventQueue;

    public OptionalAugmentedEventQueue1i2oPanelBase()
    {
        m_initialized = false;
    }

    public virtual void Initialize(OutTypeT outValue)
    {
        EventQueue = new AURORA.ClientAPI.AuroraAugmentedEventQueue<InTypeT, OutTypeT>(outValue);
        EventQueue.EventHandler += EventHandlers.Invoke;

        m_initialized = true;
    }

    public void InEvent(InTypeT value)
    {
        if (m_initialized)
        {
            EventQueue.EnqueueEvent(value);
        }
    }

    public void Invoke()
    {
        if (m_initialized)
            EventQueue.Invoke(MaxEventsPerFrame);
    }
}

public class AugmentedEventQueue2i3oPanelBase<UnityEventT, InType1T, InType2T, OutTypeT> 
    where UnityEventT : UnityEngine.Events.UnityEvent<OutTypeT, InType1T, InType2T>
{
    [Tooltip("The maximum number of events to process in a single frame")]
    [Range(1, 50)]
    public int MaxEventsPerFrame = 5;

    protected bool m_initialized = false;

    // public AURORA_GameObjectEvent EventHandlers;
    public UnityEventT EventHandlers;

    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
    public AURORA.ClientAPI.AuroraAugmentedEvent2Queue<InType1T, InType2T, OutTypeT> EventQueue;

    public AugmentedEventQueue2i3oPanelBase()
    {
        m_initialized = false;
    }

    public virtual void Initialize(OutTypeT outValue)
    {
        EventQueue = new AURORA.ClientAPI.AuroraAugmentedEvent2Queue<InType1T, InType2T, OutTypeT>(outValue);
        EventQueue.EventHandler += EventHandlers.Invoke;

        m_initialized = true;
    }

    public void InEvent(InType1T v1, InType2T v2)
    {
        if (m_initialized)
        {
            EventQueue.EnqueueEvent(v1, v2);
        }
    }

    public void Invoke()
    {
        if (m_initialized)
        {
            EventQueue.Invoke(MaxEventsPerFrame);
        }
    }
}

public class OptionalAugmentedEventQueue2i3oPanelBase<UnityEventT, InType1T, InType2T, OutTypeT>
    where UnityEventT : UnityEngine.Events.UnityEvent<OutTypeT, InType1T, InType2T>
{
    [Tooltip("To maximize performance, this event is only enabled if this box is checked.")]
    public bool Enabled;

    [Tooltip("The maximum number of events to process in a single frame")]
    [Range(1, 50)]
    public int MaxEventsPerFrame = 5;

    protected bool m_initialized = false;

    // public AURORA_GameObjectEvent EventHandlers;
    public UnityEventT EventHandlers;

    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
    public AURORA.ClientAPI.AuroraAugmentedEvent2Queue<InType1T, InType2T, OutTypeT> EventQueue;

    public OptionalAugmentedEventQueue2i3oPanelBase()
    {
        m_initialized = false;
    }

    public virtual void Initialize(OutTypeT outValue)
    {
        if (Enabled)
        {
            EventQueue = new AURORA.ClientAPI.AuroraAugmentedEvent2Queue<InType1T, InType2T, OutTypeT>(outValue);
            EventQueue.EventHandler += EventHandlers.Invoke;

            m_initialized = true;
        }
    }

    public void InEvent(InType1T v1, InType2T v2)
    {
        if (m_initialized)
        {
            EventQueue.EnqueueEvent(v1, v2);
        }
    }

    public void Invoke()
    {
        if (m_initialized)
        {
            EventQueue.Invoke(MaxEventsPerFrame);
        }
    }
}

public class AugmentedEventQueueei4oPanelBase<UnityEventT, InType1T, InType2T, InType3T, OutTypeT>
    where UnityEventT : UnityEngine.Events.UnityEvent<OutTypeT, InType1T, InType2T, InType3T>
{
    [Tooltip("The maximum number of events to process in a single frame")]
    [Range(1, 50)]
    public int MaxEventsPerFrame = 5;

    protected bool m_initialized = false;

    // public AURORA_GameObjectEvent EventHandlers;
    public UnityEventT EventHandlers;

    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
    public AURORA.ClientAPI.AuroraAugmentedEvent2Queue<InType1T, InType2T, InType3T, OutTypeT> EventQueue;

    public AugmentedEventQueueei4oPanelBase()
    {
        m_initialized = false;
    }

    public virtual void Initialize(OutTypeT outValue)
    {
        EventQueue = new AURORA.ClientAPI.AuroraAugmentedEvent2Queue<InType1T, InType2T, InType3T, OutTypeT>(outValue);
        EventQueue.EventHandler += EventHandlers.Invoke;

        m_initialized = true;
    }

    public void InEvent(InType1T v1, InType2T v2, InType3T v3)
    {
        if (m_initialized)
        {
            EventQueue.EnqueueEvent(v1, v2, v3);
        }
    }

    public void Invoke()
    {
        if (m_initialized)
        {
            EventQueue.Invoke(MaxEventsPerFrame);
        }
    }
}

public class OptionalAugmentedEventQueue3i4oPanelBase<UnityEventT, InType1T, InType2T, InType3T, OutTypeT>
    where UnityEventT : UnityEngine.Events.UnityEvent<OutTypeT, InType1T, InType2T, InType3T>
{
    [Tooltip("To maximize performance, this event is only enabled if this box is checked.")]
    public bool Enabled;

    [Tooltip("The maximum number of events to process in a single frame")]
    [Range(1, 50)]
    public int MaxEventsPerFrame = 5;

    protected bool m_initialized = false;

    // public AURORA_GameObjectEvent EventHandlers;
    public UnityEventT EventHandlers;

    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
    public AURORA.ClientAPI.AuroraAugmentedEvent2Queue<InType1T, InType2T, InType3T, OutTypeT> EventQueue;

    public OptionalAugmentedEventQueue3i4oPanelBase()
    {
        m_initialized = false;
    }

    public virtual void Initialize(OutTypeT outValue)
    {
        EventQueue = new AURORA.ClientAPI.AuroraAugmentedEvent2Queue<InType1T, InType2T, InType3T, OutTypeT>(outValue);
        EventQueue.EventHandler += EventHandlers.Invoke;

        m_initialized = true;
    }

    public void InEvent(InType1T v1, InType2T v2, InType3T v3)
    {
        if (m_initialized)
        {
            EventQueue.EnqueueEvent(v1, v2, v3);
        }
    }

    public void Invoke()
    {
        if (m_initialized)
        {
            EventQueue.Invoke(MaxEventsPerFrame);
        }
    }
} 

 

//public class EventQueuePanelBase<EventTypeT, OutEventTypeT>
//{
//    [Tooltip("The maximum number of events to process in a single frame")]
//    [Range(1, 50)]
//    public int MaxEventsPerFrame = 5;

//    protected bool m_initialized = false;

//    // public AURORA_GameObjectEvent EventHandlers;
//    public EventTypeT EventHandlers;

//    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
//    public AURORA.ClientAPI.AuroraEventQueue<OutEventTypeT> EventQueue;

//    public EventQueuePanelBase()
//    {
//        m_initialized = false;
//    }
//}

//public class SubstitutionEventQueuePanelBase<InTypeT, OutTypeT>
//{
//    [Tooltip("To maximize performance, this event is only enabled if this box is checked.")]
//    public bool Enabled;

//    [Tooltip("The maximum number of events to process in a single frame")]
//    [Range(1, 50)]
//    public int MaxEventsPerFrame = 5;

//    protected bool m_initialized = false;

//    // public AURORA_GameObjectEvent EventHandlers;
//    //public InEventTypeT EventHandlers;

//    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
//    public AURORA.ClientAPI.AuroraSubstitutionEventQueue<InTypeT, OutTypeT> EventQueue;

//    public SubstitutionEventQueuePanelBase()
//    {
//        m_initialized = false;
//    }
//}

//public class EventQueuePanelBase<EventTypeT, OutEventType1T, OutEventType2T>
//{
//    [Tooltip("To maximize performance, this event is only enabled if this box is checked.")]
//    public bool Enabled;

//    [Tooltip("The maximum number of events to process in a single frame")]
//    [Range(1, 50)]
//    public int MaxEventsPerFrame = 5;

//    protected bool m_initialized = false;

//    // public AURORA_GameObjectEvent EventHandlers;
//    public EventTypeT EventHandlers;

//    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
//    public AURORA.ClientAPI.AuroraEventQueue<OutEventType1T, OutEventType2T> EventQueue;

//    public EventQueuePanelBase()
//    {
//        m_initialized = false;
//    }
//}

//public class RequiredEventQueuePanelBase<EventTypeT, OutEventType1T, OutEventType2T>
//{
//    [Tooltip("The maximum number of events to process in a single frame")]
//    [Range(1, 50)]
//    public int MaxEventsPerFrame = 5;

//    protected bool m_initialized = false;

//    // public AURORA_GameObjectEvent EventHandlers;
//    public EventTypeT EventHandlers;

//    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
//    public AURORA.ClientAPI.AuroraEventQueue<OutEventType1T, OutEventType2T> EventQueue;

//    public RequiredEventQueuePanelBase()
//    {
//        m_initialized = false;
//    }
//}

//public class EventQueuePanelBase<OutUnityEventT, OutParam1T, OutParam2T, OutParam3T>
//{
//    //[System.Serializable]
//    //public class OutEvent : UnityEngine.Events.UnityEvent<OutEventType1T, OutEventType2T, OutEventType3T> { }

//    [Tooltip("To maximize performance, this event is only enabled if this box is checked.")]
//    public bool Enabled;

//    [Tooltip("The maximum number of events to process in a single frame")]
//    [Range(1, 50)]
//    public int MaxEventsPerFrame = 5;

//    protected bool m_initialized = false;

//    [SerializeField]    
//    public OutUnityEventT EventHandlers;

//    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
//    public AURORA.ClientAPI.AuroraEventQueue<OutParam1T, OutParam2T, OutParam3T> EventQueue;

//    public EventQueuePanelBase()
//    {
//        m_initialized = false;
//    }
//}

//public class RequiredEventQueuePanelBase<EventTypeT, OutEventType1T, OutEventType2T, OutEventType3T>
//{
//    [Tooltip("The maximum number of events to process in a single frame")]
//    [Range(1, 50)]
//    public int MaxEventsPerFrame = 5;

//    protected bool m_initialized = false;

//    // public AURORA_GameObjectEvent EventHandlers;
//    public EventTypeT EventHandlers;

//    // public AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject> EventQueue;
//    public AURORA.ClientAPI.AuroraEventQueue<OutEventType1T, OutEventType2T, OutEventType3T> EventQueue;

//    public RequiredEventQueuePanelBase()
//    {
//        m_initialized = false;
//    }
//}

//[System.Serializable]
//public class AuroraObjectEventQueuePanel : EventQueuePanelBase<AURORA_GameObjectEvent, AURORA_GameObject>
//{
//    private AURORA_GameObject m_auroraGameObject;

//    public void Initialize(AURORA_GameObject ago)
//    {
//        m_auroraGameObject = ago;

//        if(ago == null)
//        {
//            throw new AURORA.AuroraException("AURORA GameObject must not be null");
//        }
//        if (Enabled && !m_initialized)
//        {
//            m_initialized = true;

//            EventQueue = new AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject>();

//            EventQueue.EventHandler += EventHandlers.Invoke;
//        }
//    }

//    public void InEvent(AURORA.ClientAPI.AuroraObject auroraObject)
//    {
//        if (m_initialized)
//        {
//            EventQueue.EnqueueEvent(m_auroraGameObject);
//        }
//    }

//    public void Invoke()
//    {
//        if (Enabled)
//            EventQueue.Invoke(MaxEventsPerFrame);
//    }
//}

//[System.Serializable]
//public class RequiredAuroraObjectEventQueuePanel : EventQueuePanelBase<AURORA_GameObjectEvent, AURORA_GameObject>
//{
//    private AURORA_GameObject m_auroraGameObject;

//    public void Initialize(AURORA_GameObject ago)
//    {
//        m_auroraGameObject = ago;

//        if (!m_initialized)
//        {
//            m_initialized = true;

//            EventQueue = new AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject>();

//            EventQueue.EventHandler += EventHandlers.Invoke;
//        }
//    }

//    public void InEvent(AURORA.ClientAPI.AuroraObject auroraObject)
//    {
//        if(m_initialized)
//            EventQueue.EnqueueEvent(m_auroraGameObject);
//    }

//    public void Invoke()
//    {
//        if (m_initialized)
//             EventQueue.Invoke(MaxEventsPerFrame);
//    }
//}

//[System.Serializable]
//public class RequiredFundamentalAuroraObjectEventQueuePanel : EventQueuePanelBase<AURORA_GameObjectEvent, AURORA_GameObject>
//{
//    private AURORA_GameObject m_auroraGameObject;

//    public void Initialize(AURORA_GameObject ago)
//    {
//        m_auroraGameObject = ago;

//        if (!m_initialized)
//        {
//            m_initialized = true;

//            EventQueue = new AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject>();

//            EventQueue.EventHandler += EventHandlers.Invoke;
//        }
//    }

//    public void InEvent(AURORA.ClientAPI.AuroraObject auroraObject)
//    {
//        if (m_initialized)
//            EventQueue.EnqueueEvent(m_auroraGameObject);
//    }

//    public void Invoke()
//    {
//        if (m_initialized)
//            EventQueue.Invoke(MaxEventsPerFrame);
//    }
//}

//[System.Serializable]
//public class AuroraContextEventQueuePanel : SubstitutionEventQueue1i1oPanelBase<AURORA_ContextEvent, AURORA.ClientAPI.AuroraContext, AURORA_Context>
//{

//}

//[System.Serializable]
//public class RequiredAuroraContextEventQueuePanel : EventQueuePanelBase<AURORA_ContextEvent, AURORA_Context>
//{
//    public void Initialize()
//    {
//        if (!m_initialized)
//        {
//            m_initialized = true;

//            EventQueue = new AURORA.ClientAPI.AuroraEventQueue<AURORA_Context>();

//            EventQueue.EventHandler += EventHandlers.Invoke;
//        }
//    }

//    public void InEvent(AURORA_Context context)
//    {
//        if (m_initialized)
//            EventQueue.EnqueueEvent(context);
//    }

//    public void Invoke()
//    {
//        if (m_initialized)
//            EventQueue.Invoke(MaxEventsPerFrame);
//    }
//}


//[System.Serializable]
//public class OptionalRpcRequestEventQueuePanel : OptionalEventQueuePanelBase<AURORA_RpcRequestEvent, AURORA.ClientAPI.RpcRequest>
//{ }

//[System.Serializable]
//public class RequiredRpcRequestEventQueuePanel : EventQueuePanelBase<AURORA_RpcRequestEvent, AURORA.ClientAPI.RpcRequest>
//{
//    public void Initialize()
//    {
//        if (!m_initialized)
//        {
//            m_initialized = true;

//            EventQueue = new AURORA.ClientAPI.AuroraEventQueue<AURORA.ClientAPI.RpcRequest>();

//            EventQueue.EventHandler += EventHandlers.Invoke;
//        }
//    }

//    public void InEvent(AURORA.ClientAPI.RpcRequest rpcRequest)
//    {
//        if (m_initialized)
//            EventQueue.EnqueueEvent(rpcRequest);
//    }

//    public void Invoke()
//    {
//        if (m_initialized)
//            EventQueue.Invoke(MaxEventsPerFrame);
//    }
//}

//[System.Serializable]
//public class AuroraObjectRpcRequestEventQueuePanel : EventQueuePanelBase<AURORA_GameObjectRpcRequestEvent, AURORA_GameObject, AURORA.ClientAPI.RpcRequest>
//{
//    AURORA_GameObject m_auroraGameObject;

//    public void Initialize(AURORA_GameObject ago)
//    {
//        m_auroraGameObject = ago;

//        if (Enabled && !m_initialized)
//        {
//            m_initialized = true;

//            EventQueue = new AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject, AURORA.ClientAPI.RpcRequest>();

//            EventQueue.EventHandler += EventHandlers.Invoke;
//        }
//    }

//    public void InEvent(AURORA.ClientAPI.AuroraObject auroraObject, AURORA.ClientAPI.RpcRequest rpcRequest)
//    {
//        if (m_initialized)
//            EventQueue.EnqueueEvent(m_auroraGameObject, rpcRequest);
//    }

//    public void Invoke()
//    {
//        if (Enabled)
//            EventQueue.Invoke(MaxEventsPerFrame);
//    }
//}

//[System.Serializable]
//public class RequiredAuroraObjectRpcRequestEventQueuePanel : RequiredEventQueuePanelBase<AURORA_GameObjectRpcRequestEvent, AURORA_GameObject, AURORA.ClientAPI.RpcRequest>
//{
//    private AURORA_GameObject m_auroraGameObject;

//    public void Initialize(AURORA_GameObject ago)
//    {
//        m_auroraGameObject = ago;

//        if (!m_initialized)
//        {
//            m_initialized = true;

//            EventQueue = new AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject, AURORA.ClientAPI.RpcRequest>();

//            EventQueue.EventHandler += EventHandlers.Invoke;
//        }
//    }

//    public void InEvent(AURORA.ClientAPI.AuroraObject auroraObject, AURORA.ClientAPI.RpcRequest rpcRequest)
//    {
//        if (m_initialized)
//            EventQueue.EnqueueEvent(m_auroraGameObject, rpcRequest);
//    }

//    public void Invoke()
//    {
//        if (m_initialized)
//            EventQueue.Invoke(MaxEventsPerFrame);
//    }
//}

//[System.Serializable]
//public class AURORA_GameObjectRpcRequestEventQueuePanel 
//    : EventQueuePanelBase<AURORA_GameObjectRpcRequestEvent, AURORA_GameObject, AURORA.ClientAPI.RpcRequest>
//{
//    AURORA_GameObject m_auroraGameObject;

//    public void Initialize(AURORA_GameObject ago)
//    {
//        m_auroraGameObject = ago;

//        if (Enabled && !m_initialized)
//        {
//            m_initialized = true;

//            EventQueue = new AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject, AURORA.ClientAPI.RpcRequest>();

//            EventQueue.EventHandler += EventHandlers.Invoke;
//        }
//    }

//    public void InEvent(AURORA.ClientAPI.AuroraObject auroraObject, AURORA.ClientAPI.RpcRequest rpcRequest)
//    {
//        if (m_initialized)
//            EventQueue.EnqueueEvent(m_auroraGameObject, rpcRequest);
//    }

//    public void Invoke()
//    {
//        if (Enabled)
//            EventQueue.Invoke(MaxEventsPerFrame);
//    }
//}

//[System.Serializable]
//public class AURORA_GameObjectMetadataEventQueuePanel
//    : EventQueuePanelBase<AURORA_GameObjectMetadataWrapperEvent, AURORA_GameObject, string, AURORA.Protobuf.MetadataWrapper>
//{
//    private AURORA_GameObject m_auroraGameObject;

//    public void Initialize(AURORA_GameObject ago)
//    {
//        m_auroraGameObject = ago;

//        if (Enabled && !m_initialized)
//        {
//            m_initialized = true;

//            EventQueue = new AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject, string, AURORA.Protobuf.MetadataWrapper>();

//            EventQueue.EventHandler += EventHandlers.Invoke;
//        }
//    }

//    public void InEvent(AURORA.ClientAPI.MetadataCollection metadataCollection, string key, AURORA.Protobuf.MetadataWrapper metadataWrapper)
//    {
//        if (m_initialized)
//            EventQueue.EnqueueEvent(m_auroraGameObject, key, metadataWrapper);
//    }

//    public void Invoke()
//    {
//        if (Enabled)
//            EventQueue.Invoke(MaxEventsPerFrame);
//    }
//}

//[System.Serializable]
//public class AURORA_GameObjectMetadataKeyEventQueuePanel
//    : EventQueuePanelBase<AURORA_GameObjectMetadataKeyEvent, AURORA_GameObject, string>
//{
//    AURORA_GameObject m_auroraGameObject;

//    public void Initialize(AURORA_GameObject ago)
//    {
//        m_auroraGameObject = ago;

//        if (Enabled && !m_initialized)
//        {
//            m_initialized = true;

//            EventQueue = new AURORA.ClientAPI.AuroraEventQueue<AURORA_GameObject, string>();

//            EventQueue.EventHandler += EventHandlers.Invoke;
//        }
//    }

//    public void InEvent(AURORA.ClientAPI.MetadataCollection metadataCollection, string key)
//    {
//        if (m_initialized)
//            EventQueue.EnqueueEvent(m_auroraGameObject, key);
//    }

//    public void Invoke()
//    {
//        if (Enabled)
//            EventQueue.Invoke(MaxEventsPerFrame);
//    }
//}