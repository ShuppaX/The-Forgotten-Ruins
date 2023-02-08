using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Adjustable variables")]
        [SerializeField] private float movementSpeed = 5.0f;
        [SerializeField] private float turnSpeed = 360.0f;
        [SerializeField] private float dashForce = 5.0f;
        [SerializeField] private float dashCooldown = 4.0f;

        private PlayerInput playerInput;
        private InputAction moveInput;

        private Rigidbody rb;
        private Vector3 movementInput = Vector3.zero;

        private bool dashOnCooldown = false;
        private Coroutine dashCooldownRoutine = null;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            rb = GetComponent<Rigidbody>();
            playerInput = new PlayerInput();

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
            moveInput = playerInput.Player.Move;
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
            Move(Time.fixedDeltaTime);
            Look(Time.fixedDeltaTime);
        }

        // Store the moveInput values to the movementInput Vector3
        private void Update()
        {
            movementInput.Set(moveInput.ReadValue<Vector2>().x, rb.velocity.y, moveInput.ReadValue<Vector2>().y);
        }

        /// <summary>
        /// Moves the character in the direction of the players input.
        /// Uses the inputs magnitude to calculate correct movement for an isometric view.
        /// </summary>
        /// <param name="deltaTime">Use Time.fixedDeltaTime.</param>
        private void Move(float deltaTime)
        {
            Vector3 movement = (transform.forward * movementInput.magnitude) * movementSpeed * deltaTime;
            rb.MovePosition(transform.position + movement);
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
        public void Dash(InputAction.CallbackContext context)
        {
            Vector3 forceToApply = transform.forward * dashForce;

            if (!dashOnCooldown && context.phase == InputActionPhase.Performed)
            {
                rb.AddForce(forceToApply, ForceMode.Impulse);
                dashOnCooldown = true;
                
                if (dashCooldownRoutine == null)
                {
                    dashCooldownRoutine = StartCoroutine(DashCooldown());
                }
            }
        }

        /// <summary>
        /// IEnumerator to enable a cooldown for the player characters dash.
        /// Sets the stored routine to be null and the cooldown bool to false after the cooldown time
        /// has passed.
        /// </summary>
        private IEnumerator DashCooldown()
        {
            Debug.Log("Dash on cooldown!");
            yield return new WaitForSeconds(dashCooldown);
            dashCooldownRoutine = null;
            dashOnCooldown = false;
            Debug.Log("Dash off cooldown!");
        }
    }
}
