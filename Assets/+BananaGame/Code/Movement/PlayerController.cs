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
        [SerializeField, Tooltip("The amount of force for moving the character.")]
        private float movementForce = 5.0f;
        [SerializeField, Tooltip("Maximum movementspeed for the character.")]
        private float maxSpeed = 7.0f;
        [SerializeField, Tooltip("The amount of drag used while the character is on the ground.")]
        private float groundDrag = 3.5f;
        [SerializeField, Tooltip("The amount of drag used while the character is not on the ground.")]
        private float fallingDrag = 1.0f;

        // This is used to keep the player down on the ground while walking up on slopes.
        [SerializeField, Tooltip("The downward force on the character on slopes.")]
        private float downwardSlopeForce = 80.0f;

        // This is required for the smooth turning. (In the Look method)
        //[SerializeField, Tooltip("The turning speed of the character while changing direction.")] private float turnSpeed = 360.0f;

        // Variables used to store in script values.
        private Rigidbody rb;
        private Vector3 movementInput = Vector3.zero;
        private Vector3 movementDirection = Vector3.zero;

        private bool wasGrounded = false;
        [SerializeField]
        private float groundCheckOffset = 0.05f;

        private float characterWidth = 0;
        private float characterHeight = 0;

        [Header("Slope")]
        [SerializeField, Tooltip("The maximum angle for a slope the player can walk on.")]
        private float maxSlopeAngle = 70.0f;

        private RaycastHit slopeHit;

        private PlayerBase playerBase;

        // TODO: Decide whether to keep this or not (is there any use for it,
        // TODO: for example in animations etc.)
        public bool IsGrounded
        {
            get { return GroundCheck(); }
        }

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

        private void Update()
        {
            SpeedLimiter();

            Debug.Log("Ground check = " + GroundCheck());
            Debug.Log("RigidBodys velocity = " + rb.velocity);
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

            if ( OnSlope() )
            {
                rb.AddForce(GetSlopeMoveDirection() * movementForce);

                if ( rb.velocity.y > 0 )
                {
                    rb.AddForce(Vector3.down * downwardSlopeForce);
                }
            }

            if ( GroundCheck() )
            {
                rb.AddForce(movementDirection);
                movementDirection = Vector3.zero;
            }

            // Turn the RigidBodys
            rb.useGravity = !OnSlope();
        }

        private void SpeedLimiter()
        {
            // Limit characters movement speed on slopes
            if ( OnSlope() )
            {
                if (rb.velocity.magnitude > maxSpeed )
                {
                    rb.velocity = rb.velocity.normalized * maxSpeed;
                }
            }

            // Limiting the speed on the ground or air
            else
            {
                Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                // Limit the velocity if necessary.
                if (flatVel.magnitude > maxSpeed )
                {
                    Vector3 limitedVel = flatVel.normalized * maxSpeed;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }
        }

        /// <summary>
        /// Method used to set the drag to be lower while the character is not grounded
        /// allowing faster falling speed.
        /// </summary>
        /// <returns>True if the character was grounded on the previous frame, false if not.</returns>
        private bool SetDrag()
        {
            if ( GroundCheck() )
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
        private bool GroundCheck()
        {
            RaycastHit rayHit;
            return (Physics.SphereCast(transform.position, characterWidth / 2, Vector3.down, out rayHit, (characterHeight / 2) + groundCheckOffset));
        }

        private bool OnSlope()
        {
            if ( Physics.Raycast(transform.position, Vector3.down, out slopeHit, characterHeight * 0.5f + 0.3f) )
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        private Vector3 GetSlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;
        }
    }
}
