using BananaSoup.Managers;
using BananaSoup.PickupSystem;

namespace BananaSoup.Ability
{
    public class ThrowSpark : ThrowBase
    {
        // Constant PlayerState used for PlayerState handling
        public const PlayerStateManager.PlayerState sparking = PlayerStateManager.PlayerState.Sparking;

        private void OnDisable()
        {
            PickupSpark.OnEventLooted -= SetAbility;
        }

        public override void Start()
        {
            base.Start();

            SetAbilityStateName = sparking;

            PickupSpark.OnEventLooted += SetAbility;
        }

        private void SetAbility()
        {
            abilityThrow.ToggleAbilityUsability(this);
        }
    }
}
