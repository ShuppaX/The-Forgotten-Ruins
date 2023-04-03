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


        private void Awake()
        {
            // Animator anim = weapon.GetComponent<Animator>();
            //AudioSource ac = Getcomponent<AudioSource>();
        }

        public void MeleeAttack()
        {
            //anim.SetTrigger("Attack");
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
