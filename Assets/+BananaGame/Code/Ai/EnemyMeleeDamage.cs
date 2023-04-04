using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Serialization;

namespace BananaSoup
{
    public class EnemyMeleeDamage : Damager
    {
        public bool canAttack = true;
        public float attackTimeFrame = 1.5f;
        public MeleeRaycast meleeRaycast;
        public GameObject weapon;
        private Coroutine resetAttackCooldown = null;
        private AudioSource meleeSwingAudio;


        private void Awake()
        {
            // Animator anim = weapon.GetComponent<Animator>();
            meleeSwingAudio = GetComponent<AudioSource>();
        }

        public void MeleeAttack()
        {
            //anim.SetTrigger("Attack");
            AudioManager.PlayClip(meleeSwingAudio, SoundEffect.EnemySwing);

            if (resetAttackCooldown != null)
            {
                Debug.Log("Attack on cooldown");
                StartCoroutine(AttackReset());
            }
        }

        public override void OnTriggerEnter(Collider collision)
        {
            if (!canAttack) return;

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