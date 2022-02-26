using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirstLastNameMetadata : MonoBehaviour
{
    [SerializeField]
    string m_firstName;

    [SerializeField]
    string m_lastName;

    [SerializeField]
    [Range(1.0f, 5.0f)]
    float m_cycleIntervalSeconds = 2.0f;

    [SerializeField]
    StringEvent m_cycleEvent;

    float m_nextUpdateTime = 0f;

    int m_activeIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_nextUpdateTime = 0;
        m_activeIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= m_nextUpdateTime)
        {
            m_nextUpdateTime = Time.time + m_cycleIntervalSeconds;

            if(m_activeIndex == 0)
            {
                m_cycleEvent.Invoke(m_firstName);
                m_activeIndex = 1;
            }
            else
            {
                m_cycleEvent.Invoke(m_lastName);
                m_activeIndex = 0;
            }
        }
    }
}
