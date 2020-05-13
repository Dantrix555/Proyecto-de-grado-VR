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

    [Header("AI and Path")]
    //Nav Mesh reference for the player pathfinding
    [SerializeField] private NavMeshAgent _enemyNavMesh = default;
    [SerializeField] private Transform _pathPointA;
    [SerializeField] private Transform _pathPointB;

    [Space(5)]
    [Header("Player Detection")]
    [SerializeField] private GameObject objectToChase = default;

    //Layer to detect player mask
    [SerializeField] private LayerMask _objectMask = default;

    private bool _isDamagable = true;
    private IEnumerator _movePatrol;

    private void OnDisable()
    {
        StopCoroutine(_movePatrol);
        _movePatrol = null;
    }

    void Update()
    {
        if (_isDamagable && InGameManager.CanUseGameControls && !InGameManager.IsGamePaused && !InGameManager.IsInDescription)
        {
            //Check if player is near to the abstract sphere
            if (Physics.CheckSphere(transform.position, 8, _objectMask))
            {
                //Set player position as enemy destination
                _enemyNavMesh.SetDestination(objectToChase.transform.position);
                //Stop coroutine and set as null
                if(_movePatrol != null)
                {
                    StopCoroutine(_movePatrol);
                    _movePatrol = null;
                }
            }

            //If move patrol isn't set, create and start a new one
            if (_movePatrol == null && !Physics.CheckSphere(transform.position, 8, _objectMask))
            {
                //Create a move patrol until enemy detect the player
                _movePatrol = MovePatrol();
                StartCoroutine(_movePatrol);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionGameObject = other.gameObject;
        if (collisionGameObject.tag == "Shot" && collisionGameObject.GetComponent<ShotableComponent>().ComponentFormula == WeaknessComponent.ToString() && _isDamagable)
        {
            transform.parent.position -= transform.parent.forward;
            _isDamagable = false;
            Destroy(collisionGameObject);
            _enemyLife -= 3;

            if(_enemyLife <= 0)
            {
                SoundManager.LoadSoundEffect(SoundManager.SFX.EnemyDestroyed);
                SetDestroyAnimation();
            }
            else
            {
                SoundManager.LoadSoundEffect(SoundManager.SFX.EnemyCorrectHit);
                SetDamageAnimation();
                Invoke("SetEnemyDamagable", 0.5f);
            }
        }
        else if(collisionGameObject.tag == "Shot" && !(collisionGameObject.GetComponent<ShotableComponent>().ComponentFormula == WeaknessComponent.ToString()))
            SoundManager.LoadSoundEffect(SoundManager.SFX.EnemyFailHit);
    }

    private void SetEnemyDamagable()
    {
        _isDamagable = true;
    }

    //Enemy Basic Movement from point A to point B
    private IEnumerator MovePatrol()
    {
        _enemyNavMesh.SetDestination(_pathPointA.position);
        yield return new WaitForSeconds(3f);
        _enemyNavMesh.SetDestination(_pathPointB.position);
        yield return new WaitForSeconds(3f);
        StartCoroutine(MovePatrol());
    }
}
