using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : DestructableObject
{

    //Animator to control move and attack patrols
    [SerializeField] private Animator _enemyAnimator = default;

    //HP variable to control the enemy life
    [SerializeField] private int _enemyLife = default;

    private bool _isDamagable = true;

    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionGameObject = other.gameObject;
        if (collisionGameObject.tag == "Shot" && collisionGameObject.GetComponent<ShotableComponent>().ComponentFormula == WeaknessComponent.ToString() && _isDamagable)
        {
            _isDamagable = false;
            Destroy(collisionGameObject);
            _enemyLife -= 3;

            if(_enemyLife <= 0)
                SetDestroyAnimation();
            else
            {
                SetDamageAnimation();
                Invoke("SetEnemyDamagable", 0.5f);
            }
        }
    }

    private void SetEnemyDamagable()
    {
        _isDamagable = true;
    }
}
