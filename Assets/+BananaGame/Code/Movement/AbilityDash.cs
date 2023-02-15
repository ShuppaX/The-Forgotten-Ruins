using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    [RequireComponent(typeof(PlayerBase))]
    public class AbilityDash : MonoBehaviour
    {
        [Header("Dash variables")]
        [SerializeField, Tooltip("The amount of force for dashing.")] private float dashForce = 5.0f;
        [SerializeField, Tooltip("The cooldown until dash can be used again.")] private float dashCooldown = 4.0f;
        [SerializeField, Tooltip("The duration of the dash.")] private float dashDuration = 0.25f;

        private Rigidbody rb;

        private bool dashOnCooldown = false;
        private Coroutine dashCooldownRoutine = null;

        // The bool isDashing is not currently in use
        // TODO: Make use for isDashing or remove it.
        private bool isDashing = false;

        private PlayerBase playerBase;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            rb = GetComponent<Rigidbody>();

            if ( rb == null )
            {
                Debug.LogError("The dash ability couldn't find a Rigidbody on the gameObject: " + gameObject + "!");
            }

            playerBase = GetComponent<PlayerBase>();
            if ( playerBase == null )
            {
                Debug.LogError("A PlayerBase couldn't be found on the " + gameObject + "!");
            }
        }

        /// <summary>
        /// A dash movement for the player character. Allows the character to dash if
        /// dash isn't on cooldown.
        /// </summary>
        /// <param name="context">The players dash input.</param>
        public void OnDash(InputAction.CallbackContext context)
        {
            if ( playerBase.IsControllable )
            {
                isDashing = true;
                Vector3 forceToApply = transform.forward * dashForce;

                if ( !dashOnCooldown && context.phase == InputActionPhase.Performed )
                {
                    rb.velocity = forceToApply;
                    dashOnCooldown = true;

                    if ( dashCooldownRoutine == null )
                    {
                        dashCooldownRoutine = StartCoroutine(nameof(DashCooldown));
                    }

                    Invoke(nameof(ResetDash), dashDuration);
                }
            }
        }

        /// <summary>
        /// Method which resets the maxSpeed set for the dash and sets the isDashing bool to false.
        /// </summary>
        private void ResetDash()
        {
            isDashing = false;
        }

        /// <summary>
        /// IEnumerator to enable a cooldown for the player characters dash.
        /// Sets the stored routine to be null and the cooldown bool to false after the cooldown time
        /// has passed.
        /// </summary>
        private IEnumerator DashCooldown()
        {
            yield return new WaitForSeconds(dashCooldown);
            dashCooldownRoutine = null;
            dashOnCooldown = false;
        }
    }
}
