using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BananaSoup
{
    public class SimplestFollow : MonoBehaviour
    {
        private NavMeshAgent enemy;
        private PlayerController player;

        // Start is called before the first frame update
        void Start()
        {
            enemy = GetComponent<NavMeshAgent>();
            player = FindObjectOfType<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            enemy.SetDestination(player.transform.position);

        }
    }
}
