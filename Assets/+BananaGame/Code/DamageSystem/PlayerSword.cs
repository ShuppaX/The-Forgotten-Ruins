using UnityEngine;
using BananaSoup.Ability;
using BananaSoup.PickupSystem;

namespace BananaSoup.DamageSystem
{
    public class PlayerSword : Damager
    {
        private AbilityAttack playerAttack = null;
        private MeshRenderer swordMesh;

        private void Start()
        {
            swordMesh = GetComponentInChildren<MeshRenderer>();
            if ( swordMesh == null )
            {
                Debug.LogError(name + " is missing a referece to a Sword Mesh!");
            }

            SetSwordVisibility();
            playerAttack = PlayerBase.Instance.GetComponent<AbilityAttack>();
            PickupSword.OnEventLooted += SetSwordVisibility;
        }

        private void OnDisable()
        {
            PickupSword.OnEventLooted -= SetSwordVisibility;
        }

        public override void OnTriggerStay(Collider collision)
        {
            if ( !playerAttack.CanDealDamage )
            {
                return;
            }

            base.OnTriggerStay(collision);
        }

        private void SetSwordVisibility()
        {
            swordMesh.enabled = PlayerBase.Instance.IsSwordLooted;
        }
    }
}
