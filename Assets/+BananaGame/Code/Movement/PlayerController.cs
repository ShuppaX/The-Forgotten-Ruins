using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RotaryHeart.Lib.PhysicsExtension;

namespace BananaSoup
{
    [RequireComponent(typeof(PlayerBase))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Tooltip("The amount of force for moving the character.")]
        private float movementForce = 5.0f;
        [SerializeField, Tooltip("The amount of drag used while the character is on the ground.")]
        private float groundDrag = 3.5f;
        [SerializeField, Tooltip("The amount of drag used while the character is not on the ground.")]
        private float fallingDrag = 1.0f;

        // This is used to keep the player down on the ground while walking up on slopes.
        [SerializeField, Tooltip("The downward force on the character on slopes.")]
        private float downwardSlopeForce = 80.0f;

        // Variables used to store in script values.
        private Rigidbody rb;
        private Vector3 movementInput = Vector3.zero;
        private Vector3 movementDirection = Vector3.zero;

        private bool wasGrounded = false;

        [SerializeField]
        private float groundCheckOffset = 0.05f;

        [SerializeField]
        private float cameraAngle = 45.0f;

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
            //Debug.Log("Ground check = " + GroundCheck());
            //Debug.Log("Slope check = " + OnSlope());
            //Debug.Log("RigidBodys velocity = " + rb.velocity);
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
                Look();
            }
        }

        /// <summary>
        /// Used to get the players input and then store it into the movementInput Vector3
        /// </summary>
        /// <param name="context">The players movement input.</param>
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            movementInput = new Vector3(input.x, 0, input.y);
            movementDirection = IsoVectorConvert(movementInput);
        }

        /// <summary>
        /// Method used to convert the original movement vector to match the correct
        /// angles of movement for the isometric view. The method uses the cameras angle
        /// in a matrix.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>The converted movement vector.</returns>
        private Vector3 IsoVectorConvert(Vector3 vector)
        {
            Quaternion rotation = Quaternion.Euler(0, cameraAngle, 0);
            Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
            Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
            return result;
        }

        /// <summary>
        /// Moves the character in the direction of the players input.
        /// Uses the method above to translate the movementInput to fit the isometric
        /// view and then multiplies it by the movementForce.
        /// </summary>
        private void Move()
        {
            if ( OnSlope() )
            {
                rb.AddForce(GetSlopeMoveDirection() * movementForce, ForceMode.Force);

                if ( rb.velocity.y > 0 )
                {
                    rb.AddForce(Vector3.down * downwardSlopeForce, ForceMode.Force);
                }
            }
            else if ( GroundCheck() )
            {
                rb.AddForce(movementDirection * movementForce, ForceMode.Force);
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
        /// The correct direction is calculated using IsoVectorConvert method.
        /// Moving up rotates the character up instead of up and left.
        /// </summary>
        private void Look()
        {
            if ( movementInput != Vector3.zero )
            {
                var rot = Quaternion.LookRotation(IsoVectorConvert(movementInput), Vector3.up);

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
            //return (RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(transform.position, Vector3.down, (characterHeight / 2) + groundCheckOffset, PreviewCondition.Editor, 0, Color.green, Color.red));
            return UnityEngine.Physics.Raycast(transform.position, Vector3.down, (characterHeight / 2) + groundCheckOffset);

        }

        private bool OnSlope()
        {
            if ( RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(transform.position, Vector3.down, out slopeHit, (characterHeight / 2) + groundCheckOffset, PreviewCondition.Editor, 0, Color.green, Color.red) )
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                bool angleNotZero = (angle != 0);
                bool angleLessThanMaxSlopeAngle = (angle < maxSlopeAngle);

                if ( angleLessThanMaxSlopeAngle && angleNotZero )
                {
                    return true;
                }
            }

            return false;
        }

        private Vector3 GetSlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;

            //Vector3 directionRight = Vector3.Cross(movementDirection, Vector3.up);
            //return Vector3.Cross(slopeHit.normal, directionRight).normalized;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(slopeHit.point, GetSlopeMoveDirection());
        }
    }
}
