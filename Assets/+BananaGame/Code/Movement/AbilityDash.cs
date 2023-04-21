using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using BananaSoup.Managers;
using BananaSoup.Utilities;
using Unity.VisualScripting;

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

        private float dashCooldownCounter = 0.0f;

        private bool dashOnCooldown = false;
        private bool slopeCheckChanged = false;

        private Coroutine dashCooldownRoutine = null;

        [Header("Constant PlayerStates used for PlayerState handling")]
        public const PlayerStateManager.PlayerState dashing = PlayerStateManager.PlayerState.Dashing;
        public const PlayerStateManager.PlayerState dashOverInAir = PlayerStateManager.PlayerState.InAir;

        // Reference to players Rigidbody
        private Rigidbody rb = null;
        private CalculateMovementDirection directionCalculator = null;
        private SlopeCheck slopeCheck = null;
        private GroundCheck groundCheck = null;
        private PlayerStateManager psm = null;

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

            Debug.Log("dashCooldownLeft: " + dashCooldownCounter);
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
                    PlayerBase.Instance.IsMovable = false;
                    PlayerBase.Instance.IsTurnable = false;
                    PlayerBase.Instance.IsInteractingEnabled = false;
                    PlayerBase.Instance.AreAbilitiesEnabled = false;
                    PlayerBase.Instance.CanDash = false;
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

            PlayerBase.Instance.IsMovable = true;
            PlayerBase.Instance.IsTurnable = true;
            PlayerBase.Instance.IsInteractingEnabled = true;
            PlayerBase.Instance.AreAbilitiesEnabled = true;
            PlayerBase.Instance.CanDash = true;
        }

        /// <summary>
        /// IEnumerator to enable a cooldown for the player characters dash.
        /// Sets the stored routine to be null and the cooldown bool to false after the cooldown time
        /// has passed.
        /// </summary>
        private IEnumerator DashCooldown()
        {
            //yield return new WaitForSeconds(dashCooldown);
            //dashCooldownRoutine = null;
            //dashOnCooldown = false;
            dashCooldownCounter = 0.0f;
            float dashCooldownStartTime = Time.time;

            while ( dashCooldownStartTime < dashCooldownStartTime + dashCooldown )
            {
                if ( dashCooldownCounter < dashCooldown )
                {
                    dashCooldownCounter += Time.deltaTime;
                }
                dashCooldownStartTime = Time.time;
            }

            if ( dashCooldownCounter >= dashCooldown )
            {
                dashCooldownRoutine = null;
                dashOnCooldown = false;
                yield break;
            }
        }
    }
}
