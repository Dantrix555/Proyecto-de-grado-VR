using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAtAbleComponent : MonoBehaviour
{
    private int id = default;
    private string formula = default;
    private string description = default;
    private GameObject shotComponentPrefab = default;

    public int Id { get => id; set => id = value; }
    public string Formula { get => formula; set => formula = value; }
    public string Description { get => description; set => description = value; }
    public GameObject ShotComponentPrefab { get => shotComponentPrefab; set => shotComponentPrefab = value; }

    public void RespawnComponent()
    {
        GetComponentInParent<SpawnerController>().Invoke("SpawnComponent", 10);
    }

}
