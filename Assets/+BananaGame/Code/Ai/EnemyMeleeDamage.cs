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
            //var size = Physics.OverlapSphereNonAlloc(transform.position, 1f, results); Uses buffer to avoid garbage, idk how to use it though
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
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
        
        /*public override void OnDamageTaken(DamageInfo damageInfo) why did i even make this
        {
            //Debug.Log("EnemyMeleeDamage: OnDamageTaken");
            meleeRaycast._stunned = true;
            meleeRaycast.enemyStunnedRoutine = StartCoroutine(meleeRaycast.StunEnemy());
        }*/
        
        
    }
}
