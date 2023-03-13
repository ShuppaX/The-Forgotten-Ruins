using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class RangedRaycast : MeleeRaycast
    {
        public GameObject projectile;
        public GameObject firingPoint;
        
        
        
        public override void Attack()
        {
            //Stop enemy movement
            _enemy.SetDestination(transform.position);

            transform.LookAt(_playerTarget);

            if (!_alreadyAttacked)
            {

                Rigidbody rb = Instantiate(projectile, firingPoint.transform.position, Quaternion.identity)
                    .GetComponent<Rigidbody>();
                Debug.Log("pew");

                _alreadyAttacked = true;
                Invoke(nameof(ResetAttack), _timeBetweenAttacks);
            }

            _lastDidSomething = Time.time;
        }
    }
}