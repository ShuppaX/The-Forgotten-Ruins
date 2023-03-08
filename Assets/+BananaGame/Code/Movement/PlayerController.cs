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
        public static PlayerController Instance { get; private set; }

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

        private float groundCheckRayLength = 0.0f;

        private Vector3 negPositionX;
        private Vector3 posPositionX;
        private Vector3 negPositionZ;
        private Vector3 posPositionZ;

        private float groundCheckRayOffset = 0.0f;
        private float groundCheckRadiusMultiplier = 0.9f;

        [SerializeField]
        private LayerMask groundLayer;

        [Header("Slope variables")]
        [SerializeField, Tooltip("The maximum angle for a slope the player can walk on.")]
        private float maxSlopeAngle = 70.0f;

        private RaycastHit slopeHit;

        // Variables used to store in script values and references.
        private Rigidbody rb;
        private CapsuleCollider playerCollider;

        private Vector3 movementInput = Vector3.zero;
        private Vector3 movementDirection = Vector3.zero;

        private bool isMoving = false;

        private void Start()
        {
            Setup();
        }

        /// <summary>
        /// Setup method which is called in Start() to get different constant variables
        /// and/or components etc.
        /// </summary>
        private void Setup()
        {
            GetComponents();
            SetRayVariables();
        }

        /// <summary>
        /// Method used to get different components and to throw an error if a component
        /// is missing.
        /// </summary>
        private void GetComponents()
        {
            rb = GetComponent<Rigidbody>();
            if ( rb == null )
            {
                Debug.LogError("A Rigidbody couldn't be found on the " + gameObject + "!");
            }

            playerCollider = GetComponent<CapsuleCollider>();
            if ( playerCollider == null )
            {
                Debug.LogError("A CapsuleCollider couldn't be found on the " + gameObject + "!");
            }
        }

        /// <summary>
        /// Method to set the variables used for GroundChecks raycasting (used in Setup()).
        /// </summary>
        private void SetRayVariables()
        {
            groundCheckRayLength = (transform.localScale.y / 2) + groundCheckOffset;
            groundCheckRayOffset = playerCollider.radius * groundCheckRadiusMultiplier;
        }


        private void Update()
        {
            SetPlayerState();

            //Debug.Log("Ground check = " + GroundCheck());
            //Debug.Log("Slope check = " + OnSlope());
            //Debug.Log("RigidBodys velocity = " + rb.velocity);

            //Debug.Log("negPositionX: " + negPositionX);
            //Debug.Log("posPositionX: " + posPositionX);
            //Debug.Log("negPositionZ: " + negPositionZ);
            //Debug.Log("posPositionZ: " + posPositionZ);
        }

        private void FixedUpdate()
        {
            if ( PlayerBase.Instance.IsMovable )
            {
                SetDrag();

                if ( WasGrounded() )
                {
                    Move();
                }
            }

            if ( PlayerBase.Instance.IsTurnable )
            {
                Look();
            }
        }

        private void SetPlayerState()
        {
            if ( PlayerBase.Instance.playerState == PlayerBase.PlayerState.Dashing )
            {
                return;
            }

            if ( !GroundCheck() )
            {
                PlayerBase.Instance.playerState = PlayerBase.PlayerState.InAir;
            }
            else
            {
                if ( isMoving )
                {
                    PlayerBase.Instance.playerState = PlayerBase.PlayerState.Moving;
                }
                else if ( !isMoving )
                {
                    PlayerBase.Instance.playerState = PlayerBase.PlayerState.Idle;
                }
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
            isMoving = true;

            if ( context.phase == InputActionPhase.Canceled )
            {
                isMoving = false;
            }
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
            if ( PlayerBase.Instance.playerState == PlayerBase.PlayerState.Dashing )
            {
                return;
            }

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
        private void SetDrag()
        {
            if ( GroundCheck() )
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = fallingDrag;
            }
        }

        /// <summary>
        /// Method to check if the player was grounded on previous frame (used in FixedUpdate)
        /// </summary>
        /// <returns>True if the player was grounded, otherwise false.</returns>
        private bool WasGrounded()
        {
            if ( GroundCheck() )
            {
                return true;
            }
            else
            {
                return false;
            }
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

            GroundCheckRay(0, negPositionX);
            GroundCheckRay(1, posPositionX);
            GroundCheckRay(2, negPositionZ);
            GroundCheckRay(3, posPositionZ);

            foreach ( bool rayHit in raycasts )
            {
                if ( rayHit )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a raycast with the given parameters. The ray(s) are used for the GroundCheck.
        /// </summary>
        /// <param name="index">The index of the ray used (up to 4)</param>
        /// <param name="position">The start position (offset) of the ray.</param>
        private void GroundCheckRay(int index, Vector3 position)
        {
            raycasts[index] = UnityEngine.Physics.Raycast(position, Vector3.down, groundCheckRayLength);

            // This can be used for debugging the raycasts using the RotaryHeart physics debugging extension.
            // The ray is green if it hits the groundCheckLayer and red if it doesnt and there is also a red cross to mark the rayhit.
            //raycasts[index] = RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(position, Vector3.down, groundCheckRayLength, groundLayer, PreviewCondition.Editor, 0, Color.green, Color.red);
        }

        /// <summary>
        /// This method calculates the starting points for the four rays used for GroundCheck.
        /// </summary>
        private void CalculateGroundCheckRayStartPoints()
        {
            negPositionX.Set(transform.position.x - groundCheckRayOffset, transform.position.y, transform.position.z);
            posPositionX.Set(transform.position.x + groundCheckRayOffset, transform.position.y, transform.position.z);
            negPositionZ.Set(transform.position.x, transform.position.y, transform.position.z - groundCheckRayOffset);
            posPositionZ.Set(transform.position.x, transform.position.y, transform.position.z + groundCheckRayOffset);
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
