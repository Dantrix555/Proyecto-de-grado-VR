using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController : MonoBehaviour
{
    [Header("Pointer GameObject")]
    [SerializeField] private Pointer _pointer = default;
    [Space(5)]
    [SerializeField] private Animator _canonAnimator = default;
    
    //Public reference to the canon pointer
    public Pointer Pointer { get => _pointer; }
    
    private GameObject _shotComponentPrefab;
    private string _componentFormula;
    private bool _canShot = false;
    private float _nextFire = 0.5f;
    private float _absorbSpeed = 2.5f;

    private void Awake()
    {
        PlayerEvents.OnTriggerDown += OnOculusTriggerDown;
        PlayerEvents.OnBackButtonDown += OnButtonPause;
        PlayerEvents.OnTouchpadTouch += RotateArrow;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnTriggerDown -= OnOculusTriggerDown;
        PlayerEvents.OnBackButtonDown -= OnButtonPause;
        PlayerEvents.OnTouchpadTouch -= RotateArrow;
    }

    private void RotateArrow(Vector2 fingerPosition)
    {
        //Taken from https://answers.unity.com/questions/361658/precise-rotation-based-on-joystick-axis-input-for.html
        _pointer.Arrow.transform.eulerAngles = new Vector3(0, Mathf.Atan2(fingerPosition.x, fingerPosition.y) * 180 / Mathf.PI, 0);
    }

    private void OnOculusTriggerDown()
    {
        if(InGameManager.CanUseGameControls && !InGameManager.IsGamePaused && !InGameManager.IsInDescription)
        {
            if (_pointer.currentObject != null)
            {
                switch (_pointer.currentObject.tag)
                {
                    case "Floor":
                        InGameManager.MainCanvas.FadePanelWindow.SetFadeAnimation();
                        StartCoroutine(InGameManager.TeleportPlayer(new Vector3(_pointer.hitPoint.x, transform.position.y, _pointer.hitPoint.z), _pointer.Arrow.transform.eulerAngles, gameObject));
                        break;
                    case "Component":
                        GameObject componentObject = _pointer.currentObject;
                        componentObject.transform.parent.GetComponent<Rigidbody>().velocity = (transform.position - componentObject.transform.position) * _absorbSpeed;
                        break;
                    default:
                        Shot();
                        break;
                }
            }
            else
                Shot();
        }
        else if(!InGameManager.CanUseGameControls || InGameManager.IsGamePaused || InGameManager.IsInDescription)
        {
            if (_pointer.currentObject != null)
            {
                switch (_pointer.currentObject.tag)
                {
                    case "Button":
                        if (_pointer.currentObject.GetComponent<ButtonController>().IsInteractable)
                        {
                            _pointer.currentObject.GetComponent<ButtonController>().CheckOption();
                        }
                        break;
                }
            }
        }
    }

    private void OnButtonPause()
    {
        if(InGameManager.CanUseGameControls && !InGameManager.IsInDescription)
        {
            if(InGameManager.IsGamePaused)
            {
                InGameManager.ResumeGame();
            }
            else
            {
                InGameManager.SetPause();
            }
        }
    }

    private void Shot()
    {
        if(_canShot && _shotComponentPrefab != null)
        {
            _canonAnimator.SetTrigger("Shot");
            Invoke("CanShot", 0.25f);
            _canShot = false;
            GameObject _shotComponentInstance = Instantiate(_shotComponentPrefab, _pointer.shotPoint.transform.position, Quaternion.identity, transform);
            //Set to the pointer position because this script is attached to OVRCameraRig GameObject the absolute father of the other objects 
            //And the father has blocked its rotations
            _shotComponentInstance.GetComponent<ShotableComponent>().SetVelocity(_pointer.transform.forward, 2 * _absorbSpeed);
            _shotComponentInstance.GetComponent<ShotableComponent>().ComponentFormula = _componentFormula;
        }
    }

    private void CanShot()
    {
        _canShot = true;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Component" && InGameManager.CanUseGameControls)
        {
            GetAtAbleComponent gotComponent = other.gameObject.transform.parent.GetComponent<GetAtAbleComponent>();
            gotComponent.RespawnComponent();
            _shotComponentPrefab = gotComponent.ShotComponentPrefab;
            _componentFormula = gotComponent.Formula;
            _pointer.SetComponentText(_componentFormula);
            _canShot = true;
            if (!InGameManager.ComponentIsInDex(gotComponent.Id))
            {
                InGameManager.ActivateDescription(true);
                InGameManager.GameUI.FactsController.SetText(gotComponent.Description);
                InGameManager.GameUI.FactsController.ShowFact();
            }
            Destroy(gotComponent.gameObject);

        }
    }
}
