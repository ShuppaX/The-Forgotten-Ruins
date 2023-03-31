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
        
        //TODO clean up instances of this script
        private void FixedUpdate()
        {
            if (meleeRaycast._playerInAttackRange && CanAttack)
            {
                meleeAttack();
            }
           
        }

        public void meleeAttack()
        {
            CanAttack = false;
            Animator anim = weapon.GetComponent<Animator>();
            anim.SetTrigger("Attack");
            
            //AudioSource ac = Getcomponent<AudioSource>();
            //ac.Play(MeleeSound);
            StartCoroutine(ResetAttackCooldown());
            
        }
        
        IEnumerator ResetAttackCooldown()
        {
            yield return new WaitForSeconds(attackCooldown);
            CanAttack = true;
        }
        
        /*public override void OnDamageTaken(DamageInfo damageInfo) 
        {
            //Debug.Log("EnemyMeleeDamage: OnDamageTaken");
            meleeRaycast._stunned = true;
            meleeRaycast.enemyStunnedRoutine = StartCoroutine(meleeRaycast.StunEnemy());
        }*/
        
        
    }
}
