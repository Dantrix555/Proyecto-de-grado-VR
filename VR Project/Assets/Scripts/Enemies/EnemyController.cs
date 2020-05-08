using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : DestructableObject
{

    //Animator to control move and attack patrols
    [SerializeField] private Animator _enemyAnimator = default;

    //HP variable to control the enemy life
    [SerializeField] private int _enemyLife = default;

    [Header("AI")]
    //Nav Mesh reference for the player pathfinding
    [SerializeField] private NavMeshAgent _enemyNavMesh = default;

    [Space(5)]
    [Header("Player Detection")]
    [SerializeField] private GameObject objectToChase = default;

    //Layer to detect player mask
    [SerializeField] private LayerMask _objectMask = default;

    private bool _isDamagable = true;
    
    void Update()
    {
        if (_isDamagable)
        {
            //Check if player is near to the abstract sphere
            if (Physics.CheckSphere(transform.position, 8, _objectMask))
            {
                _enemyNavMesh.SetDestination(objectToChase.transform.position);
            }
        }
    }


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

    private IEnumerator MovePatrol()
    {

        yield return new WaitForSeconds(1f);
    }
}
