using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [Header("Component Scriptable Object")]
    [SerializeField] private ChemicalComponent _spawnerComponent;

    public void SpawnComponent()
    {
        if (transform.childCount > 0) { return; }
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        GameObject chemicalComponent = Instantiate(_spawnerComponent.componentObject, spawnPosition, Quaternion.identity, transform);
        chemicalComponent.GetComponent<GetAtAbleComponent>().SetComponentValues(_spawnerComponent.id, _spawnerComponent.formula, _spawnerComponent.shotObject);
    }

}
