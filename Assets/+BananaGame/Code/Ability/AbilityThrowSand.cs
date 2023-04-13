using BananaSoup.Managers;

namespace BananaSoup.Ability
{
    public class AbilityThrowSand : AbilityThrowBase
    {
        // Constant PlayerState used for PlayerState handling
        public const PlayerStateManager.PlayerState sanding = PlayerStateManager.PlayerState.Sanding;

        public override void Start()
        {
            base.Start();

            SetAbilityStateName = sanding;
        }

        // TODO: Change these not to use animation event
        // OnThrowSpark() is called from a throw animation of the player by an event.
        private void OnThrowSand()
        {
            base.OnThrow();
        }

        // OnSparkDone() is called from a throw animation of the player by an event.
        private void OnSandDone()
        {
            base.OnThrowDone();
        }
    }
}
