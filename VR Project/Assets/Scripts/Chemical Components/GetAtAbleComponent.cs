using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAtAbleComponent : MonoBehaviour
{
    private int _id = default;
    private string _formula = default;
    private string _description = default;
    private string _useDescription = default;
    private Material _imageDescription = default;
    private GameObject _shotComponentPrefab = default;

    public int Id { get => _id; set => _id = value; }
    public string Formula { get => _formula; set => _formula = value; }
    public string Description { get => _description; set => _description = value; }
    public string UseDescription { get => _useDescription; set => _useDescription = value; }
    public Material ImageDescription { get => _imageDescription; set => _imageDescription = value; }
    public GameObject ShotComponentPrefab { get => _shotComponentPrefab; set => _shotComponentPrefab = value; }

    public void RespawnComponent()
    {
        GetComponentInParent<SpawnerController>().Invoke("SpawnComponent", 20);
    }

}
