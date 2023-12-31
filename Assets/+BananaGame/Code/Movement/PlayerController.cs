using UnityEngine;
using UnityEngine.InputSystem;
using BananaSoup.Managers;
using BananaSoup.Utilities;

namespace BananaSoup
{
    [RequireComponent(typeof(PlayerBase), typeof(PlayerStateManager), typeof(CalculateMovementDirection))]
    [RequireComponent(typeof(AllowMovement), typeof(GroundCheck), typeof(GroundAhead))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Tooltip("The movement speed of the character when moving.")]
        private float movementSpeed = 7.0f;
        [SerializeField, Tooltip("Movement animation blending damp speed.")]
        private float movementDamp = 0.05f;
        [SerializeField, Tooltip("The turning speed of the character when rotating.")]
        private float turnSpeed = 760.0f;
        [SerializeField, Tooltip("The movementspeed of the character when interacting.")]
        private float interactMovementSpeed = 3.5f;
        [SerializeField, Tooltip("The movementspeed of the character when it's getting pushed back.")]
        private float pushbackMovementSpeed = 3.5f;
        [SerializeField, Tooltip("The multiplier for y-velocity when falling.")]
        private float fallingVelocityMultiplier = 1.5f;
        [SerializeField, Tooltip("The games camera angle. (Used to calculate correct movement directions.)")]
        private float cameraAngle = 45.0f;
        [SerializeField, Tooltip("Maximum allowed slope angle for moving.")]
        private float maxSlopeAngle = 40.0f;
        [SerializeField, Tooltip("Offset which is added to different Raycasts length. (GroundCheck, SlopeCheck, AllowMovement)")]
        private float groundCheckLength = 0.1f;
        [SerializeField]
        private LayerMask walkableLayer;

        // References to other components
        private Rigidbody rb = null;
        private CapsuleCollider playerCollider = null;
        private CalculateMovementDirection directionCalculator = null;
        private AllowMovement allowMovement = null;
        private GroundCheck groundCheck = null;
        private GroundAhead groundAhead = null;
        private PlayerStateManager psm = null;
        private Animator animator = null;

        // Variables used to store in script values
        private Vector3 movementInput = Vector3.zero;
        private Vector3 isometricDirection = Vector3.zero;

        private bool hasMoveInput = false;
        private bool wasPushed = false;

        private float inputMagnitude = 0.0f;

        public float InputMagnitude
        {
            get => inputMagnitude;
        }

        [Header("Constant PlayerStates used for PlayerState handling")]
        private const PlayerStateManager.PlayerState moving = PlayerStateManager.PlayerState.Moving;
        private const PlayerStateManager.PlayerState interactingIdle = PlayerStateManager.PlayerState.InteractingIdle;
        private const PlayerStateManager.PlayerState interactingMove = PlayerStateManager.PlayerState.InteractingMove;
        private const PlayerStateManager.PlayerState pickingUp = PlayerStateManager.PlayerState.PickingUp;
        private const PlayerStateManager.PlayerState puttingDown = PlayerStateManager.PlayerState.PuttingDown;
        private const PlayerStateManager.PlayerState inAir = PlayerStateManager.PlayerState.InAir;

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
        public LayerMask WalkableLayer
        {
            get { return walkableLayer; }
        }

        /// <summary>
        /// Public property to tell PlayerStateManager if there is a moveInput active.
        /// </summary>
        public bool HasMoveInput
        {
            get { return hasMoveInput; }
        }

        public Rigidbody Rb
        {
            get => rb;
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
        }

        /// <summary>
        /// Method used to get different components and to throw an error if a component
        /// is missing.
        /// </summary>
        private void GetComponents()
        {
            rb = GetDependency<Rigidbody>();
            playerCollider = GetDependency<CapsuleCollider>();
            directionCalculator = GetDependency<CalculateMovementDirection>();
            allowMovement = GetDependency<AllowMovement>();
            groundCheck = GetDependency<GroundCheck>();
            groundAhead = GetDependency<GroundAhead>();
            animator = GetDependency<Animator>();
        }

