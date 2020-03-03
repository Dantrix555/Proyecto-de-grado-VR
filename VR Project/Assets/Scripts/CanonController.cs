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

    private bool _canShot;
    private float _nextFire;
    private float _absorbSpeed;

    private void Awake()
    {
        PlayerEvents.OnTouchpadDown += OnOculusTriggerDown;
    }

    void Start()
    {
        _absorbSpeed = 5;
        _canShot = false;
        _nextFire = 0;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnTouchpadDown -= OnOculusTriggerDown;
    }

    private void OnOculusTriggerDown()
    {
        switch(_pointer.currentObject.tag)
        {
            case "Floor":
                GameManager.MainCanvas.FadePanelWindow.SetFadeAnimation();
                Transform floorTransform = _pointer.currentObject.transform;
                transform.position = new Vector3(floorTransform.position.x, transform.position.y, floorTransform.position.z);
                break;
            case "Component":
                GameManager.MainCanvas.FadePanelWindow.SetFadeAnimation();
                break;
        }
    }
}
