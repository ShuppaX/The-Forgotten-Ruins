using System.Collections;
using UnityEngine;

namespace BananaSoup
{
    public class EnemyMeleeDamage : Damager
    {
        private bool canDealDamage = true;
        private AudioSource meleeSwingAudio;

        public bool CanDealDamage
        {
            get => canDealDamage;
            set => canDealDamage = value;
        }


        private void Awake()
        {
            meleeSwingAudio = GetComponent<AudioSource>();
        }

        public void MeleeAttack()
        {
            AudioManager.PlayClip(meleeSwingAudio, SoundEffect.EnemySwing);
        }

        public override void OnTriggerEnter(Collider collision)
        {
            if (!canDealDamage) return;

            base.OnTriggerEnter(collision);

        }

        public IEnumerator ResetCanDealDamage(float waitTime)
            {
                yield return new WaitForSeconds(waitTime);
                canDealDamage = true;
            }
        }
    }