        /// <summary>
        /// Method to simplify getting components and to throw an error if it's null
        /// this improves readability.
        /// This method is by Sami Kojo-Fry from 2023 Tank Game.
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
                PushPlayerBack();
            }

            SetWasPushedFalse();

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

            inputMagnitude = Mathf.Clamp01(movementInput.magnitude);
            animator.SetFloat("InputMagnitude", inputMagnitude, movementDamp, Time.deltaTime);

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
                Vector3 forceToApply = GetMovementDirection(isometricDirection) * GetMovementSpeed();
                rb.velocity = forceToApply;

                if ( !CanStateBeMoving() )
                {
                    psm.SetPlayerState(interactingMove);
                    return;
                }

                psm.SetPlayerState(moving);
            }
        }

        /// <summary>
        /// Method used to check if the PlayerState can be moving.
        /// </summary>
        /// <returns>False if it's not allowed to be moving, otherwise true.</returns>
        private bool CanStateBeMoving()
        {
            if ( psm.CurrentPlayerState == interactingIdle )
            {
                return false;
            }

            if ( psm.CurrentPlayerState == interactingMove )
            {
                return false;
            }

            if ( psm.CurrentPlayerState == pickingUp )
            {
                return false;
            }

            if ( psm.CurrentPlayerState == puttingDown )
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Used in Move() to get the correct movementSpeed while interacting and moving
        /// and while just moving.
        /// </summary>
        /// <returns>interactMovementSpeed if currentPlayerState equals interacting,
        /// otherwise normal movementSpeed.</returns>
        private float GetMovementSpeed()
        {
            if ( psm.CurrentPlayerState == interactingMove )
            {
                return interactMovementSpeed;
            }
            else
            {
                return movementSpeed;
            }
        }

        /// <summary>
        /// Method used to push the player back towards the center of the object they're on
        /// if they are trying to go off a ledge they're not allowed to go down from.
        /// </summary>
        private void PushPlayerBack()
        {
            GameObject below = GetGameObjectBelow();

            if ( below != null )
            {
                var belowTopCenter = below.GetComponent<Collider>().bounds.center
                    + Vector3.up * below.GetComponent<Collider>().bounds.extents.y;

                Vector3 direction = (belowTopCenter - transform.position).normalized;

                rb.velocity = direction * pushbackMovementSpeed;
            }

            wasPushed = true;
        }

        /// <summary>
        /// Method used to get the gameObject below the player character. Used in
        /// PushPlayerBack().
        /// </summary>
        /// <returns>The object the player is standing on, if the player is standing
        /// on nothing, then return null.</returns>
        private GameObject GetGameObjectBelow()
        {
            RaycastHit hit;

            if ( Physics.Raycast(((transform.position + Vector3.up) + -transform.forward * playerCollider.radius),
                Vector3.down, out hit, 4.0f, walkableLayer) )
            {
                if ( hit.collider.gameObject != gameObject )
                {
                    return hit.collider.gameObject;
                }
            }

            return null;
        }

        /// <summary>
        /// Called when the player input is canceled. If the players state is "Moving",
        /// then the players state is set to Idle or InAir depending on if the player
        /// is grounded.
        /// Also set the rigidbody.velocity to Vector3.zero if the player is grounded.
        /// If the player is interacting and stopped moving set the state to interactingIdle.
        /// </summary>
        private void StoppedMoving()
        {
            if ( psm.CurrentPlayerState == moving )
            {
                if ( groundCheck.IsGrounded )
                {
                    psm.ResetPlayerState();
                }
                else if ( !groundCheck.IsGrounded )
                {
                    psm.SetPlayerState(inAir);
                }
            }

            if ( psm.CurrentPlayerState == interactingMove )
            {
                psm.SetPlayerState(interactingIdle);
            }

            if ( groundCheck.IsGrounded )
            {
                rb.velocity = Vector3.zero;
            }
        }

        /// <summary>
        /// Set the players rigidbody.velocity to Vector3.zero IF the player is not
        /// dashing or in the air.
        /// </summary>
        private void StopMovement()
        {
            if ( psm.CurrentPlayerState == PlayerStateManager.PlayerState.Dashing )
            {
                return;
            }

            if ( psm.CurrentPlayerState == PlayerStateManager.PlayerState.InAir )
            {
                return;
            }

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
        /// Uses Quaternion.RotateTowards for smooth turning.
        /// </summary>
        private void Look()
        {
            if ( hasMoveInput )
            {
                var rot = Quaternion.LookRotation(IsoVectorConvert(movementInput), Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
            }
        }
    }
}
