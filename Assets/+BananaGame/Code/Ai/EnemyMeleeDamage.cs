using System.Collections;
using UnityEngine;

namespace BananaSoup
{
    public class EnemyMeleeDamage : Damager
    {
        private bool canDealDamage = true;
        public float attackTimeFrame = 1.5f;
        public MeleeRaycast meleeRaycast;
        private Coroutine resetAttackCooldown = null;
        private AudioSource meleeSwingAudio;

        public bool CanDealDamage
        {
            get { return canDealDamage; }
        }


        private void Awake()
        {
            meleeSwingAudio = GetComponent<AudioSource>();
        }

        public void MeleeAttack()
        {
            //anim.SetTrigger("Attack");
            AudioManager.PlayClip(meleeSwingAudio, SoundEffect.EnemySwing);
        }

        public override void OnTriggerEnter(Collider collision)
        {
            if (!canDealDamage) return;

            base.OnTriggerEnter(collision);

            if ( resetAttackCooldown != null )
            {
                Debug.Log("Attack on cooldown");
                resetAttackCooldown = StartCoroutine(AttackReset());
            }
        }

        //Script for Melee Attack cooldown
        private IEnumerator AttackReset()
        {
            canDealDamage = true;
            yield return new WaitForSeconds(attackTimeFrame);
            canDealDamage = false;
            resetAttackCooldown = null;
        }
    }
}