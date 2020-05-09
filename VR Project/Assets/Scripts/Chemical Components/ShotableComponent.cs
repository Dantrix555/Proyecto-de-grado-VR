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
        Destroy(gameObject, 5);
    }

    //Temporally everything destroys the shot
    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.tag == "Wall" || other.gameObject.tag == "Water") && _componentFormula == other.gameObject.GetComponent<DestructableObject>().WeaknessComponent.ToString())
        {
            other.gameObject.GetComponent<DestructableObject>().SetDestroyAnimation();
            Destroy(gameObject);
        }
        if(other.gameObject.tag != "Enemy" && other.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
