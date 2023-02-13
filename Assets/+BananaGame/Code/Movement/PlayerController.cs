using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class PlayerController : PlayerBase
    {
        [Header("Movement")]
        [SerializeField, Tooltip("The amount of force for moving the character.")] private float movementForce = 5.0f;
        [SerializeField, Tooltip("The amount of drag used while the character is on the ground.")] private float groundDrag = 3.5f;
        [SerializeField, Tooltip("The amount of drag used while the character is not on the ground.")] private float fallingDrag = 1.0f;
        [SerializeField, Tooltip("The turning speed of the character while changing direction.")] private float turnSpeed = 360.0f;

        private Rigidbody rb;
        private Vector3 movementInput = Vector3.zero;
        private Vector3 movementDirection = Vector3.zero;

        private bool wasGrounded = false;

        private float characterWidth = 0;
        private float characterHeight = 0;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            rb = GetComponent<Rigidbody>();

            // NOTE: Moved to PlayerBase
            // playerInput = new PlayerInput();

            characterWidth = transform.localScale.x;
            characterHeight = transform.localScale.y;

            if (rb == null)
            {
                Debug.LogError("A Rigidbody couldn't be found on the " + gameObject + "!");
            }
        }

        // NOTE: Moved to PlayerBase
        ///// <summary>
        ///// Stores the players input to moveInput and enables the playerInput if the gameObject is enabled.
        ///// </summary>
        //private void OnEnable()
        //{
        //    playerInput.Player.Enable();
        //}

        // NOTE: Moved to PlayerBase
        ///// <summary>
        ///// Disables the playerInput if the game object is disabled.
        ///// </summary>
        //private void OnDisable()
        //{
        //    playerInput.Player.Disable();
        //}

        private void FixedUpdate()
        {
            if (SetDrag())
            {
                Move();
            }

            Look(Time.fixedDeltaTime);
        }

        private void Update()
        {
            //Debug.Log("The objects current rb velocity is: " + rb.velocity);
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

            if (IsGrounded())
            {
                rb.AddForce(movementDirection);
                movementDirection = Vector3.zero;
            }
        }

        /// <summary>
        /// Method used to set the drag to be lower while the character is not grounded
        /// allowing faster falling speed.
        /// </summary>
        /// <returns>True if the character was grounded on the previous frame, false if not.</returns>
        private bool SetDrag()
        {
            if (IsGrounded())
            {
                rb.drag = groundDrag;
                wasGrounded = true;
            }
            else
            {
                rb.drag = fallingDrag;
                wasGrounded = false;
            }

            return wasGrounded;
        }

        /// <summary>
        /// Rotates the character towards the direction of movement.
        /// Also calculates the correct rotation for the character for an isometric view.
        /// Moving up rotates the character up instead of up and left.
        /// </summary>
        /// <param name="deltaTime">Use Time.fixedDeltaTime.</param>
        private void Look(float deltaTime)
        {
            if ( movementInput != Vector3.zero )
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
        /// Uses a spherecast to check if the character has something to collide with below it.
        /// </summary>
        /// <returns>True if the spherecast finds something below, false if not.</returns>
        public bool IsGrounded()
        {
            RaycastHit rayHit;
            return (Physics.SphereCast(transform.position, characterWidth / 2, Vector3.down, out rayHit, characterHeight / 2));
        }
    }
}
