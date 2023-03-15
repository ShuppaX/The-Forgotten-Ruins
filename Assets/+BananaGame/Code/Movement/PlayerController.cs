using UnityEngine;
using UnityEngine.InputSystem;
using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine.Events;

namespace BananaSoup
{
    [RequireComponent(typeof(PlayerBase), typeof(PlayerStateManager), typeof(CalculateMovementDirection))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Tooltip("The amount of force for moving the character.")]
        private float movementSpeed = 7.0f;

        [SerializeField, Tooltip("The games camera angle. (Used to calculate correct movement directions.)")]
        private float cameraAngle = 45.0f;

        [Header("GroundCheck variables")]
        [SerializeField]
        private float groundCheckRayLengthOffset = 0.25f;

        private bool[] groundCheckRays = new bool[4];

        private Vector3 groundCheckFrontOrigin = Vector3.zero;
        private Vector3 groundCheckBackOrigin = Vector3.zero;
        private Vector3 groundCheckRightOrigin = Vector3.zero;
        private Vector3 groundCheckLeftOrigin = Vector3.zero;

        private float groundCheckRayOriginOffset = 0.0f;
        private float groundCheckRadiusMultiplier = 0.9f;

        private float groundCheckRayLength = 0.0f;

        private bool groundCheckChanged = false;

        [SerializeField]
        private LayerMask groundLayer;

        [Header("Slope variables")]
        [SerializeField, Tooltip("The maximum angle for a slope the player can walk on.")]
        private float maxSlopeAngle = 70.0f;

        private RaycastHit allowedSlopeRayHit;

        [Header("GroundAhead variables")]
        [SerializeField]
        private float groundAheadRadiusMultiplier = 1.6f;
        private float groundAheadRayLengthMultiplier = 2.0f;
        private float groundAheadSphereOffset = 0.0f;
        private float groundAheadRayLength = 0.0f;

        private bool[] groundAheadSpheres = new bool[3];

        private Vector3 groundAheadLeftOrigin = Vector3.zero;
        private Vector3 groundAheadCenterOrigin = Vector3.zero;
        private Vector3 groundAheadRightOrigin = Vector3.zero;

        private float groundAheadSphereRadius = 0.025f;

        private bool wasPushed = false;

        // Variables used to store in script values and references.
        private Rigidbody rb;
        private CapsuleCollider playerCollider;
        private CalculateMovementDirection directionCalculator;

        private Vector3 movementInput = Vector3.zero;
        private Vector3 movementDirection = Vector3.zero;

        private float latestMovementspeed = 0.0f;

        [Header("UnityActions to manage PlayerStates")]
        public UnityAction onPlayerGroundedAndIdle;
        public UnityAction onPlayerInAir;
        public UnityAction onPlayerMoveInput;
        public UnityAction onNoPlayerMoveInput;
        public UnityAction onVelocityChanged;
        public UnityAction onGroundCheck;

        public bool IsGrounded
        {
            get { return GroundCheck(); }
        }

        public float PlayerMovementspeed
        {
            get { return Mathf.Round(rb.velocity.magnitude); }
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
                Debug.LogError("A Rigidbody component couldn't be found for the " + gameObject.name + "!");
            }

            playerCollider = GetComponent<CapsuleCollider>();
            if ( playerCollider == null )
            {
                Debug.LogError("A CapsuleCollider component couldn't be found for the " + gameObject.name + "!");
            }

