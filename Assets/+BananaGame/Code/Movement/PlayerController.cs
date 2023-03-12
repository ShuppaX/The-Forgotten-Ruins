using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RotaryHeart.Lib.PhysicsExtension;
using JetBrains.Annotations;

namespace BananaSoup
{
    [RequireComponent(typeof(PlayerBase), typeof(PlayerStateManager))]
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [Header("Movement")]
        [SerializeField, Tooltip("The amount of force for moving the character.")]
        private float movementSpeed = 7.0f;

        [SerializeField, Tooltip("The games camera angle. (Used to calculate correct movement directions.)")]
        private float cameraAngle = 45.0f;

        [Header("GroundCheck variables")]
        [SerializeField]
        private float groundCheckOffset = 0.05f;

        private bool[] raycasts = new bool[4];

        private Vector3 rayFrontPosition;
        private Vector3 rayBackPosition;
        private Vector3 rayRightPosition;
        private Vector3 rayLeftPosition;

        private float groundCheckRayOffset = 0.0f;
        private float groundCheckRadiusMultiplier = 0.9f;

        private float groundCheckRayLength = 0.0f;

        [SerializeField]
        private LayerMask groundLayer;

        [Header("Slope variables")]
        [SerializeField, Tooltip("The maximum angle for a slope the player can walk on.")]
        private float maxSlopeAngle = 70.0f;

        private RaycastHit rayHit;

        [Header("GroundAhead variables")]
        [SerializeField]
        private float groundAheadRadiusMultiplier = 1.1f;
        private float groundAheadRayLengthMultiplier = 2.0f;
        private float groundAheadRayOffset = 0.0f;
        private float groundAheadRayLength = 0.0f;

        // Variables used to store in script values and references.
        private Rigidbody rb;
        private CapsuleCollider playerCollider;

        private Vector3 movementInput = Vector3.zero;
        private Vector3 movementDirection = Vector3.zero;

        private bool isMoving = false;

        private void Awake()
        {
            if ( Instance == null )
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

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

            groundAheadRayOffset = playerCollider.radius * groundAheadRadiusMultiplier;
            groundAheadRayLength = (transform.localScale.y * groundAheadRayLengthMultiplier);
        }


        private void Update()
        {
            SetPlayerState();

            //Debug.Log("AllowedSlope: " + AllowedSlope());
            //Debug.Log("WasGrounded: " + WasGrounded());
            //Debug.Log("GroundCheck: " + GroundCheck());

            //Debug.Log("RigidBodys velocity = " + rb.velocity);
        }

        private void FixedUpdate()
        {
            if ( PlayerBase.Instance.IsMovable )
            {
                if ( WasGrounded() && AllowedSlope() && GroundAhead() )
                {
                    Move();
                }
                else if ( !GroundAhead() && GroundCheck() )
                {
                    rb.velocity = Vector3.zero;
                }
            }

            if ( PlayerBase.Instance.IsTurnable )
            {
                Look();
            }
        }

