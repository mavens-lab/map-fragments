using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AURORA_SpatialAnchor : MonoBehaviour
{
    //protected Vector3 m_originalLocalPosition;
    //protected Quaternion m_originalLocalRotation;
    //protected Vector3 m_originalLocalScale;

    private void Start()
    {
        //m_originalLocalPosition = transform.localPosition;
        //m_originalLocalRotation = transform.localRotation;
        //m_originalLocalScale = transform.localScale;
    }

    public void ResetTransform()
    {
        transform.localPosition = Vector3.zero; ;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
