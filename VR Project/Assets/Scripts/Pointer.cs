using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pointer : MonoBehaviour
{
    public float m_Distance = 6;
    public LineRenderer m_lineRenderer;
    public LayerMask m_EverythingMask = 0;
    public LayerMask m_InteractableMask = 0;
    public UnityAction<Vector3, GameObject> OnPointerUpdate = null;
    public GameObject m_currentObject = null;

    private Transform m_currentOrigin = null;

    void Awake()
    {
        PlayerEvents.OnControllerSource += UpdateOrigin;
    }

    void Start()
    {
        SetLineColor(true);
    }

    void OnDestroy()
    {
        PlayerEvents.OnControllerSource -= UpdateOrigin;
    }

    void Update()
    {
        Vector3 hitPoint = UpdateLine();

        m_currentObject = UpdatePointerStatus();
        if (OnPointerUpdate != null)
            OnPointerUpdate(hitPoint, m_currentObject);
    }

    private Vector3 UpdateLine()
    {
        //Create Ray
        RaycastHit hit = CreateRaycast(m_EverythingMask);

        //Default end
        Vector3 endPosition = m_currentOrigin.position + (m_currentOrigin.forward * m_Distance);

        //Check hit and set the line until the position of the object
        if (hit.collider != null)
            endPosition = hit.point;

        //Set Position
        m_lineRenderer.SetPosition(0, m_currentOrigin.position);
        m_lineRenderer.SetPosition(1, endPosition);
        return endPosition;
    }

    private void UpdateOrigin(OVRInput.Controller controller, GameObject controllerObject)
    {
        //Set origin of pointer
        m_currentOrigin = controllerObject.transform;

        //Is the laser visible?
        if (controller == OVRInput.Controller.Touchpad)
        {
            m_lineRenderer.enabled = false;
        }
        else
        {
            m_lineRenderer.enabled = true;
        }
    }

    private GameObject UpdatePointerStatus()
    {
        //Create Ray
        RaycastHit hit = CreateRaycast(m_InteractableMask);

        //Check hit
        if (hit.collider)
            return hit.collider.gameObject;

        //Return
        return null;
    }

    private RaycastHit CreateRaycast(int layer)
    {
        RaycastHit hit;
        Ray ray = new Ray(m_currentOrigin.position, m_currentOrigin.forward);
        Physics.Raycast(ray, out hit, m_Distance, layer);
        return hit;
    }

    private void SetLineColor(bool setColor)
    {
        if (!m_lineRenderer)
            return;

        Color endColor = Color.white;
        //Pointer has color
        if (setColor)
            endColor.a = 0.0f;
        //Pointer is transparent
        else
            endColor.a = 1.0f;
        m_lineRenderer.endColor = endColor;
    }
}
