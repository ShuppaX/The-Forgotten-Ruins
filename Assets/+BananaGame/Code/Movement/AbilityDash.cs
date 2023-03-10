using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    [RequireComponent(typeof(PlayerBase), typeof(PlayerStateManager))]
    public class AbilityDash : MonoBehaviour
    {
        public static AbilityDash Instance { get; private set; }

        [Header("Dash variables")]
        [SerializeField, Tooltip("The amount of force for dashing.")]
        private float dashForce = 5.0f;
        [SerializeField, Tooltip("The cooldown until dash can be used again.")]
        private float dashCooldown = 4.0f;
        [SerializeField, Tooltip("The duration of the dash.")]
        private float dashDuration = 0.25f;
        [SerializeField, Tooltip("Does dash stop like a collision to a wall or slowly decrease speed.")]
        private bool isLerpingDash = true;
        [SerializeField] float lerpSpeed = 25.0f;

        private Rigidbody rb;

        private bool dashOnCooldown = false;
        private Coroutine dashCooldownRoutine = null;

        private void Awake()
        {
            if ( Instance == null )
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

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
        }

        //TODO: Have the dash disable gravity for the duration of the dash and possibly
        //TODO: have the character rise a bit when dashing?
        //TODO: Also have the player "forced" to the ground while dashing.

        /// <summary>
        /// A dash movement for the player character. Allows the character to dash if
        /// dash isn't on cooldown.
        /// </summary>
        /// <param name="context">The players dash input.</param>
        public void OnDash(InputAction.CallbackContext context)
        {
            if ( PlayerBase.Instance.AreAbilitiesEnabled )
            {
                if ( !dashOnCooldown && context.phase == InputActionPhase.Performed )
                {
                    PlayerStateManager.Instance.playerState = PlayerStateManager.State.Dashing;
                    Vector3 forceToApply = transform.forward * dashForce;

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
            if ( isLerpingDash )
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, lerpSpeed * Time.deltaTime);

                Debug.Log("step: " + lerpSpeed);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

            PlayerStateManager.Instance.playerState = PlayerStateManager.State.Idle;
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
