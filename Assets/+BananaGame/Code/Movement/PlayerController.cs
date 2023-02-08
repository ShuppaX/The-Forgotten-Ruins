using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float movementForce = 5.0f;
        [SerializeField] private float maxSpeed = 5.0f;
        [SerializeField] private float turnSpeed = 360.0f;

        [Header("Dash")]
        [SerializeField] private float dashForce = 5.0f;
        [SerializeField] private float dashCooldown = 4.0f;
        [SerializeField] private float dashDuration = 0.25f;
        [SerializeField] private float dashMaxSpeed = 10.0f;

        private PlayerInput playerInput;

        private Rigidbody rb;
        private Vector3 movementInput = Vector3.zero;
        private Vector3 movementDirection = Vector3.zero;

        private bool dashOnCooldown = false;
        private Coroutine dashCooldownRoutine = null;

        private float characterWidth = 0;
        private float characterHeight = 0;

        private bool isDashing = false;
        private float currentMaxSpeed = 0;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            rb = GetComponent<Rigidbody>();
            playerInput = new PlayerInput();

            characterWidth = transform.localScale.x;
            characterHeight = transform.localScale.y;

            currentMaxSpeed = maxSpeed;

            if (rb == null)
            {
                Debug.LogError("A Rigidbody couldn't be found on the " + gameObject + "!");
            }
        }

        /// <summary>
        /// Stores the players input to moveInput and enables the playerInput if the gameObject is enabled.
        /// </summary>
        private void OnEnable()
        {
            playerInput.Player.Enable();
        }

        /// <summary>
        /// Disables the playerInput if the game object is disabled.
        /// </summary>
        private void OnDisable()
        {
            playerInput.Player.Disable();
        }

        private void FixedUpdate()
        {
            Move();
            Look(Time.fixedDeltaTime);
        }

        private void Update()
        {

        }

        /// <summary>
        /// Used to get the players input and then store it into the movementInput Vector3
        /// </summary>
        /// <param name="context">The players movement input.</param>
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            movementInput.Set(input.x, 0, input.y);
        }

        /// <summary>
        /// Moves the character in the direction of the players input.
        /// Uses the inputs magnitude to calculate correct movement for an isometric view.
        /// </summary>
        private void Move()
        {
            movementDirection += (transform.forward * movementInput.magnitude) * movementForce;

            rb.AddForce(movementDirection, ForceMode.Impulse);
            movementDirection = Vector3.zero;

            if (rb.velocity.magnitude > currentMaxSpeed)
            {
                rb.velocity = rb.velocity.normalized * currentMaxSpeed;
            }
        }

        /// <summary>
        /// Rotates the character towards the direction of movement.
        /// Also calculates the correct rotation for the character for an isometric view.
        /// Moving up rotates the character up instead of up and left.
        /// </summary>
        /// <param name="deltaTime">Use Time.fixedDeltaTime.</param>
        private void Look(float deltaTime)
        {
            if (movementInput != Vector3.zero)
            {
                var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

                var skewedInput = matrix.MultiplyPoint3x4(movementInput);

                var relative = (transform.position + skewedInput) - transform.position;
                var rot = Quaternion.LookRotation(relative, Vector3.up);

                // Use this for smooth turning
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * deltaTime);

                // Use this for instant turning
                //transform.rotation = rot;
            }
        }

        /// <summary>
        /// A dash movement for the player character. Allows the character to dash if
        /// dash isn't on cooldown.
        /// </summary>
        /// <param name="context">The players dash input.</param>
        public void OnDash(InputAction.CallbackContext context)
        {
            isDashing = true;
            Vector3 forceToApply = transform.forward * dashForce;

            if (!dashOnCooldown && context.phase == InputActionPhase.Performed)
            {
                currentMaxSpeed = dashMaxSpeed;
                rb.velocity = forceToApply;
                dashOnCooldown = true;

                if (dashCooldownRoutine == null)
                {
                    dashCooldownRoutine = StartCoroutine(nameof(DashCooldown));
                }

                Invoke(nameof(ResetDash), dashDuration);
            }
        }

        /// <summary>
        /// Method which resets the maxSpeed set for the dash and sets the isDashing bool to false.
        /// </summary>
        private void ResetDash()
        {
            isDashing = false;
            currentMaxSpeed = maxSpeed;
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

        public bool IsGrounded()
        {
            RaycastHit rayHit;
            return (Physics.SphereCast(transform.position, characterWidth / 2, Vector3.down, out rayHit, characterHeight / 2));
        }
    }
}
