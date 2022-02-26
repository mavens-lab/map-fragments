using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour
{
    [SerializeField] Camera m_camera;
    Vector2 m_mousePos;
    Vector3 m_worldPos;
    Ray m_ray;
    AURORA_GameObject ago;

    private readonly Vector2 screenCenter = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

    private void Start()
    {
        ago = GetComponent<AURORA_GameObject>();
        if (!m_camera)
        {
            m_camera = Camera.main;
        }
    }

    void OnGUI()
    {
        m_mousePos = Mouse.current.position.ReadValue();
        m_worldPos = m_camera.ScreenToWorldPoint(new Vector3(m_mousePos.x, m_mousePos.y, 2));
        Vector3 viewportPos = m_camera.ScreenToViewportPoint(m_mousePos);

        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Mouse position in window: " + m_mousePos);
        GUILayout.Label("Mouse position in world: " + m_worldPos);
        GUILayout.Label("Mouse position in viewport: " + viewportPos);
        GUILayout.Label("Object is locally owned: " + ago.IsLocallyOwned); 
        GUILayout.EndArea();
    }

    void Update()
    {
        m_ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Mouse.current.leftButton.isPressed)
        {
            RaycastHit hit;

            if (Physics.Raycast(m_ray, out hit))
            {
                Debug.Log("Hit object " + hit.collider.gameObject.name + " at " + hit.point);

                GameObject go = hit.collider.gameObject;

                if (go == this.gameObject)
                {
                    Debug.Log("Object matched.");

                    if (ago != null)
                    {
                        if (!ago.IsLocallyOwned)
                        {
                            ago.RequestObjectOwnership();
                            Debug.Log("Ownership requested.");
                        }
                        else
                        {
                            go.transform.position = new Vector3(m_worldPos.x, m_worldPos.y, m_worldPos.z);
                        }
                    }
                }
            }
        }
    }
}