using UnityEngine;

namespace BananaSoup
{
    public class PlayerSword : Damager
    {
        public override void OnTriggerEnter(Collider collision)
        {
            if ( PlayerStateManager.Instance.currentPlayerState != PlayerStateManager.PlayerState.Attacking )
            {
                return;
            }

            base.OnTriggerEnter(collision);
        }
    }
}
