using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using BananaSoup.Managers;
using BananaSoup.Utilities;
using System;

namespace BananaSoup.Ability
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
        [SerializeField]
        private float lerpSpeed = 25.0f;

        private float remainingCooldown = 0.0f;
        private float roundedRemainingCooldown = 0.0f;

        private bool dashOnCooldown = false;
        private bool slopeCheckChanged = false;

        private Coroutine dashCooldownRoutine = null;

        [Header("Constant PlayerStates used for PlayerState handling")]
        private const PlayerStateManager.PlayerState dashing = PlayerStateManager.PlayerState.Dashing;
        private const PlayerStateManager.PlayerState dashOverInAir = PlayerStateManager.PlayerState.InAir;
        private const PlayerStateManager.PlayerState dead = PlayerStateManager.PlayerState.Dead;

        // References
        private Rigidbody rb = null;
        private CalculateMovementDirection directionCalculator = null;
        private SlopeCheck slopeCheck = null;
        private GroundCheck groundCheck = null;
        private PlayerStateManager psm = null;

        // Unity Action used to update UI
        public static event Action DashEventAction;

        public float DashCooldown
        {
            get => dashCooldown;
        }

        public float RoundedRemainingCooldown
        {
            get => roundedRemainingCooldown;
        }

        private void OnDisable()
        {
            if ( dashCooldownRoutine != null )
            {
                StopCoroutine(dashCooldownRoutine);
                dashCooldownRoutine = null;
            }
        }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            GetInstances();

            rb = GetDependency<Rigidbody>();
            directionCalculator = GetDependency<CalculateMovementDirection>();
            groundCheck = GetDependency<GroundCheck>();
            slopeCheck = GetDependency<SlopeCheck>();
        }

        /// <summary>
        /// Method to get references of existing Instances and to throw an error if it is null.
        /// </summary>
        private void GetInstances()
        {
            psm = PlayerStateManager.Instance;
            if ( psm == null )
            {
                Debug.LogError(gameObject.name + " couldn't find an Instance of PlayerStateManager!");
            }
        }

        /// <summary>
        /// Method to simplify getting components and to throw an error if it's null
        /// this improves readability.
        /// </summary>
        /// <typeparam name="T">The name of the component to get.</typeparam>
        /// <returns>The wanted component if it's found.</returns>
        private T GetDependency<T>() where T : Component
        {
            T component = GetComponent<T>();
            if ( component == null )
            {
                Debug.LogError($"The component of type {typeof(T).Name} couldn't be found on the " + gameObject.name + "!");
            }

            return component;
        }

        private void Update()
        {
            OnSlopeCheckValueChanged();
        }

        /// <summary>
        /// Method to check if the value of slopeChecks OnSlope() has changed.
        /// If it has invoke the UpdateDash method to correct the dash direction, so that
        /// the player stays on the ground.
        /// </summary>
        private void OnSlopeCheckValueChanged()
        {
            if ( slopeCheckChanged != slopeCheck.IsOnSlope )
            {
                slopeCheckChanged = !slopeCheckChanged;
                UpdateRemainingVelocityDirection();
            }
        }

        /// <summary>
        /// Method used to update the direction of the remaining velocity of a dash.
        /// Invoked if slopeCheck.OnSlope() value changes.
        /// Stores the remaining velocity as a Vector3, sets the current rb.velocity to
        /// zero and then sets the velocity to the remainingVelocity with corrected direction.
        /// </summary>
        private void UpdateRemainingVelocityDirection()
        {
            Vector3 remainingVelocity = rb.velocity;
            rb.velocity = Vector3.zero;
            rb.velocity = GetCalculatedDirection(remainingVelocity);
        }

        /// <summary>
        /// A dash movement for the player character. Allows the character to dash if
        /// dash isn't on cooldown.
        /// </summary>
        /// <param name="context">The players dash InputAction.</param>
        public void OnDash(InputAction.CallbackContext context)
        {
            if ( !PlayerBase.Instance.IsDashLooted )
            {
                return;
            }

            if ( PlayerBase.Instance.CanDash )
            {
                if ( !dashOnCooldown && context.phase == InputActionPhase.Performed )
                {
                    psm.SetPlayerState(dashing);
                    
                    if ( DashEventAction != null )
                    {
                        DashEventAction();
                    }

                    PlayerBase.Instance.ToggleAllActions(false);

                    Vector3 forceToApply = GetCalculatedDirection(transform.forward) * dashForce;

                    rb.velocity = forceToApply;

                    dashOnCooldown = true;

                    if ( dashCooldownRoutine == null )
                    {
                        dashCooldownRoutine = StartCoroutine(nameof(DashCooldownRoutine));
                    }

                    Invoke(nameof(DashReset), dashDuration);
                }
            }
        }

        /// <summary>
        /// Method used to get calculated corrected direction depending on if the player
        /// is on a slope or on even ground.
        /// </summary>
        /// <param name="direction">The current direction of movement. (transform.forward for dash)</param>
        /// <returns></returns>
        private Vector3 GetCalculatedDirection(Vector3 direction)
        {
            return directionCalculator.CalculateDirection(direction);
        }

        /// <summary>
        /// Method which resets the players velocity to zero.
        /// Toggleable lerp for the velocity reset.
        /// Also used to invoke statechange after Dash is reset.
        /// </summary>
        private void DashReset()
        {
            if ( psm.CurrentPlayerState == dead )
            {
                return;
            }

            if ( isLerpingDash )
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, lerpSpeed * Time.deltaTime);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

            if ( groundCheck.IsGrounded )
            {
                psm.ResetPlayerState();
            }
            else if ( !groundCheck.IsGrounded )
            {
                psm.SetPlayerState(dashOverInAir);
            }

            PlayerBase.Instance.ToggleAllActions(true);
        }

        /// <summary>
        /// IEnumerator which calculates the remaining cooldown based on the start time
        /// of the Coroutine and current Time.time.
        /// Rounds the remainingCooldown value to two decimals and then after the
        /// cooldown is over nulls the dashCooldownRoutine, turns dashOnCooldown false
        /// and sets remainingCooldown to 0.0f.
        /// </summary>
        private IEnumerator DashCooldownRoutine()
        {
            remainingCooldown = dashCooldown;
            float startTime = Time.time;

            while ( remainingCooldown > 0.0f )
            {
                yield return null; // Wait for next frame
                remainingCooldown = dashCooldown - (Time.time - startTime);

                // Round the remainingCooldown to two decimals.
                roundedRemainingCooldown = Mathf.Round(remainingCooldown * 100f) / 100f;
            }

            if ( remainingCooldown <= 0.0f )
            {
                dashCooldownRoutine = null;
                dashOnCooldown = false;
                remainingCooldown = 0.0f;

                if ( DashEventAction != null )
                {
                    DashEventAction();
                }
            }
        }
    }
}
