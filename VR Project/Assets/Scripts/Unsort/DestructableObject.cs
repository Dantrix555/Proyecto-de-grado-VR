using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{

    public enum Weakness
    {
        None,
        KMnO4,
        NaClO,
        C3H8O2,
        H3BO3
    }

    [SerializeField] private Weakness _weaknessComponent = default;
    public Weakness WeaknessComponent { get => _weaknessComponent; }

    //Animator to set damage and destroy objects
    [SerializeField] private Animator _objectAnimator = default;

    //Time according animator to destroy the object
    [SerializeField] private float _destroyTimeInSeconds;

    public void SetDestroyAnimation()
    {
        _objectAnimator.SetTrigger("Destroy");
        Invoke("DestroyObject", _destroyTimeInSeconds);
    }

    //Damage animation is only necesary on enemies, call this method in enemy controller script
    public void SetDamageAnimation()
    {
        _objectAnimator.SetTrigger("Damage");
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

}
