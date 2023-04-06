using UnityEngine;
using UnityEngine.InputSystem;
using BananaSoup.Managers;

namespace BananaSoup.Ability
{
    public class AbilityAttack : MonoBehaviour
    {
        private bool canDealDamage = false;

        public bool CanDealDamage
        {
            get { return canDealDamage; }
        }

        private PlayerBase playerBase = null;
        private PlayerStateManager psm = null;

        [Header("Constant PlayerState for PlayerState handling")]
        private const PlayerStateManager.PlayerState attacking = PlayerStateManager.PlayerState.Attacking;

        // Start is called before the first frame update
        private void Start()
        {
            GetInstances();
        }

        private void GetInstances()
        {
            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError(gameObject.name + " couldn't find an Instance of PlayerBase!");
            }

            psm = PlayerStateManager.Instance;
            if ( psm == null )
            {
                Debug.LogError(gameObject.name + " couldn't find an Instance of PlayerStateManager!");
            }
        }

        /// <summary>
        /// Method called on Attack input action. Checks if abilities are enabled, if not returns.
        /// Sets playerState to attacking and then toggles other actions false / nonusable.
        /// </summary>
        /// <param name="context"></param>
        public void OnAttack(InputAction.CallbackContext context)
        {
            if ( !playerBase.AreAbilitiesEnabled )
            {
                return;
            }

            if ( context.performed )
            {
                canDealDamage = true;
                psm.SetPlayerState(attacking);
                playerBase.IsMovable = false;
                playerBase.IsTurnable = false;
                playerBase.CanDash = false;
                playerBase.AreAbilitiesEnabled = false;
            }
        }

        /// <summary>
        /// OnAttackOver() is called from Fennec@Attack animation with an animation event.
        /// </summary>
        private void OnAttackOver()
        {
            psm.ResetPlayerState();
            playerBase.IsMovable = true;
            playerBase.IsTurnable = true;
            playerBase.CanDash = true;
            playerBase.AreAbilitiesEnabled = true;
        }

        /// <summary>
        /// Method used to disable the damage from the players attack.
        /// </summary>
        private void DisableDamage()
        {
            canDealDamage = false;
        }
    }
}
