using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace BananaSoup
{
    [RequireComponent(typeof(PlayerBase), typeof(PlayerStateManager), typeof(CalculateMovementDirection))]
    [RequireComponent(typeof(SlopeCheck))]
    public class AbilityDash : MonoBehaviour
    {
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

        private bool dashOnCooldown = false;
        private bool slopeCheckChanged = false;

        private Coroutine dashCooldownRoutine = null;

        [Header("UnityActions used to manage PlayerStates")]
        public UnityAction onDashAction;
        public UnityAction onDashReset;

        // Reference to players Rigidbody
        private Rigidbody rb = null;
        private CalculateMovementDirection directionCalculator = null;
        private SlopeCheck slopeCheck = null;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            rb = GetComponent<Rigidbody>();
            if ( rb == null )
            {
                Debug.LogError("A Rigidbody component couldn't be found on the " + gameObject.name + "!");
            }

            directionCalculator = GetComponent<CalculateMovementDirection>();
            if ( directionCalculator == null )
            {
                Debug.LogError("A CalculateMovementDirection component couldn't be found on the " + gameObject.name + "!");
            }

            slopeCheck = GetComponent<SlopeCheck>();
            if ( slopeCheck == null )
            {
                Debug.LogError("A SlopeCheck component couldn't be found on the " + gameObject.name + "!");
            }
        }

        private void Update()
        {
            //OnSlopeCheckValueChanged();
        }

        private void OnSlopeCheckValueChanged()
        {
            if ( slopeCheckChanged != slopeCheck.OnSlope() )
            {
                slopeCheckChanged = !slopeCheckChanged;
                UpdateDash();
            }
        }

        private void UpdateDash()
        {
            Vector3 remainingVelocity = rb.velocity;
            GetCalculatedDirection(remainingVelocity);
            rb.velocity = remainingVelocity;
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
                    onDashAction.Invoke();
                    Vector3 forceToApply = GetCalculatedDirection(transform.forward) * dashForce;

                    rb.velocity = forceToApply;
                    dashOnCooldown = true;

                    if ( dashCooldownRoutine == null )
                    {
                        dashCooldownRoutine = StartCoroutine(nameof(DashCooldown));
                    }

                    Invoke(nameof(DashReset), dashDuration);
                }
            }
        }

        private Vector3 GetCalculatedDirection(Vector3 direction)
        {
            return directionCalculator.CalculateDirection(direction);
        }

        /// <summary>
        /// Method which resets the maxSpeed set for the dash and sets the isDashing bool to false.
        /// </summary>
        private void DashReset()
        {
            if ( isLerpingDash )
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, lerpSpeed * Time.deltaTime);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

            onDashReset.Invoke();
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
