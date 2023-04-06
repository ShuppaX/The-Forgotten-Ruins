using UnityEngine;
using BananaSoup.Ability;

namespace BananaSoup.DamageSystem
{
    public class PlayerSword : Damager
    {
        private AbilityAttack playerAttack = null;

        private void Start()
        {
            playerAttack = PlayerBase.Instance.GetComponent<AbilityAttack>();
        }

        public override void OnTriggerEnter(Collider collision)
        {
            if ( !playerAttack.CanDealDamage )
            {
                return;
            }

            base.OnTriggerEnter(collision);
        }
    }
}
