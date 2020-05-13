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
    private int _hp = 3;

    //Start the events for the oculus controls
    private void Awake()
    {
        PlayerEvents.OnTriggerDown += OnOculusTriggerDown;
        PlayerEvents.OnBackButtonDown += OnButtonPause;
        PlayerEvents.OnTouchpadTouch += RotateArrow;
    }

    //Destroy the events for the oculus controls
    private void OnDestroy()
    {
        PlayerEvents.OnTriggerDown -= OnOculusTriggerDown;
        PlayerEvents.OnBackButtonDown -= OnButtonPause;
        PlayerEvents.OnTouchpadTouch -= RotateArrow;
    }

    private void RotateArrow(Vector2 fingerPosition)
    {
        if(InGameManager.CanUseGameControls && !InGameManager.IsGamePaused && !InGameManager.IsInDescription)
        {
            //Taken from https://answers.unity.com/questions/361658/precise-rotation-based-on-joystick-axis-input-for.html

            //The move made in the touchpad is added to the actual Y rotation of the arrow to move according the arrow position
            //and not according to the Vector3.zero position
            _pointer.Arrow.transform.eulerAngles = new Vector3(0, (Mathf.Atan2(fingerPosition.x, fingerPosition.y) * 180 / Mathf.PI) 
                + _pointer.ArrowYRotation, 0);
        }
    }

    private void OnOculusTriggerDown()
    {
        //Verify if the game is not paused, in description and if player can use the controls
        if(InGameManager.CanUseGameControls && !InGameManager.IsGamePaused && !InGameManager.IsInDescription)
        {
            if (_pointer.currentObject != null)
            {
                switch (_pointer.currentObject.tag)
                {
                    //Set fade in animation and teleport player
                    case "Floor":
                        InGameManager.MainCanvas.FadePanelWindow.SetFadeAnimation();
                        StartCoroutine(InGameManager.TeleportPlayer(new Vector3(_pointer.hitPoint.x, transform.position.y, _pointer.hitPoint.z), _pointer.Arrow.transform.eulerAngles, gameObject));
                        break;
                    //Atract the component to the player position
                    case "Component":
                        GameObject componentObject = _pointer.currentObject;
                        componentObject.transform.parent.GetComponent<Rigidbody>().velocity = (transform.position - componentObject.transform.position) * _absorbSpeed;
                        SoundManager.LoadSoundEffect(SoundManager.SFX.ObjectAbsorption);
                        break;
                    //Atract the key to the player position
                    case "Key":
                        _pointer.currentObject.transform.parent.GetComponent<Rigidbody>().velocity = (transform.position - _pointer.currentObject.transform.position) * _absorbSpeed;
                        SoundManager.LoadSoundEffect(SoundManager.SFX.ObjectAbsorption);
                        break;
                    case "Door":
                        //If player has the key open (destroy) the door
                        if(InGameManager.PlayerHasKey)
                        {
                            _pointer.currentObject.GetComponent<DestructableObject>().SetDestroyAnimation();
                            SoundManager.LoadSoundEffect(SoundManager.SFX.CanOpenDoor);
                        }
                        else
                        {
                            SoundManager.LoadSoundEffect(SoundManager.SFX.CantOpenDoor);
                        }
                        break;
                    case "Portal":
                        InGameManager.MainCanvas.FadePanelWindow.SetFadeAnimation();
                        StartCoroutine(InGameManager.TeleportPlayerToPortal(new Vector3(_pointer.currentObject.transform.position.x, transform.position.y, _pointer.currentObject.transform.position.z), gameObject));
                        break;
                    //if the tag of the object is another and player can shot, Shoot a component
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
            SoundManager.LoadSoundEffect(SoundManager.SFX.Shoot);
            _canonAnimator.SetTrigger("Shot");
            Invoke("CanShot", 0.25f);
            _canShot = false;
            GameObject _shotComponentInstance = Instantiate(_shotComponentPrefab, _pointer.shotPoint.transform.position, Quaternion.identity, transform);
            //Set to the pointer position because this script is attached to OVRCameraRig GameObject the absolute father of the other objects 
            //And the father has blocked its rotations
            _shotComponentInstance.GetComponent<ShotableComponent>().SetVelocity(_pointer.transform.forward, 20);
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
            InGameManager.MainCanvas.FadePanelWindow.SetGotComponentAnimation();
            GetAtAbleComponent gotComponent = other.gameObject.transform.parent.GetComponent<GetAtAbleComponent>();
            gotComponent.RespawnComponent();
            _shotComponentPrefab = gotComponent.ShotComponentPrefab;
            _componentFormula = gotComponent.Formula;
            _pointer.SetComponentText(_componentFormula);
            _canShot = true;
            SoundManager.LoadSoundEffect(SoundManager.SFX.GotComponent);
            if (!InGameManager.ComponentIsInDex(gotComponent.Id))
            {
                InGameManager.ActivateDescription(true);
                InGameManager.GameUI.FactsController.SetFact(gotComponent);
                InGameManager.GameUI.FactsController.ShowFact();
            }
            Destroy(gotComponent.gameObject);

        }

        if(other.gameObject.tag == "Key" && InGameManager.CanUseGameControls)
        {
            SoundManager.LoadSoundEffect(SoundManager.SFX.GotKey);
            InGameManager.MainCanvas.FadePanelWindow.SetGotKeyAnimation();
            InGameManager.PlayerHasKey = true;
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "Enemy" && !InGameManager.PlayerIsDamaged)
        {
            InGameManager.PlayerIsDamaged = true;
            _hp--;
            if(_hp <= 0)
            {
                InGameManager.SetGameOver();
            }
            else
            {
                InGameManager.MainCanvas.FadePanelWindow.SetDamageFade();
                Invoke("ActivateDamage", 1.5f);
            }
        }
    }

    private void ActivateDamage()
    {
        InGameManager.PlayerIsDamaged = false;
    }
}
