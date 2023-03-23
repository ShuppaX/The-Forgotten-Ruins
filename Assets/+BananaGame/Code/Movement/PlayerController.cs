using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Unity.VisualScripting;

namespace BananaSoup
{
    [RequireComponent(typeof(PlayerBase), typeof(PlayerStateManager), typeof(CalculateMovementDirection))]
    [RequireComponent(typeof(AllowMovement), typeof(GroundCheck), typeof(GroundAhead))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Tooltip("The amount of force for moving the character.")]
        private float movementSpeed = 7.0f;
        [SerializeField, Tooltip("The multiplier for y-velocity when falling.")]
        private float fallingVelocityMultiplier = 1.5f;
        [SerializeField, Tooltip("The games camera angle. (Used to calculate correct movement directions.)")]
        private float cameraAngle = 45.0f;
        [SerializeField, Tooltip("Maximum allowed slope angle for moving.")]
        private float maxSlopeAngle = 40.0f;
        [SerializeField, Tooltip("Offset which is added to different Raycasts length. (GroundCheck, SlopeCheck, AllowMovement)")]
        private float groundCheckLength = 0.1f;
        [SerializeField]
        private LayerMask groundLayer;

        // References to other components
        private Rigidbody rb = null;
        private CalculateMovementDirection directionCalculator = null;
        private AllowMovement allowMovement = null;
        private GroundCheck groundCheck = null;
        private GroundAhead groundAhead = null;
        private PlayerStateManager psm = null;
        private DebugManager debug = null;

        // Variables used to store in script values
        private Vector3 movementInput = Vector3.zero;
        private Vector3 isometricDirection = Vector3.zero;

        private float latestMovementspeed = 0.0f;

        private bool hasMoveInput = false;
        private bool wasPushed = false;

        [Header("Constant strings used for PlayerState handling")]
        public const string moving = "Moving";
        public const string notMoving = "Idle";

        public float MaxSlopeAngle
        {
            get { return maxSlopeAngle; }
        }

        /// <summary>
        /// Public property to define groundCheck Raycast length.
        /// </summary>
        public float GroundCheckLength
        {
            get { return groundCheckLength; }
        }

        /// <summary>
        /// Public property to define the groundLayer LayerMask for several check scripts.
        /// </summary>
        public LayerMask GroundLayer
        {
            get { return groundLayer; }
        }

        private void Start()
        {
            Setup();
        }

        /// <summary>
        /// Setup method which is called in Start() to get components and Instances.
        /// </summary>
        private void Setup()
        {
            GetInstances();
            GetComponents();
        }

        /// <summary>
        /// Method to get references of existing Instances and to throw an error if it is null.
        /// </summary>
        private void GetInstances()
        {
            psm = PlayerStateManager.Instance;
            if ( psm == null )
            {
                Debug.LogError(gameObject.name + " couldn't find an Instance of PlayerStateManager!");
            }

            debug = DebugManager.Instance;
            if ( debug == null )
            {
                Debug.LogError(gameObject.name + " couldn't find an Instance of DebugManager!");
            }
        }

        /// <summary>
        /// Method used to get different components and to throw an error if a component
        /// is missing.
        /// </summary>
        private void GetComponents()
        {
            rb = GetDependency<Rigidbody>();
            directionCalculator = GetDependency<CalculateMovementDirection>();
            allowMovement = GetDependency<AllowMovement>();
            groundCheck = GetDependency<GroundCheck>();
            groundAhead = GetDependency<GroundAhead>();
        }

        /// <summary>
        /// Method to simplify getting components and to throw an error if it's null
        /// this improves readability.
        /// </summary>
        /// <typeparam name="T">The name of the component to get.</typeparam>
        /// <returns>The wanted component if it's found.</returns>
        private T GetDependency<T>() where T : Component
        {
            T component = GetComponent<T>();
            if ( component == null )
            {
                Debug.LogError($"The component of type {typeof(T).Name} couldn't be found on the " + gameObject.name + "!");
            }

            return component;
        }

        private void Update()
        {
            UpdateMovementspeedDebug();
            SetWasPushedFalse();
        }

