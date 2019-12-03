using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private GameObject absorbable;
    private Rigidbody rigidbody;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        absorbable = GameObject.Find("PR_Absorbable");
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
