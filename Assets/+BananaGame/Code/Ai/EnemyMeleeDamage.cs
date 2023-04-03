using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class EnemyMeleeDamage : Damager
    {
        public GameObject weapon;
        public bool CanAttack = true;
        public float attackCooldown = 1f;
        public MeleeRaycast meleeRaycast;

        /*private void FixedUpdate() runs in MeleeRaycast
        {
            if (meleeRaycast._playerInAttackRange && CanAttack)
            {
                MeleeAttack();
            }
           
        }*/

        public void MeleeAttack()
        {
            CanAttack = false;
            Animator anim = weapon.GetComponent<Animator>();
            anim.SetTrigger("Attack");


            Collider weaponCollider = weapon.GetComponent<Collider>();
            if (weaponCollider != null && weaponCollider.enabled)
            {
                var bounds = weaponCollider.bounds;
                Collider[] hitColliders = Physics.OverlapBox(
                    bounds.center,
                    bounds.extents,
                    weaponCollider.transform.rotation
                );

                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.CompareTag("Player"))
                    {
                        hitCollider.gameObject.GetComponent<PlayerHealth>().DecreaseHealth(1);
                    }
                }

                //AudioSource ac = Getcomponent<AudioSource>();
                //ac.Play(MeleeSound);
                //StartCoroutine(ResetAttackCooldown());

            }

            /*
            IEnumerator ResetAttackCooldown()
            {
                yield return new WaitForSeconds(attackCooldown);
                CanAttack = true;
            }*/


        }
    }
}