            directionCalculator = GetComponent<CalculateMovementDirection>();
            if ( directionCalculator == null )
            {
                Debug.LogError("A CalculateMovementDirection component couldn't be found for the " + gameObject.name + "!");
            }
        }

        /// <summary>
        /// Method to set the variables used for GroundChecks raycasting (used in Setup()).
        /// </summary>
        private void SetRayVariables()
        {
            groundCheckRayLength = (transform.localScale.y / 2) + groundCheckRayLengthOffset;
            groundCheckRayOriginOffset = playerCollider.radius * groundCheckRadiusMultiplier;

            groundAheadSphereOffset = playerCollider.radius * groundAheadRadiusMultiplier;
            groundAheadRayLength = (transform.localScale.y * groundAheadRayLengthMultiplier);
        }


        private void Update()
        {
            InvokeEventOnGroundCheckValueChange();
            InvokeEventOnRbVelocityChanged();
        }

        /// <summary>
        /// Invoke the event once when the GroundCheck value changes.
        /// </summary>
        private void InvokeEventOnGroundCheckValueChange()
        {
            if ( groundCheckChanged != GroundCheck() )
            {
                groundCheckChanged = !groundCheckChanged;
                onGroundCheck.Invoke();
            }
        }

        /// <summary>
        /// Invoke the event once when the rb.velocity.sqrMagnitude changes.
        /// </summary>
        private void InvokeEventOnRbVelocityChanged()
        {
            if( latestMovementspeed != rb.velocity.sqrMagnitude )
            {
                latestMovementspeed = rb.velocity.sqrMagnitude;
                onVelocityChanged.Invoke();
            }
        }

        private void FixedUpdate()
        {
            if ( PlayerBase.Instance.IsMovable )
            {
                if ( AllowedSlope() && GroundAhead() && WasGrounded() && !wasPushed )
                {
                    Move();
                }
                else if ( !GroundAhead() && GroundCheck() )
                {
                    rb.velocity = -transform.forward;
                    wasPushed = true;
                }
            }

            if ( PlayerBase.Instance.IsTurnable )
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
            onPlayerMoveInput.Invoke();

            if ( context.phase == InputActionPhase.Canceled )
            {
                onNoPlayerMoveInput.Invoke();
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
            if ( PlayerStateManager.Instance.currentPlayerState == PlayerStateManager.PlayerState.Dashing )
            {
                return;
            }

            if ( GroundCheck() )
            {
                Vector3 forceToApply = GetMovementDirection() * movementSpeed;


                rb.velocity = forceToApply;
            }
        }

        private Vector3 GetMovementDirection()
        {
            return directionCalculator.CalculateDirection(
                    allowedSlopeRayHit, groundCheckRayLength / 2, groundCheckRayLengthOffset, movementDirection);
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

                transform.rotation = rot;
            }
        }

        /// <summary>
        /// Uses four separate raycasts to track if the player is standing on something or not.
        /// </summary>
        /// <returns>True if any of the rays hit an object on the groundLayer, false if not.</returns>
        private bool GroundCheck()
        {
            CalculateGroundCheckRayOriginPoints();

            GroundCheckRay(0, groundCheckFrontOrigin);
            GroundCheckRay(1, groundCheckBackOrigin);
            GroundCheckRay(2, groundCheckRightOrigin);
            GroundCheckRay(3, groundCheckLeftOrigin);

            foreach ( bool groundCheckRay in groundCheckRays )
            {
                if ( groundCheckRay )
                {
                    onPlayerGroundedAndIdle.Invoke();
                    return true;
                }
            }

            onPlayerInAir.Invoke();
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
            groundCheckRays[index] = UnityEngine.Physics.Raycast(position, Vector3.down, groundCheckRayLength, groundLayer);

            // Can be used to debug and draw the Raycast(s) using the RotaryHeart
            // Physics debug library.
            //raycasts[index] = RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(position, Vector3.down, groundCheckRayLength, groundLayer, PreviewCondition.Editor, 0, Color.green, Color.red);
        }

        /// <summary>
        /// Method used to calculate and update the starting points of the Raycasts
        /// used for the GroundCheck method.
        /// </summary>
        private void CalculateGroundCheckRayOriginPoints()
        {
            groundCheckFrontOrigin = transform.position + transform.forward * groundCheckRayOriginOffset;
            groundCheckBackOrigin = transform.position - transform.forward * groundCheckRayOriginOffset;
            groundCheckRightOrigin = transform.position + transform.right * groundCheckRayOriginOffset;
            groundCheckLeftOrigin = transform.position - transform.right * groundCheckRayOriginOffset;
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
        /// Method used to check if the slope the player is on is one that they can
        /// walk on or not. The calculation includes a Raycast from which the method
        /// gets the normal of the object hit and then determines the angle between that
        /// and Vector3.up.
        /// </summary>
        /// <returns>True if the angle between the hit objects normal and Vector3.up is within
        /// the allowed range, otherwise false.</returns>
        private bool AllowedSlope()
        {
            if ( UnityEngine.Physics.Raycast(transform.position, Vector3.down, out allowedSlopeRayHit, groundCheckRayLength, groundLayer) )
            {
                float angle = Vector3.Angle(Vector3.up, allowedSlopeRayHit.normal);
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
        /// <returns>False if any of the SphereCasts don't hit anything on the groundLayer,
        /// True otherwise.</returns>
        private bool GroundAhead()
        {
            CalculateGroundAheadSphereOriginPoints();

            GroundAheadSphereCast(0, groundAheadLeftOrigin);
            GroundAheadSphereCast(1, groundAheadCenterOrigin);
            GroundAheadSphereCast(2, groundAheadRightOrigin);

            foreach ( bool groundAheadSphere in groundAheadSpheres )
            {
                if ( !groundAheadSphere )
                {
                    return false;
                }
            }

            wasPushed = false;
            return true;
        }

        private void CalculateGroundAheadSphereOriginPoints()
        {
            groundAheadLeftOrigin = transform.position + (transform.forward * groundAheadSphereOffset) * 0.71f - (transform.right * groundAheadSphereOffset) * 0.71f;
            groundAheadCenterOrigin = transform.position + transform.forward * playerCollider.radius * 2.0f;
            groundAheadRightOrigin = transform.position + (transform.forward * groundAheadSphereOffset) * 0.71f + (transform.right * groundAheadSphereOffset) * 0.71f;
        }

        private void GroundAheadSphereCast(int index, Vector3 position)
        {
            RaycastHit hit;
            groundAheadSpheres[index] = UnityEngine.Physics.SphereCast(position, groundAheadSphereRadius, Vector3.down, out hit, groundAheadRayLength, groundLayer);

            // Can be used to debug and draw the Raycast(s) using the RotaryHeart
            // Physics debug library.
            //groundAheadSpheres[index] = RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(position, groundAheadSphereRadius, Vector3.down, groundAheadRayLength, groundLayer, PreviewCondition.Editor, 0, Color.green, Color.red);
        }
    }
}
