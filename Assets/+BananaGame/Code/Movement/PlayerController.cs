using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace BananaSoup
{
    [RequireComponent(typeof(PlayerBase), typeof(PlayerStateManager), typeof(CalculateMovementDirection))]
    [RequireComponent(typeof(AllowMovement), typeof(GroundCheck), typeof(GroundAhead))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Tooltip("The amount of force for moving the character.")]
        private float movementSpeed = 7.0f;
        [SerializeField, Tooltip("The games camera angle. (Used to calculate correct movement directions.)")]
        private float cameraAngle = 45.0f;
        [SerializeField, Tooltip("Maximum allowed slope angle for moving.")]
        private float maxSlopeAngle = 40.0f;
        [SerializeField, Tooltip("Offset which is added to different Raycasts length. (GroundCheck, SlopeCheck, AllowMovement)")]
        private float raycastLength = 0.25f;
        [SerializeField]
        private LayerMask groundLayer;

        // References to other components
        private Rigidbody rb = null;
        private CapsuleCollider playerCollider = null;
        private CalculateMovementDirection directionCalculator = null;
        private AllowMovement allowMovement = null;
        private GroundCheck groundCheck = null;
        private GroundAhead groundAhead = null;

        // Variables used to store in script values
        private Vector3 movementInput = Vector3.zero;
        private Vector3 isometricDirection = Vector3.zero;

        private float latestMovementspeed = 0.0f;

        private bool wasPushed = false;

        [Header("UnityActions to manage PlayerStates")]
        public UnityAction onPlayerMoveInput;
        public UnityAction onNoPlayerMoveInput;
        public UnityAction onVelocityChanged;

        public float MaxSlopeAngle
        {
            get { return maxSlopeAngle; }
        }

        /// <summary>
        /// Public property for raycastLength which is used to define the length of the
        /// GroundCheck and AllowMovement Raycasts length.
        /// </summary>
        public float RayLength
        {
            get { return raycastLength; }
        }

        public LayerMask GroundLayer
        {
            get { return groundLayer; }
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
        /// Setup method which is called in Start() to get components.
        /// </summary>
        private void Setup()
        {
            GetComponents();
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
                Debug.LogError("A Rigidbody component couldn't be found on the " + gameObject.name + "!");
            }

            playerCollider = GetComponent<CapsuleCollider>();
            if ( playerCollider == null )
            {
                Debug.LogError("A CapsuleCollider component couldn't be found on the " + gameObject.name + "!");
            }

            directionCalculator = GetComponent<CalculateMovementDirection>();
            if ( directionCalculator == null )
            {
                Debug.LogError("A CalculateMovementDirection component couldn't be found on the " + gameObject.name + "!");
            }

            allowMovement = GetComponent<AllowMovement>();
            if ( allowMovement == null )
            {
                Debug.LogError("A AllowMovement component couldn't be found on the " + gameObject.name + "!");
            }

            groundCheck = GetComponent<GroundCheck>();
            if ( groundCheck == null )
            {
                Debug.LogError("A GroundCheck component couldn't be found on the " + gameObject.name + "!");
            }

            groundAhead = GetComponent<GroundAhead>();
            if ( groundAhead == null )
            {
                Debug.LogError("A GroundAhead component couldn't be found on the " + gameObject.name + "!");
            }
        }

        private void Update()
        {
            InvokeEventOnRbVelocityChanged();
            SetWasPushedFalse();
        }

        /// <summary>
        /// Invoke the event once when the rb.velocity.sqrMagnitude changes.
        /// </summary>
        private void InvokeEventOnRbVelocityChanged()
        {
            if ( latestMovementspeed != rb.velocity.sqrMagnitude )
            {
                latestMovementspeed = rb.velocity.sqrMagnitude;
                onVelocityChanged.Invoke();
            }
        }

        /// <summary>
        /// Method used to set wasPushed to false if the player has ground in front of them
        /// </summary>
        private void SetWasPushedFalse()
        {
            if ( groundAhead.IsGroundAhead )
            {
                wasPushed = false;
            }
        }

        private void FixedUpdate()
        {
            if ( PlayerBase.Instance.IsMovable )
            {
                if ( IsMovementAllowed() && groundAhead.IsGroundAhead && !wasPushed )
                {
                    Move();
                }
                else if ( !groundAhead.IsGroundAhead && groundCheck.IsGrounded )
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
        /// which is then converted with the IsoVectorConvert method and stored in
        /// movementDirection.
        /// </summary>
        /// <param name="context">The players movement input.</param>
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            movementInput.Set(input.x, 0, input.y);
            isometricDirection = IsoVectorConvert(movementInput);
            onPlayerMoveInput.Invoke();

            if ( context.phase == InputActionPhase.Canceled )
            {
                onNoPlayerMoveInput.Invoke();
            }
        }

        /// <summary>
        /// Method used to convert the original movement vector to match the correct
        /// angles of movement for the isometric view. The method uses the cameras angle
        /// in a Quaternion and then that Quaternion is converted into a 4x4.Rotate Matrix.
        /// </summary>
        /// <param name="vector">The vector that you want to convert.</param>
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
        /// Uses the method GetMovementDirection to get the corrected direction for
        /// slopes and even ground and then that is multiplied by the movementSpeed.
        /// </summary>
        private void Move()
        {
            if ( PlayerStateManager.Instance.currentPlayerState == PlayerStateManager.PlayerState.Dashing )
            {
                return;
            }

            if ( groundCheck.IsGrounded )
            {
                Vector3 forceToApply = GetMovementDirection(isometricDirection) * movementSpeed;
                rb.velocity = forceToApply;
            }
        }

        /// <summary>
        /// Method used to get the corrected and calculated direction from directionCalculator.
        /// </summary>
        /// <param name="direction">The direction you want to correct.</param>
        /// <returns>The corrected Vector3 direction.</returns>
        private Vector3 GetMovementDirection(Vector3 direction)
        {
            return directionCalculator.CalculateDirection(direction);
        }

        /// <summary>
        /// Method used to check the allowMovement.CanMove value with the allowed
        /// maximum slope angle.
        /// </summary>
        /// <returns>True if the player is allowed to move on the ground they're on,
        /// false if not.</returns>
        private bool IsMovementAllowed()
        {
            return allowMovement.CanMove(maxSlopeAngle);
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
    }
}
