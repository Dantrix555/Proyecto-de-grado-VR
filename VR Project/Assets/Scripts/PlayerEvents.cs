using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
    #region Events
    public static UnityAction OnTouchpadUp = null;
    public static UnityAction OnTouchpadDown = null;
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

        //If no controllers, then headset
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
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            if (OnTouchpadDown != null)
            {
                OnTouchpadDown();
            }
        }

        //Touchpad Up (touchpad is released)
        if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
        {
            if (OnTouchpadUp != null)
            {
                OnTouchpadUp();
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
