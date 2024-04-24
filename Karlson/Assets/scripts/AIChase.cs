using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    NavMeshAgent enemy;
    public Transform target;
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        enemy.SetDestination(target.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 13)
        {
            Destroy(gameObject);
        }
    }
}
