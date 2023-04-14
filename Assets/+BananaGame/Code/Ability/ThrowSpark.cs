using BananaSoup.Managers;

namespace BananaSoup.Ability
{
    public class ThrowSpark: ThrowBase
    {
        // Constant PlayerState used for PlayerState handling
        public const PlayerStateManager.PlayerState sparking = PlayerStateManager.PlayerState.Sparking;

        public override void Start()
        {
            base.Start();

            SetAbilityStateName = sparking;
        }
    }
}