        private void SetPlayerState()
        {
            if ( PlayerStateManager.Instance.playerState == PlayerStateManager.State.Dashing )
            {
                return;
            }

            if ( !GroundCheck() )
            {
                PlayerStateManager.Instance.playerState = PlayerStateManager.State.InAir;
            }
            else
            {
                if ( isMoving )
                {
                    PlayerStateManager.Instance.playerState = PlayerStateManager.State.Moving;
                }
                else if ( !isMoving )
                {
                    PlayerStateManager.Instance.playerState = PlayerStateManager.State.Idle;
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
            if ( PlayerStateManager.Instance.playerState == PlayerStateManager.State.Dashing )
            {
                return;
            }

            if ( GroundCheck() )
            {
                //rb.AddForce(movementDirection * movementForce, ForceMode.Force);

                Vector3 forceToApply = GetMoveDirection() * movementSpeed;

                rb.velocity = forceToApply;
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
        /// <returns>True if any of the rays hit an object on the groundLayer, false if not.</returns>
        private bool GroundCheck()
        {
            //if ( RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(transform.position, Vector3.down, groundCheckRayLength, PreviewCondition.Editor, 0, Color.green, Color.red) )
            //{
            //    return true;
            //}

            //if ( UnityEngine.Physics.Raycast(transform.position, Vector3.down, groundCheckRayLength) )
            //{
            //    return true;
            //}

            CalculateGroundCheckRayStartPoints();

            GroundCheckRay(0, rayFrontPosition);
            GroundCheckRay(1, rayBackPosition);
            GroundCheckRay(2, rayRightPosition);
            GroundCheckRay(3, rayLeftPosition);

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
        /// Method that makes a Raycast with given parameters. (Used with an array for
        /// several Raycasts)
        /// </summary>
        /// <param name="index">The index of the array used to store the value of
        /// the Raycast.</param>
        /// <param name="position">The position the Raycast originates from.</param>
        private void GroundCheckRay(int index, Vector3 position)
        {
            raycasts[index] = UnityEngine.Physics.Raycast(position, Vector3.down, groundCheckRayLength, groundLayer);

            // Can be used to debug and draw the Raycast(s) using the RotaryHeart
            // Physics debug library.
            //raycasts[index] = RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(position, Vector3.down, groundCheckRayLength, groundLayer, PreviewCondition.Editor, 0, Color.green, Color.red);
        }

        /// <summary>
        /// Method used to calculate and update the starting points of the Raycasts
        /// used for the GroundCheck method.
        /// </summary>
        private void CalculateGroundCheckRayStartPoints()
        {
            rayFrontPosition = transform.position + transform.forward * groundCheckOffset;
            rayBackPosition = transform.position - transform.forward * groundCheckOffset;
            rayRightPosition = transform.position + transform.right * groundCheckOffset;
            rayLeftPosition = transform.position - transform.right * groundCheckOffset;
        }

        /// <summary>
        /// Method used to check if the slope the player is on is one that they can
        /// walk on or not. The calculation includes a Raycast from which the method
        /// gets the normal of the object hit and then determines the angle between that
        /// and Vector3.up.
        /// </summary>
        /// <returns>True if the angle between the hit objects normal and Vector3.up is within
        /// the allowed range, otherwise false.</returns>
        private bool AllowedSlope()
        {
            if ( UnityEngine.Physics.Raycast(transform.position, Vector3.down, out rayHit, groundCheckRayLength, groundLayer) )
            {
                float angle = Vector3.Angle(Vector3.up, rayHit.normal);
                bool angleLessThanMaxSlopeAngle = (angle < maxSlopeAngle);

                if ( angleLessThanMaxSlopeAngle )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Method used to check if there is ground ahead of the player. Used in FixedUpdate()
        /// to determine if the player can walk forward or not.
        /// </summary>
        /// <returns>True if the Raycast hits something on the groundLayer, otherwise false.</returns>
        private bool GroundAhead()
        {
            if ( RotaryHeart.Lib.PhysicsExtension.Physics.Raycast((transform.position + transform.forward * groundAheadRayOffset), Vector3.down, groundAheadRayLength, groundLayer, PreviewCondition.Editor, 0, Color.magenta, Color.white) )
            {
                return true;
            }

            //if ( UnityEngine.Physics.Raycast((transform.position + transform.forward * groundAheadRayOffset), Vector3.down, groundAheadRayLength, groundLayer))
            //{
            //    return true;
            //}

            return false;
        }

        /// <summary>
        /// Method used to calclulate the movement direction while on a slope.
        /// </summary>
        /// <returns>The normalized ProjectOnPlane Vector3 where the adjusted direction is calculated.</returns>
        private Vector3 GetMoveDirection()
        {
            UnityEngine.Physics.Raycast(transform.position, Vector3.down, out rayHit, (groundCheckRayLength / 2) + groundCheckOffset);

            return Vector3.ProjectOnPlane(movementDirection, rayHit.normal).normalized;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(rayHit.point, GetMoveDirection());
        }
    }
}
