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
        SoundManager.LoadSoundEffect(SoundManager.SFX.SpawnComponent);
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        GameObject chemicalComponent = Instantiate(_spawnerComponent.componentObject, spawnPosition, _spawnerComponent.componentObject.transform.rotation, transform);
        GetAtAbleComponent getAtAbleComponent = chemicalComponent.GetComponent<GetAtAbleComponent>();
        getAtAbleComponent.Id = _spawnerComponent.id;
        getAtAbleComponent.Formula = _spawnerComponent.formula;
        getAtAbleComponent.Description = _spawnerComponent.description;
        getAtAbleComponent.UseDescription = _spawnerComponent.useDescription;
        getAtAbleComponent.ImageDescription = _spawnerComponent.imageDescription;
        getAtAbleComponent.ShotComponentPrefab = _spawnerComponent.shotObject;
    }

}
