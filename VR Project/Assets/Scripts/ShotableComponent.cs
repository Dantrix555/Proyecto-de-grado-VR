using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotableComponent : MonoBehaviour
{
    [Header("Rigidbody Reference")]
    [SerializeField] private Rigidbody _rigidbody = default;
    
    public void SetVelocity(Vector3 direction, float speed)
    {
        _rigidbody.velocity = direction * speed;
    }
}
