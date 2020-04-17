using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// This script is to detect which is the actual controller and the actions of the controller through events
/// </summary>

//Taken from https://www.youtube.com/playlist?list=PLmc6GPFDyfw9AaEsedQZ3-EJi5RnUN7VU

public class PlayerEvents : MonoBehaviour
{
    #region Events
    //Event to detect when is releasing the oculus go trigger button
    public static UnityAction OnBackButtonDown = null;
    //Event to detect when is holding the oculus go trigger button
    public static UnityAction OnTriggerDown = null;
    //Event to detect when is the player touching the touchpad
    public static UnityAction<Vector2> OnTouchpadTouch = null;
    //Event to get the actual controller
    public static UnityAction<OVRInput.Controller, GameObject> OnControllerSource = null;
    #endregion

    #region Anchors
    public GameObject m_leftAnchor;
    public GameObject m_rightAnchor;
    public GameObject m_headAnchor;
    #endregion

    #region Input
    private Dictionary<OVRInput.Controller, GameObject> m_controllersSet = null;
    private OVRInput.Controller m_inputSource = OVRInput.Controller.None;
    private OVRInput.Controller m_Controller = OVRInput.Controller.None;
    private bool m_inputActive = true;
    #endregion

    void Awake()
    {
        OVRManager.HMDMounted += PlayerFound;
        OVRManager.HMDUnmounted += PlayerLost;
        m_controllersSet = CreateControllerSets();
    }

    void OnDestroy()
    {
        OVRManager.HMDMounted -= PlayerFound;
        OVRManager.HMDUnmounted -= PlayerLost;
    }

    void Update()
    {
        //Check for active input
        if (!m_inputActive)
            return;

        //Check if a controller exists
        CheckForController();

        //Check for input source
        CheckInputSource();

        //Check for actual input
        Input();

    }

    void CheckForController()
    {
        OVRInput.Controller controllerCheck = m_Controller;

        //Rigth Remote
        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
            controllerCheck = OVRInput.Controller.RTrackedRemote;

        //Left Remote
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
            controllerCheck = OVRInput.Controller.LTrackedRemote;

        //If no controllers, then set headset
        if (!OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote) &&
            !OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
            controllerCheck = OVRInput.Controller.Touchpad;

        //Update 
        m_Controller = UpdateSource(controllerCheck, m_Controller);

    }

    void CheckInputSource()
    {
        //Update
        m_inputSource = UpdateSource(OVRInput.GetActiveController(), m_inputSource);
    }

    void Input()
    {
        //Touchpad Down (touchpad is pressed)
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (OnTriggerDown != null)
            {
                OnTriggerDown();
            }
        }

        //Touchpad Up (touchpad is released)
        if (OVRInput.GetDown(OVRInput.Button.Back))
        {
            if (OnBackButtonDown != null)
            {
                OnBackButtonDown();
            }
        }

        if(OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
        {
            if(OnTouchpadTouch != null)
            {
                OnTouchpadTouch(OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad));
            }
        }

    }

    void PlayerFound()
    {
        m_inputActive = true;
    }

    void PlayerLost()
    {
        m_inputActive = false;
    }

    private Dictionary<OVRInput.Controller, GameObject> CreateControllerSets()
    {
        Dictionary<OVRInput.Controller, GameObject> newSets = new Dictionary<OVRInput.Controller, GameObject>()
        {
            { OVRInput.Controller.LTrackedRemote, m_leftAnchor },
            { OVRInput.Controller.RTrackedRemote, m_rightAnchor },
            { OVRInput.Controller.Touchpad, m_headAnchor }
        };

        return newSets;
    }

    private OVRInput.Controller UpdateSource(OVRInput.Controller check, OVRInput.Controller previous)
    {
        //if values are the same, return
        if (check == previous)
        {
            return previous;
        }

        //Get controller object
        GameObject controllerObject = null;
        m_controllersSet.TryGetValue(check, out controllerObject);

        //If no controller, set to the head
        if (controllerObject == null)
        {
            controllerObject = m_headAnchor;
        }

        //Send out event
        if (OnControllerSource != null)
        {
            OnControllerSource(check, controllerObject);
        }

        //Return new value
        return check;
    }
}
