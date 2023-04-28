using UnityEngine;
using UnityEngine.InputSystem;
using BananaSoup.Managers;

namespace BananaSoup.Ability
{
    public class AbilityAttack : MonoBehaviour
    {
        [SerializeField, Tooltip("The time in animation when the damage should be disabled.")]
        private float timeToDisableDamage = 0.3f;
        [SerializeField, Tooltip("The time in animation when the attack should be over.")]
        private float timeToAttackOver = 0.65f;
        private bool canDealDamage = false;

        // References
        private PlayerBase playerBase = null;
        private PlayerStateManager psm = null;

        [Header("Constant PlayerState for PlayerState handling")]
        private const PlayerStateManager.PlayerState attacking = PlayerStateManager.PlayerState.Attacking;

        public bool CanDealDamage
        {
            get { return canDealDamage; }
        }

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
            if ( !playerBase.IsSwordLooted )
            {
                return;
            }

            if ( !playerBase.AreAbilitiesEnabled )
            {
                return;
            }

            if ( context.performed )
            {
                canDealDamage = true;
                psm.SetPlayerState(attacking);
                ToggleActions(false);

                Invoke(nameof(DisableDamage), timeToDisableDamage);
                Invoke(nameof(AttackOver), timeToAttackOver);
            }
        }

        /// <summary>
        /// AttackOver is called with an Invoke in OnAttack after timeToAttackOvere
        /// float value.
        /// </summary>
        private void AttackOver()
        {
            psm.ResetPlayerState();
            ToggleActions(true);
        }

        /// <summary>
        /// Method used to disable the damage from the players attack.
        /// Called from OnAttack with an invoke after timeToDisableDamage.
        /// </summary>
        private void DisableDamage()
        {
            canDealDamage = false;
        }

        /// <summary>
        /// Method used to toggle the playerBase bools which are used to disable/enable
        /// actions during other actions.
        /// </summary>
        /// <param name="value">True to set true, false to set false.</param>
        private void ToggleActions(bool value)
        {
            playerBase.IsMovable = value;
            playerBase.IsTurnable = value;
            playerBase.CanDash = value;
            playerBase.AreAbilitiesEnabled = value;
        }
    }
}
