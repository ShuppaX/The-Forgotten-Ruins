using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BananaSoup
{
    public class MeleeRaycast : MonoBehaviour
    {
    private NavMeshAgent enemy;// assign navmesh agent
    private Transform playerTarget;// reference to player's position
    private float attackRadius = 10.0f; // radius where enemy will spot player
    public Transform[] destinationPoints;// array of points for enemy to patrol
    private int currentDestination;// reference to current position
    public bool canSeePlayer = false;
    private Ray enemyEyes;
    public RaycastHit hitData;


    private void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
        playerTarget = GameObject.Find("Player").GetComponent<Transform>();
        enemyEyes = new Ray(transform.position, transform.forward);
    }
    private void Start()
    {
        Physics.Raycast(enemyEyes, attackRadius);
    }
    private void Update()
    {
        Lurk();
        Debug.DrawRay(transform.position, transform.forward * attackRadius);
    }

    void Lurk()
    {
        Debug.Log("Lurking");
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        //check if raycast hits playerLayer and enemy is close enough to attack
        if (Physics.Raycast(enemyEyes, out hitData, attackRadius * 2, layerMask: ~6) && distanceToPlayer < attackRadius)
        {
            Debug.Log("You hit " + hitData.collider.gameObject.name);
            ChasePlayer();
        }
        else
        {
            canSeePlayer = false;
            Patrol();
        }
    }
    void Patrol()
    {
        if (!canSeePlayer && enemy.remainingDistance < 0.5f)
        {
            enemy.destination = destinationPoints[currentDestination].position;
            UpdateCurrentPoint();
        }
    }

    void UpdateCurrentPoint()
    {
        if (currentDestination == destinationPoints.Length - 1)
        {
            currentDestination = 0;
        }
        else
        {
            currentDestination++;
        }
    }

    void ChasePlayer()
    {
        StartCoroutine(ChaseTime());
        canSeePlayer = true;
        transform.LookAt(playerTarget.position);
        Vector3 moveTo = Vector3.MoveTowards(transform.position, playerTarget.position, attackRadius);
        enemy.SetDestination(moveTo);
        
    }


    IEnumerator ChaseTime()
    {
        Debug.Log("Chasing");
        yield return new WaitForSeconds(10.0f);
        if (!Physics.Raycast(enemyEyes, out hitData, attackRadius * 2))
        {
            canSeePlayer = false;
            Debug.Log("Lurking");
            Lurk();
        }
    }
}
}
