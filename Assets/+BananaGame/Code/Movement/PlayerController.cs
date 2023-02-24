using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    [RequireComponent(typeof(PlayerBase))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Tooltip("The amount of force for moving the character.")] private float movementForce = 5.0f;
        [SerializeField, Tooltip("The amount of drag used while the character is on the ground.")] private float groundDrag = 3.5f;
        [SerializeField, Tooltip("The amount of drag used while the character is not on the ground.")] private float fallingDrag = 1.0f;

        // This is required for the smooth turning. (In the Look method)
        //[SerializeField, Tooltip("The turning speed of the character while changing direction.")] private float turnSpeed = 360.0f;

        private Rigidbody rb;
        private Vector3 movementInput = Vector3.zero;
        private Vector3 movementDirection = Vector3.zero;

        private bool wasGrounded = false;
        private float groundCheckOffset = 0.01f;

        private float characterWidth = 0;
        private float characterHeight = 0;

        private PlayerBase playerBase;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            characterWidth = transform.localScale.x;
            characterHeight = transform.localScale.y;

            rb = GetComponent<Rigidbody>();
            if ( rb == null )
            {
                Debug.LogError("A Rigidbody couldn't be found on the " + gameObject + "!");
            }

            playerBase = GetComponent<PlayerBase>();
            if ( playerBase == null )
            {
                Debug.LogError("A PlayerBase couldn't be found on the " + gameObject + "!");
            }
        }

        private void FixedUpdate()
        {
            if ( playerBase.IsMovable )
            {
                if ( SetDrag() )
                {
                    Move();
                }
            }

            if ( playerBase.IsTurnable )
            {
                Look(Time.fixedDeltaTime);
            }
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

            if ( IsGrounded() )
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
            if ( IsGrounded() )
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
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * deltaTime);

                // Use this for instant turning
                transform.rotation = rot;
            }
        }

        /// <summary>
        /// Uses a spherecast to check if the character has something to collide with below it.
        /// </summary>
        /// <returns>True if the spherecast finds something below, false if not.</returns>
        public bool IsGrounded()
        {
            RaycastHit rayHit;
            return (Physics.SphereCast(transform.position, characterWidth / 2, Vector3.down, out rayHit, (characterHeight / 2) + groundCheckOffset));
        }
    }
}
