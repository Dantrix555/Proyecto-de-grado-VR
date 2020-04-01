using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAtAbleComponent : MonoBehaviour
{
    private int id = default;
    private string formula = default;
    private GameObject shotComponentPrefab = default;

    [Header("Rigidbody Component")]
    [SerializeField] private Rigidbody _rigidbody = default;

    public void SetComponentValues(int id, string formula, GameObject shotComponentPrefab)
    {
        this.id = id;
        this.formula = formula;
        this.shotComponentPrefab = shotComponentPrefab;
    }

    public GameObject GetShotPrefab()
    {
        return shotComponentPrefab;
    }

    public string GetComponentFormula()
    {
        return formula;
    }

}
