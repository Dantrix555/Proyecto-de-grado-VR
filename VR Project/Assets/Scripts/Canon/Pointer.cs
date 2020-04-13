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
    [Header("Object layers")]
    public LayerMask everythingMask = 0;
    public LayerMask interactableMask = 0;
    [HideInInspector] public GameObject currentObject = null;

    [Space(5)]
    [Header("Canon Shot Point")]
    public Transform shotPoint = default;
    [HideInInspector] public Vector3 hitPoint = default;

    [Space(5)]
    [Header("Canon Reader")]
    [SerializeField] private CanonReaderCanvas _reader = default;
    public CanonReaderCanvas Reader { get => _reader; }

    //Text reader variables
    private string _currentText = default;
    private string _currentComponent = default;

    private Transform currentOrigin = null;

    void Awake()
    {
        PlayerEvents.OnControllerSource += UpdateOrigin;
    }

    void Start()
    {
        SetLineColor(Color.white);
    }

    void OnDestroy()
    {
        PlayerEvents.OnControllerSource -= UpdateOrigin;
    }

    void Update()
    {
        hitPoint = UpdateLine();
        currentObject = UpdatePointerStatus();
    }

    private Vector3 UpdateLine()
    {
        //Create Ray
        RaycastHit hit = CreateRaycast(everythingMask);

        //Default end
        Vector3 endPosition = currentOrigin.position + (currentOrigin.forward * lineDistance);

        Color endLineColor = Color.white;

        //Check hit and set the line until the position of the object
        if (hit.collider != null && InGameManager.CanUseGameControls && !InGameManager.IsGamePaused)
        {
            endPosition = hit.point;
            switch (hit.collider.tag)
            {
                case "Floor":
                    endLineColor = Color.blue;
                    _currentText = "Teletransportar";
                    break;
                case "Component":
                    endLineColor = Color.green;
                    _currentText = "Absorber";
                    break;
                case "Enemy":
                    endLineColor = Color.red;
                    _currentText = "Dispara!";
                    break;
                default:
                    endLineColor = Color.white;
                    _currentText = _currentComponent;
                    break;
            }
        }
        else if (hit.collider != null && (!InGameManager.CanUseGameControls || InGameManager.IsGamePaused))
        {
            endPosition = hit.point;
            switch (hit.collider.tag)
            {
                case "Button":
                    if (hit.collider.gameObject.GetComponent<ButtonController>().IsInteractable)
                    {
                        endLineColor = Color.cyan;
                        _currentText = hit.collider.gameObject.GetComponent<ButtonController>().GetOptionName();
                    }
                    break;
                default:
                    endLineColor = Color.white;
                    _currentText = _currentComponent;
                    break;
            }
        }
        //Set Color according to the hit object collider if it has
        SetLineColor(endLineColor);
        _reader.SetReaderText(_currentText);

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

    public void SetLineColor(Color lineColor)
    {
        if (!lineRenderer)
            return;

        Color endColor = lineColor;
        lineRenderer.endColor = endColor;
    }

    public void SetComponentText(string _componentText)
    {
        _currentComponent = _componentText;
    }

}
