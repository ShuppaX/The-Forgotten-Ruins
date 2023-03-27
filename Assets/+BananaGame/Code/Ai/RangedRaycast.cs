using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class RangedRaycast : MeleeRaycast
    {
        public GameObject projectile;
        public GameObject firingPoint;

        [SerializeField] private Transform aimpoint;


        public override void Attack()
        {
            //Stop enemy movement
            _enemy.SetDestination(transform.position);

            // transform.LookAt(_playerTarget);

            if (!alreadyAttacked)
            {

                Rigidbody rb = Instantiate(projectile, firingPoint.transform.position, aimpoint.rotation)
                    .GetComponent<Rigidbody>();
                Debug.Log("pew");

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), _timeBetweenAttacks);
            }

            _lastDidSomething = Time.time;
        }
    }
}