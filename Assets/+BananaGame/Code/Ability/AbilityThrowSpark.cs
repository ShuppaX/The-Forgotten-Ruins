using BananaSoup.Managers;

namespace BananaSoup.Ability
{
    public class AbilityThrowSpark: AbilityThrowBase
    {
        // Constant PlayerState used for PlayerState handling
        public const PlayerStateManager.PlayerState sparking = PlayerStateManager.PlayerState.Sparking;

        public override void Start()
        {
            base.Start();

            SetAbilityStateName = sparking;
        }

        // TODO: Change these not to use animation event
        // OnThrow() is called from a throw animation of the player by an event.
        private void OnThrowSpark()
        {
            base.OnThrow();
        }

        // OnThrowDone() is called from a throw animation of the player by an event.
        private void OnSparkDone()
        {
            base.OnThrowDone();
        }
    }
}
