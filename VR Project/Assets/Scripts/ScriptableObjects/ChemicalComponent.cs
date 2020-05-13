using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Component", menuName ="Chemical Component")]
public class ChemicalComponent : ScriptableObject
{
    public int id;
    public new string name;
    public string formula;
    public string description;
    public string useDescription;
    public GameObject componentObject;
    public Material imageDescription;
    public GameObject shotObject;
}
