using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAtAbleComponent : MonoBehaviour
{
    private int id = default;
    private string formula = default;
    private string description = default;
    private GameObject shotComponentPrefab = default;
    
    public void SetComponentValues(int id, string formula, string description, GameObject shotComponentPrefab)
    {
        this.id = id;
        this.formula = formula;
        this.description = description;
        this.shotComponentPrefab = shotComponentPrefab;
    }

    public GameObject GetShotPrefab()
    {
        if(!InGameManager.ComponentIsInDex(id))
        {
            InGameManager.ActivateDescription(true);
            InGameManager.GameUI.FactsController.SetText(description);
            InGameManager.GameUI.FactsController.ShowFact();
        }
        return shotComponentPrefab;
    }

    public string GetComponentFormula()
    {
        return formula;
    }

    public void SpawnComponent()
    {
        GetComponentInParent<SpawnerController>().SpawnComponent();
    }

}
