using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BananaSoup
{
    public class EnemyMeleeDamage : Damager
    {
        public GameObject weapon;
        public bool canAttack = true;
        public float attackTimeFrame = 1.5f;
        public MeleeRaycast meleeRaycast;
        private Coroutine resetAttackCooldown = null;


     

        public void MeleeAttack()
        {
            // Animator anim = weapon.GetComponent<Animator>();
            // anim.SetTrigger("Attack");
            //AudioSource ac = Getcomponent<AudioSource>();
            //ac.Play(MeleeSound);
            if (resetAttackCooldown != null)
            {
                StartCoroutine(AttackReset());
            }
        }

        public override void OnTriggerEnter(Collider collision)
        {
            if (!canAttack)
            {
                return;
            }

            base.OnTriggerEnter(collision);
        }

        //Script for Melee Attack cooldown
            private IEnumerator AttackReset()
            {
                canAttack = true;
                yield return new WaitForSeconds(attackTimeFrame);
                canAttack = false;
                resetAttackCooldown = null;
            }
        }
    }
