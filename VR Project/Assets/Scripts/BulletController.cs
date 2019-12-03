using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float _speed;
    private Rigidbody _rigidbody;

    void Start()
    {
        _speed = 5;
        _rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, 5);
    }

    void Update()
    {
        _rigidbody.velocity = transform.forward * _speed;
    }

    private void OnCollisionEnter(Collision _other)
    {
        if (_other.gameObject.tag == "Enemy")
        {
            Destroy(_other.gameObject);
        }
        Destroy(gameObject);
    }
}
