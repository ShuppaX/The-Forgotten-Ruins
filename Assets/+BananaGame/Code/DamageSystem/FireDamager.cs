using BananaSoup.Ability;
using BananaSoup.PickupSystem;
using System;
using UnityEngine;

namespace BananaSoup.DamageSystem
{
    public class FireDamager : Damager
    {
        private bool canDealDamage = true;

        public bool CanDealDamage
        {
            get { return canDealDamage; }
            set { canDealDamage = value; }
        }

        public override void OnTriggerStay(Collider other)
        {
            if ( !canDealDamage )
            {
                return;
            }

            base.OnTriggerStay(other);
        }
    }
}
