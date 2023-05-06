using BananaSoup.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public class FireToggler : MonoBehaviour, IThrowReactable
    {
        [SerializeField] private ParticleSystem fireParticles;
        [SerializeField] private bool isFireBurningAtStart = true;
        [SerializeField] bool canDealDamageWhenBurning;
        private bool isBurning = true;
        private FireDamager damager;

        private void Start()
        {
            if ( fireParticles == null )
            {
                Debug.LogError(name + " is missing Fire ParticleSystem");
            }

            if ( canDealDamageWhenBurning )
            {
                // NOTE: This should be Damager instead of FireDamager but time is running out. 
                damager = GetComponent<FireDamager>();
                if ( damager == null )
                {
                    Debug.LogError(name + " is missing reference to a Damager!");
                }
            }

            if ( isFireBurningAtStart )
            {
                LitTorch();
            }
            else
            {
                Extinguish();
            }
        }

        public void LitTorch()
        {
            fireParticles.Play();
            isBurning = true;

            if ( canDealDamageWhenBurning )
            {
                ToggleDamager(true);
            }
        }

        public void Extinguish()
        {
            fireParticles.Stop();
            isBurning = false;

            if ( canDealDamageWhenBurning )
            {
                ToggleDamager(false);
            }
        }

        private void ToggleDamager(bool value)
        {
            damager.CanDealDamage = value;
        }

        public void OnThrowAbility(ParticleProjectile.Type projectileType)
        {
            switch ( projectileType )
            {
                case ParticleProjectile.Type.Sand:
                {
                    if ( !isBurning )
                    {
                        return;
                    }

                    Extinguish();

                    break;
                }
                case ParticleProjectile.Type.Spark:
                {
                    if ( isBurning )
                    {
                        return;
                    }

                    LitTorch();

                    break;
                }
            }
        }
    }
}
