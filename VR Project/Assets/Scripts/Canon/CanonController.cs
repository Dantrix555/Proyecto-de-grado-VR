using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanonController : MonoBehaviour
{
    [Header("Pointer GameObject")]
    [SerializeField] private Pointer _pointer = default;
    
    //Public reference to the canon pointer
    public Pointer Pointer { get => _pointer; }
    
    private GameObject _shotComponentPrefab;
    private string _componentFormula;
    private bool _canShot;
    private float _nextFire;
    private float _absorbSpeed;

    private void Awake()
    {
        PlayerEvents.OnTriggerDown += OnOculusTriggerDown;
        PlayerEvents.OnBackButtonDown += OnButtonPause;
    }

    void Start()
    {
        _absorbSpeed = 2.5f;
        _canShot = false;
        _nextFire = 0.5f;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnTriggerDown -= OnOculusTriggerDown;
        PlayerEvents.OnBackButtonDown -= OnButtonPause;
    }

    private void OnOculusTriggerDown()
    {
        if(!InGameManager.IsGamePaused && !InGameManager.IsInDescription && !InGameManager.GameOver)
        {
            if (_pointer.currentObject != null)
            {
                switch (_pointer.currentObject.tag)
                {
                    case "Floor":
                        InGameManager.MainCanvas.FadePanelWindow.SetFadeAnimation();
                        StartCoroutine(InGameManager.TeleportPlayer(new Vector3(_pointer.hitPoint.x, transform.position.y, _pointer.hitPoint.z), gameObject));
                        break;
                    case "Component":
                        GameObject componentObject = _pointer.currentObject;
                        componentObject.GetComponent<Rigidbody>().velocity = (transform.position - componentObject.transform.position) * _absorbSpeed;
                        break;
                    case "Button":
                        if(_pointer.currentObject.GetComponent<ButtonController>().IsInteractable)
                        {
                            _pointer.currentObject.GetComponent<ButtonController>().CheckOption();
                        }
                        break;
                    default:
                        Shot();
                        break;
                }
            }
            else
                Shot();
        }
        else
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
        if(InGameManager.IsGameScene && !InGameManager.IsInDescription && !InGameManager.GameOver)
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
        if(_canShot)
        {
            _nextFire = Time.time;
            GameObject _shotComponentInstance = Instantiate(_shotComponentPrefab, _pointer.shotPoint.transform.position, Quaternion.identity, transform);
            //Set to the pointer position because this script is attached to OVRCameraRig GameObject the absolute father of the other objects 
            //And the father has blocked its rotations
            _shotComponentInstance.GetComponent<ShotableComponent>().SetVelocity(_pointer.transform.forward, 2 * _absorbSpeed);
            _shotComponentInstance.GetComponent<ShotableComponent>().ComponentFormula = _componentFormula;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Component")
        {
            other.gameObject.transform.parent.GetComponent<SpawnerController>().Invoke("SpawnComponent", 15);
            _shotComponentPrefab = other.gameObject.GetComponent<GetAtAbleComponent>().GetShotPrefab();
            _componentFormula = other.gameObject.GetComponent<GetAtAbleComponent>().GetComponentFormula();
            _pointer.SetComponentText(_componentFormula);
            _canShot = true;
            Destroy(other.gameObject);
        }
    }
}
