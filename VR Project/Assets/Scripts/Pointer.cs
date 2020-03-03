﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Taken from https://www.youtube.com/playlist?list=PLmc6GPFDyfw9AaEsedQZ3-EJi5RnUN7VU

public class Pointer : MonoBehaviour
{
    [Header("Line Renderer")]
    public float lineDistance = 10;
    public LineRenderer lineRenderer = default;
    [Space(5)]
    [Header("Target Sprite")]
    [SerializeField] private SpriteRenderer _circleSpriteRenderer = default;
    [SerializeField] private Sprite _circleSprite = default;
    [Space(5)]
    [Header("Object layers")]
    public LayerMask everythingMask = 0;
    public LayerMask interactableMask = 0;
    [HideInInspector] public GameObject currentObject = null;
    [Space(5)]
    [Header("Canon Shot Point")]
    public Transform shotPoint = default;
    public Vector3 hitPoint = default;
    private Transform currentOrigin = null;

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
        hitPoint = UpdateLine();
        //Set the circle sprite renderer in the hit position
        _circleSpriteRenderer.gameObject.transform.position = hitPoint;
        currentObject = UpdatePointerStatus();
    }

    private Vector3 UpdateLine()
    {
        //Create Ray
        RaycastHit hit = CreateRaycast(everythingMask);

        //Default end
        Vector3 endPosition = currentOrigin.position + (currentOrigin.forward * lineDistance);

        //Check hit and set the line until the position of the object
        if (hit.collider != null)
            endPosition = hit.point;

        //Set Position
        lineRenderer.SetPosition(0, currentOrigin.position);
        lineRenderer.SetPosition(1, endPosition);
        return endPosition;
    }

    private void UpdateOrigin(OVRInput.Controller controller, GameObject controllerObject)
    {
        //Set origin of pointer acording to the control position
        currentOrigin = shotPoint;

        //Is the laser visible?
        if (controller == OVRInput.Controller.Touchpad)
        {
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;
        }
    }

    private GameObject UpdatePointerStatus()
    {
        //Create Ray
        RaycastHit hit = CreateRaycast(interactableMask);

        //Check hit
        if (hit.collider)
        {
            _circleSpriteRenderer.sprite = _circleSprite;
            return hit.collider.gameObject;
        }
        
        //Return
        return null;
    }

    private RaycastHit CreateRaycast(int layer)
    {
        RaycastHit hit;
        Ray ray = new Ray(currentOrigin.position, currentOrigin.forward);
        Physics.Raycast(ray, out hit, lineDistance, layer);
        return hit;
    }

    public void SetLineColor(bool setColor)
    {
        if (!lineRenderer)
            return;

        Color endColor = Color.red;
        //Pointer has color
        if (setColor)
            endColor.a = 0.0f;
        //Pointer is transparent
        else
            endColor.a = 1.0f;
        lineRenderer.endColor = endColor;
    }
}
