using BananaSoup.Managers;
using BananaSoup.PickupSystem;

namespace BananaSoup.Ability
{
    public class ThrowSand : ThrowBase
    {
        // Constant PlayerState used for PlayerState handling
        public const PlayerStateManager.PlayerState sanding = PlayerStateManager.PlayerState.Sanding;

        public override void Start()
        {
            base.Start();

            SetAbilityStateName = sanding;

            PickupSand.OnEventLooted += SetAbility;
        }

        private void SetAbility()
        {
            abilityThrow.ToggleAbilityUsability(this);
        }
    }
}
