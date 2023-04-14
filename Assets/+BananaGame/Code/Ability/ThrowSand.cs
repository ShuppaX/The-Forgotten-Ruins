using BananaSoup.Managers;

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
        }
    }
}