        /// <summary>
        /// Update the movementSpeed display debug once every time the rb.velocity.sqrMagnitude changes.
        /// </summary>
        private void UpdateMovementspeedDebug()
        {
            if ( latestMovementspeed != rb.velocity.sqrMagnitude )
            {
                latestMovementspeed = rb.velocity.sqrMagnitude;
                debug.UpdateMovementSpeedText(Mathf.Round(rb.velocity.magnitude));
            }
        }

        /// <summary>
        /// Method used to set wasPushed to false if the player has ground in front of them
        /// and wasPushed is true.
        /// </summary>
        private void SetWasPushedFalse()
        {
            if ( groundAhead.IsGroundAhead && wasPushed == true )
            {
                wasPushed = false;
            }
        }

        private void FixedUpdate()
        {
            // If the character is movable, has movement input and has ground in
            // front of it call Move method.
            // If it is movable and has no movement input then call StoppedMoving.
            if ( PlayerBase.Instance.IsMovable )
            {
                if ( hasMoveInput && groundAhead.IsGroundAhead )
                {
                    Move();
                }
                else if ( !hasMoveInput )
                {
                    StoppedMoving();
                }
            }

            // If hte character isn't movable call StopMovement method.
            if ( !PlayerBase.Instance.IsMovable )
            {
                StopMovement();
            }

            // If the character can be turned call the Look method.
            if ( PlayerBase.Instance.IsTurnable )
            {
                Look();
            }

            // If the character doesn't have ground in front of it it gets
            // pushed back and the bool wasPushed is set to true.
            if ( !groundAhead.IsGroundAhead )
            {
                rb.velocity = -transform.forward;
                wasPushed = true;
            }

            // If the character is not grounded and is falling apply a multiplied
            // Physics.gravity.y to make the character fall down faster.
            if ( rb.velocity.y < 0 && !groundCheck.IsGrounded )
            {
                rb.velocity -= transform.up * fallingVelocityMultiplier;
            }
        }

        /// <summary>
        /// Used to get the players input and then store it into the Vector3 movementInput
        /// then it gets set as the isometricDirection after it is converted with the
        /// IsoVectorConvert.
        /// Set hasMoveInput to true while input is held and false when input action is 
        /// canceled (also reset the movementInput vector).
        /// </summary>
        /// <param name="context">The players movement input.</param>
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            movementInput.Set(input.x, 0.0f, input.y);
            isometricDirection = IsoVectorConvert(movementInput);
            hasMoveInput = true;

            if ( context.phase == InputActionPhase.Canceled )
            {
                movementInput = Vector3.zero;
                hasMoveInput = false;
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
            Quaternion rotation = Quaternion.Euler(0.0f, cameraAngle, 0.0f);
            Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
            Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
            return result;
        }

        /// <summary>
        /// Moves the character in the direction of the players input.
        /// The direction is dependent on the ground the player is standing on,
        /// for example the direction is in a 25 degree angle if the player is standing
        /// on a 25 degree angled ground.
        /// </summary>
        private void Move()
        {
            if ( !IsMovementAllowed() )
            {
                return;
            }

            if ( wasPushed )
            {
                return;
            }

            if ( groundCheck.IsGrounded )
            {
                Vector3 forceToApply = GetMovementDirection(isometricDirection) * movementSpeed;
                rb.velocity = forceToApply;
                psm.SetPlayerState(moving);
                debug.UpdatePlayerStateText();
            }
        }

        /// <summary>
        /// Method which is called when the player has stopped movement input.
        /// </summary>
        private void StoppedMoving()
        {
            if ( psm.currentPlayerState == PlayerStateManager.PlayerState.Moving )
            {
                rb.velocity = Vector3.zero;
                psm.SetPlayerState(notMoving);
                debug.UpdatePlayerStateText();
            }
        }

        /// <summary>
        /// Set the players rigidbody.velocity to Vector3.zero.
        /// </summary>
        private void StopMovement()
        {
            rb.velocity = Vector3.zero;
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
            return allowMovement.IsMovementAllowed;
        }

        /// <summary>
        /// Rotates the character towards the direction of movement.
        /// The correct direction is calculated using IsoVectorConvert method.
        /// Moving up rotates the character up instead of up and left.
        /// </summary>
        private void Look()
        {
            if ( hasMoveInput )
            {
                var rot = Quaternion.LookRotation(IsoVectorConvert(movementInput), Vector3.up);

                transform.rotation = rot;
            }
        }
    }
}
