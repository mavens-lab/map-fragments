using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AURORA_VrComponentSpawner : MonoBehaviour
{
    [SerializeField]
    bool m_spawnVrComponents = false;

    [SerializeField]
    AURORA_Context m_auroraContext;

    [SerializeField]
    Transform m_spawnParent;

    [SerializeField]
    GameObject m_headAnchor;

    [SerializeField]
    GameObject m_leftHandAnchor;

    [SerializeField]
    GameObject m_rightHandAnchor;

    [SerializeField]
    GameObject m_localHeadPrefab;

    [SerializeField]
    GameObject m_localLeftHandPrefab;

    [SerializeField]
    GameObject m_localRightHandPrefab;

    [SerializeField]
    GameObject m_ghostLeftHandPref;

    [SerializeField]
    GameObject m_ghostRightHandPrab;

    [SerializeField]
    AURORA_GameObject m_spawnedHead;

    [SerializeField]
    AURORA_GameObject m_spawnedLeftHand;

    [SerializeField]
    AURORA_GameObject m_spawnedRightHand;

    [SerializeField]
    AURORA_GameObject m_spawnedGhostLeftHand;

    [SerializeField]
    AURORA_GameObject m_spawnedGhostRightHand;

    // Start is called before the first frame update
    void Start()
    {
        if (m_auroraContext != null && m_spawnVrComponents)
        {
            if (m_spawnParent == null)
            {
                m_spawnParent = m_auroraContext.transform;
            }

            GameObject head = Instantiate(m_localHeadPrefab, m_spawnParent);
            GameObject leftHand = Instantiate(m_localLeftHandPrefab, m_spawnParent);
            GameObject rightHand = Instantiate(m_localRightHandPrefab, m_spawnParent);
            GameObject ghostLeftHand = null;
            GameObject ghostRightHand = null;

            if (m_ghostLeftHandPref != null)
            {
                ghostLeftHand = Instantiate(m_ghostLeftHandPref, m_spawnParent);
                m_spawnedGhostLeftHand = ghostLeftHand.GetComponent<AURORA_GameObject>();
            }

            if (m_ghostRightHandPrab != null)
            {
                ghostRightHand = Instantiate(m_ghostRightHandPrab, m_spawnParent);
                m_spawnedGhostRightHand = ghostRightHand.GetComponent<AURORA_GameObject>();
            }

            m_spawnedHead = head.GetComponent<AURORA_GameObject>();
            m_spawnedLeftHand = leftHand.GetComponent<AURORA_GameObject>();
            m_spawnedRightHand = rightHand.GetComponent<AURORA_GameObject>();

            UnityEngine.Animations.ParentConstraint headConstraint = head.GetComponent<UnityEngine.Animations.ParentConstraint>();
            UnityEngine.Animations.ParentConstraint leftHandConstraint = leftHand.GetComponent<UnityEngine.Animations.ParentConstraint>();
            UnityEngine.Animations.ParentConstraint rightHandConstraint = rightHand.GetComponent<UnityEngine.Animations.ParentConstraint>();

            UnityEngine.Animations.ConstraintSource headCS = new UnityEngine.Animations.ConstraintSource();
            headCS.sourceTransform = m_headAnchor.transform;
            headCS.weight = 1.0f;
            headConstraint.AddSource(headCS);
            headConstraint.constraintActive = true;

            UnityEngine.Animations.ConstraintSource leftHandCS = new UnityEngine.Animations.ConstraintSource();
            leftHandCS.sourceTransform = m_leftHandAnchor.transform;
            leftHandCS.weight = 1.0f;
            leftHandConstraint.AddSource(leftHandCS);
            leftHandConstraint.constraintActive = true;

            UnityEngine.Animations.ConstraintSource rightHandCS = new UnityEngine.Animations.ConstraintSource();
            rightHandCS.sourceTransform = m_rightHandAnchor.transform;
            rightHandCS.weight = 1.0f;
            rightHandConstraint.AddSource(rightHandCS);
            rightHandConstraint.constraintActive = true;

            if(ghostLeftHand != null)
            {
                m_spawnedLeftHand.SetGhostRepresentation(m_spawnedGhostLeftHand);
            }

            if (ghostRightHand != null)
            {
                m_spawnedRightHand.SetGhostRepresentation(m_spawnedGhostRightHand);
            }

            m_spawnedLeftHand.RequestObjectOwnership();
            m_spawnedRightHand.RequestObjectOwnership();
            m_spawnedHead.RequestObjectOwnership();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
