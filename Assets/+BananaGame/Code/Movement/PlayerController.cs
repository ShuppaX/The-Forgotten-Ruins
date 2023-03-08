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

        [SerializeField, Tooltip("The games camera angle. (Used to calculate correct movement directions.)")]
        private float cameraAngle = 45.0f;

        [Header("GroundCheck variables")]
        [SerializeField]
        private float groundCheckOffset = 0.05f;

        bool[] raycasts = new bool[4];

        private float groundCheckRayLength = 0;

        private Vector3 negPositionX;
        private Vector3 posPositionX;
        private Vector3 negPositionZ;
        private Vector3 posPositionZ;

        private float groundCheckRayNegativeOffset = -1.0f;
        private float groundCheckRayPositiveOffset = 1.0f;

        private bool wasGrounded = false;

        [Header("Slope variables")]
        [SerializeField, Tooltip("The maximum angle for a slope the player can walk on.")]
        private float maxSlopeAngle = 70.0f;

        private RaycastHit slopeHit;

        // Variables used to store in script values and references.
        private Rigidbody rb;
        private PlayerBase playerBase;

        private Vector3 movementInput = Vector3.zero;
        private Vector3 movementDirection = Vector3.zero;

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
            groundCheckRayLength = (transform.localScale.y / 2) + groundCheckOffset;

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
            movementInput.Set(input.x, 0, input.y);
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
        /// Uses four separate raycasts to track if the player is standing on something or not.
        /// </summary>
        /// <returns>True if any of the rays is true, false if not.</returns>
        private bool GroundCheck()
        {
            CalculateGroundCheckRayStartPoints();

            raycasts[0] = UnityEngine.Physics.Raycast(negPositionX, Vector3.down, groundCheckRayLength);
            raycasts[1] = UnityEngine.Physics.Raycast(posPositionX, Vector3.down, groundCheckRayLength);
            raycasts[2] = UnityEngine.Physics.Raycast(negPositionZ, Vector3.down, groundCheckRayLength);
            raycasts[3] = UnityEngine.Physics.Raycast(posPositionZ, Vector3.down, groundCheckRayLength);

            foreach ( bool ray in raycasts )
            {
                if ( ray )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This method calculates the starting points for the four rays used for GroundCheck.
        /// </summary>
        private void CalculateGroundCheckRayStartPoints()
        {
            negPositionX.Set(transform.position.x - groundCheckRayNegativeOffset, transform.position.y, transform.position.z);
            posPositionX.Set(transform.position.x + groundCheckRayPositiveOffset, transform.position.y, transform.position.z);
            negPositionZ.Set(transform.position.x, transform.position.y, transform.position.z - groundCheckRayNegativeOffset);
            posPositionZ.Set(transform.position.x, transform.position.y, transform.position.z + groundCheckRayPositiveOffset);
        }

        /// <summary>
        /// A method used to track if the player is on a slope or not. It uses a raycast
        /// and calculates the angle between Vector3.up and the objects that the raycast hits normal.
        /// </summary>
        /// <returns>True if the angle between the Vector3.up and hit objects normal is less than the
        /// maximum allowed slope angle and if the angle is not zero.</returns>
        private bool OnSlope()
        {
            // RotaryHeart debug for this Slope Raycast
            // RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(transform.position, Vector3.down, out slopeHit, (characterHeight / 2) + groundCheckOffset, PreviewCondition.Editor, 0, Color.green, Color.red)

            if ( UnityEngine.Physics.Raycast(transform.position, Vector3.down, out slopeHit, (groundCheckRayLength / 2) + groundCheckOffset) )
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

        /// <summary>
        /// Method used to calclulate the movement direction while on a slope.
        /// </summary>
        /// <returns>The normalized ProjectOnPlane Vector3 where the adjusted direction is calculated.</returns>
        private Vector3 GetSlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(slopeHit.point, GetSlopeMoveDirection());
        }
    }
}
