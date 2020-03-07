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
        PlayerEvents.OnTouchpadDown += DiabsorbComponent;
    }

    void Start()
    {
        _absorbSpeed = 5;
        _canShot = false;
        _nextFire = 0.5f;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnTriggerDown -= OnOculusTriggerDown;
        PlayerEvents.OnTouchpadDown -= DiabsorbComponent;
    }

    private void OnOculusTriggerDown()
    {
        switch (_pointer.currentObject.tag)
        {
            case "Defaut":
                break;
            case "Floor":
                GameManager.MainCanvas.FadePanelWindow.SetFadeAnimation();
                transform.position = new Vector3(_pointer.hitPoint.x, transform.position.y, _pointer.hitPoint.z);
                break;
            case "Component":
                GameObject componentObject = _pointer.currentObject;
                componentObject.GetComponent<Rigidbody>().velocity = (transform.position - componentObject.transform.position);
                break;
        }

        Shot();
    }

    private void Shot()
    {
        if(_canShot)
        {
            _nextFire = Time.time;
            _shotComponent = Instantiate(_shotComponent, _pointer.shotPoint.transform.position, Quaternion.identity, transform);
            _shotComponent.GetComponent<ComponentController>().SetComponentVelocity(transform.forward);
        }
    }

    private void DiabsorbComponent()
    {
        _debugText.text = "No object";
        _shotComponent = null;
        _canShot = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Component")
        {
            other.gameObject.transform.parent.GetComponent<SpawnerController>().Invoke("SpawnComponent", 15);
            _shotComponent = other.gameObject.GetComponent<ComponentController>().GetShotPrefab();
            _debugText.text = _shotComponent.name;
            Destroy(other.gameObject);
            _canShot = true;
        }
    }
}
