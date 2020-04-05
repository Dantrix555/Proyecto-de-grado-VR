using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotableComponent : MonoBehaviour
{

    private string _componentFormula = default;

    public string ComponentFormula { get => _componentFormula; set => _componentFormula = value; }

    public void SetVelocity(Vector3 direction, float speed)
    {
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    //Temporally everything destroys the shot
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
