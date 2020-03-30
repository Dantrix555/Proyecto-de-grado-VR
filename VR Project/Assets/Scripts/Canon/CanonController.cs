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

    [Header("Debug References")]
    [SerializeField] private Text _debugText = default;

    private GameObject _shotComponent;
    private bool _canShot;
    private float _nextFire;
    private float _absorbSpeed;

    private void Awake()
    {
        PlayerEvents.OnTriggerDown += OnOculusTriggerDown;
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
    }

    private void OnOculusTriggerDown()
    {
        if (_pointer.currentObject != null)
        {
            switch (_pointer.currentObject.tag)
            {
                case "Floor":
                    GameManager.MainCanvas.FadePanelWindow.SetFadeAnimation();
                    transform.position = new Vector3(_pointer.hitPoint.x, transform.position.y, _pointer.hitPoint.z);
                    break;
                case "Component":
                    GameObject componentObject = _pointer.currentObject;
                    componentObject.GetComponent<Rigidbody>().velocity = (transform.position - componentObject.transform.position) * _absorbSpeed;
                    break;
                default:
                    Shot();
                    break;
            }
        }
        else
            Shot();
    }

    private void Shot()
    {
        if(_canShot)
        {
            _nextFire = Time.time;
            _shotComponent = Instantiate(_shotComponent, _pointer.shotPoint.transform.position, Quaternion.identity, transform);
            //Set to the pointer position because this script is attached to OVRCameraRig GameObject the absolute father of the other objects 
            //And the father has blocked its rotations
            _shotComponent.GetComponent<ShotableComponent>().SetVelocity(_pointer.transform.forward, 2 * _absorbSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Component")
        {
            other.gameObject.transform.parent.GetComponent<SpawnerController>().Invoke("SpawnComponent", 15);
            _shotComponent = other.gameObject.GetComponent<GetAtAbleComponent>().GetShotPrefab();
            _debugText.text = _shotComponent.name;
            _canShot = true;
            Destroy(other.gameObject);
        }
    }
}
