using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public GameObject _bullet;
    public GameObject _shootPoint;
    public Pointer _pointer;
    public float _fireRate;
    public bool _canShot;
    private GameObject _absorbableObject;
    private float _maxPointerDistance;
    private float _nextFire;
    private float _absorbSpeed;

    void Start()
    {
        _absorbSpeed = 5;
        _canShot = false;
        _nextFire = 0;
    }

    void Update()
    {
        Shoot();
        Absorb();
        TeleportFade();
    }

    void Shoot()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && Time.time > _nextFire && _canShot)
        {
            _nextFire = Time.time + _fireRate;
            Instantiate(_bullet, _shootPoint.transform.position, _shootPoint.transform.rotation);
        }
    }

    void Absorb()
    {
        //10 stands for absorbableObject Layer
        if (_pointer.m_currentObject.layer == 10 && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && !_canShot)
        {
            _absorbableObject = _pointer.m_currentObject;
            _absorbableObject.GetComponent<Rigidbody>().velocity = (transform.position - _absorbableObject.transform.position) * _absorbSpeed;
        }
        if (_pointer.m_currentObject.layer == 10 && OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && !_canShot)
        {
            _absorbableObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    void TeleportFade()
    {
        if (_pointer.m_currentObject.tag == "Teleport" && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            Vector3 _currentPlayerPosition = transform.position;
            _pointer.m_currentObject.GetComponent<TeleportController>().FadeIn();
            StartCoroutine(TeleportPlayer(_pointer.m_currentObject, _currentPlayerPosition));
        }
    }

    IEnumerator TeleportPlayer(GameObject _teleportPoint, Vector3 _playerPosition)
    {
        yield return new WaitForSeconds(2f);
        transform.position = _teleportPoint.transform.position;
        _teleportPoint.transform.position = _playerPosition;
    }

    private void OnTriggerEnter(Collider _other)
    {
        //Detect the collision with the player
        if (_other.gameObject == _absorbableObject)
        {
            Destroy(_other.gameObject);
            _absorbableObject = null;
            _canShot = true;
        }
    }
}
