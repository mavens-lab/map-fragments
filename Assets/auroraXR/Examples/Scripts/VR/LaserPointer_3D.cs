using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class LaserPointer_3D : MonoBehaviour
{

    [SerializeField] InputManager3D m_InputManager3D;

    public enum LaserBeamBehavior
    {
        On,        // laser beam always on
        Off,        // laser beam always off
        OnWhenHitTarget,  // laser beam only activates when hit valid target
    }

    public GameObject cursorVisual;
    float maxLength;

    [SerializeField] private LaserBeamBehavior _laserBeamBehavior;

    public LaserBeamBehavior laserBeamBehavior
    {
        set
        {
            _laserBeamBehavior = value;
            if (laserBeamBehavior == LaserBeamBehavior.Off || laserBeamBehavior == LaserBeamBehavior.OnWhenHitTarget)
            {
                lineRenderer.enabled = false;
            }
            else
            {
                lineRenderer.enabled = true;
            }
        }
        get
        {
            return _laserBeamBehavior;
        }
    }

    private Vector3 m_startPoint;
    private Vector3 m_forward;
    private Vector3 m_endPoint;
    private bool m_hitTarget;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        if (cursorVisual) cursorVisual.SetActive(false);
        maxLength = m_InputManager3D.m_rayCastLength;
    }

    public void SetCursorStartDest(Vector3 start, Vector3 dest)
    {
        m_startPoint = start;
        m_endPoint = dest;
        m_hitTarget = true;
    }

    public void SetCursorRay(Transform t)
    {
        m_startPoint = t.position;
        m_forward = t.forward;
        m_hitTarget = false;
    }

    private void LateUpdate()
    {
        lineRenderer.SetPosition(0, m_startPoint);
        if (m_hitTarget)
        {
            lineRenderer.SetPosition(1, m_endPoint);
            UpdateLaserBeam(m_startPoint, m_endPoint);
            if (cursorVisual)
            {
                cursorVisual.transform.position = m_endPoint;
                cursorVisual.SetActive(true);
            }
        }
        else
        {
            UpdateLaserBeam(m_startPoint, m_startPoint + maxLength * m_forward);
            lineRenderer.SetPosition(1, m_startPoint + maxLength * m_forward);
            if (cursorVisual) cursorVisual.SetActive(false);
        }
    }

    // make laser beam a behavior with a prop that enables or disables
    private void UpdateLaserBeam(Vector3 start, Vector3 end)
    {
        if (laserBeamBehavior == LaserBeamBehavior.Off)
        {
            lineRenderer.enabled = false;
            return;
        }
        else if (laserBeamBehavior == LaserBeamBehavior.On)
        {
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }
        else if (laserBeamBehavior == LaserBeamBehavior.OnWhenHitTarget)
        {
            if (m_hitTarget)
            {
                if (!lineRenderer.enabled)
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, start);
                    lineRenderer.SetPosition(1, end);
                }
            }
            else
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                }
            }
        }
    }

    void OnDisable()
    {
        if (cursorVisual) cursorVisual.SetActive(false);
    }
}
