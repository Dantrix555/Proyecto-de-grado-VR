using System.Collections;
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

    [Space(5)]
    [SerializeField] private GameObject _arrow;
    public GameObject Arrow { get => _arrow; }

    //Text reader variables
    private string _currentText = default;
    private string _currentComponent = default;

    private Transform _currentOrigin = null;
    private bool _arrowHasBeenSet = false;

    //Start y transform rotation of the arrow
    private float _arrowYRotation = default;
    public float ArrowYRotation { get => _arrowYRotation; }

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
        Vector3 endPosition = _currentOrigin.position + (_currentOrigin.forward * lineDistance);

        Color endLineColor = Color.white;

        if(hit.collider == null)
        {
            SetArrowInScene(false);
        }

        //Check hit and set the line until the position of the object
        if (hit.collider != null && InGameManager.CanUseGameControls && !InGameManager.IsGamePaused && !InGameManager.IsInDescription)
        {
            if(hit.collider.tag == "Floor")
            {
                SetArrowInScene(true);
            }
            else
            {
                SetArrowInScene(false);
            }

            endPosition = hit.point;
            switch (hit.collider.tag)
            {
                case "Floor":
                    _arrow.transform.position = endPosition;
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
                case "Wall":
                    if(hit.collider.gameObject.GetComponent<DestructableObject>().WeaknessComponent.ToString() == _currentComponent)
                    {
                        endLineColor = Color.green;
                        _currentText = "Derretir";
                    }
                    else
                    {
                        endLineColor = Color.red;
                        _currentText = "Busca " + hit.collider.gameObject.GetComponent<DestructableObject>().WeaknessComponent.ToString();
                    }
                    break;
                case "Water":
                    if (hit.collider.gameObject.GetComponent<DestructableObject>().WeaknessComponent.ToString() == _currentComponent)
                    {
                        endLineColor = Color.green;
                        _currentText = "Purificar";
                    }
                    else
                    {
                        endLineColor = Color.red;
                        _currentText = "Busca " + hit.collider.gameObject.GetComponent<DestructableObject>().WeaknessComponent.ToString();
                    }
                    break;
                case "Key":
                    endLineColor = Color.yellow;
                    _currentText = "Llave";
                    break;
                case "Door":
                    if(InGameManager.PlayerHasKey)
                    {
                        endLineColor = Color.green;
                        _currentText = "Abrir";
                    }
                    else
                    {
                        endLineColor = Color.gray;
                        _currentText = "Busca la llave";
                    }
                    break;
                case "Portal":
                    endLineColor = Color.cyan;
                    _currentText = "Meta";
                    break;
                default:
                    endLineColor = Color.white;
                    _currentText = _currentComponent;
                    break;
            }
        }
        else if (hit.collider != null && (!InGameManager.CanUseGameControls || InGameManager.IsGamePaused || InGameManager.IsInDescription))
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

        //Set Position
        lineRenderer.SetPosition(0, _currentOrigin.position);
        lineRenderer.SetPosition(1, endPosition);
        _reader.SetReaderText(_currentText);
        return endPosition;
    }

    private void UpdateOrigin(OVRInput.Controller controller, GameObject controllerObject)
    {
        //Set origin of pointer acording to the control position
        _currentOrigin = shotPoint;

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
        Ray ray = new Ray(_currentOrigin.position, _currentOrigin.forward);
        Physics.Raycast(ray, out hit, lineDistance, layer);
        return hit;
    }

    private void SetArrowInScene(bool newArrowActiveState)
    {
        _arrow.SetActive(newArrowActiveState);
        if(!_arrowHasBeenSet && newArrowActiveState)
        {
            _arrowYRotation = transform.eulerAngles.y;
            _arrowHasBeenSet = true;
            _arrow.transform.eulerAngles = new Vector3(_arrow.transform.eulerAngles.x, transform.eulerAngles.y, _arrow.transform.eulerAngles.z);
        }
        else if(_arrowHasBeenSet && !newArrowActiveState)
        {
            _arrowHasBeenSet = false;
        }
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
