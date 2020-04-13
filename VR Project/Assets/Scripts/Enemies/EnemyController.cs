using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum Weakness
    {
        KMnO4,
        NaClO,
        C3H8O2,
        H3BO3
    }

    [SerializeField] private Weakness _weaknessComponent = default;

    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionGameObject = other.gameObject;
        if (collisionGameObject.tag == "Shot" && collisionGameObject.GetComponent<ShotableComponent>().ComponentFormula == _weaknessComponent.ToString())
        {
            //Simulates a shot
            Destroy(collisionGameObject);
            Destroy(gameObject);
        }
    }
}
