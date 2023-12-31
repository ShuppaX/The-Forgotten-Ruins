using System.Collections;
using UnityEngine;
using BananaSoup.DamageSystem;

namespace BananaSoup
{
    public class EnemyMeleeDamage : Damager
    {
        [SerializeField] private bool canDealDamage = false;
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
            canDealDamage = true;

        }

        public override void OnTriggerStay(Collider collision)
        {
            if (!canDealDamage) return;

            base.OnTriggerStay(collision);

        }

        public IEnumerator ResetCanDealDamage(float waitTime)
            {
                yield return new WaitForSeconds(waitTime);
                canDealDamage = false;

            }
        }
    }