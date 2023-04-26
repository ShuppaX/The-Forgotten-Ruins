using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine;
using BananaSoup.Managers;

namespace BananaSoup.Utilities
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField]
        private bool isDrawingRays = false;

        private bool[] rays = new bool[4];

        private Vector3 frontRayOrigin = Vector3.zero;
        private Vector3 backRayOrigin = Vector3.zero;
        private Vector3 rightRayOrigin = Vector3.zero;
        private Vector3 leftRayOrigin = Vector3.zero;

        private Vector3 originHeightOffset = Vector3.zero;

        private float rayOriginOffset = 0.0f;
        private float colliderRadiusMultiplier = 0.9f;

        private float rayLength = 0.0f;

        private bool isGrounded = false;
        private bool groundCheckChanged = false;

        private LayerMask groundLayer;

        private CapsuleCollider playerCollider = null;
        private PlayerStateManager psm = null;
        private PlayerBase playerBase = null;

        [Header("Constant PlayerState used for PlayerState handling")]
        private const PlayerStateManager.PlayerState inAir = PlayerStateManager.PlayerState.InAir;
        private const PlayerStateManager.PlayerState interactingIdle = PlayerStateManager.PlayerState.InteractingIdle;
        private const PlayerStateManager.PlayerState interactingMove = PlayerStateManager.PlayerState.InteractingMove;

        public bool IsGrounded
        {
            get { return isGrounded; }
        }

        // Start is called before the first frame update
        void Start()
        {
            playerCollider = GetComponent<CapsuleCollider>();
            if ( playerCollider == null )
            {
                Debug.LogError($"The component of type {typeof(CapsuleCollider).Name} couldn't be found on the " + gameObject.name + "!");
            }

            GetInstances();

            groundLayer = GetComponent<PlayerController>().GroundLayer;

            rayLength = (playerCollider.height / 2.0f) + GetComponent<PlayerController>().GroundCheckLength;
            rayOriginOffset = playerCollider.radius * colliderRadiusMultiplier;
            originHeightOffset.Set(0.0f, (playerCollider.height / 2.0f), 0.0f);

            if ( !Grounded() )
            {
                psm.SetPlayerState(inAir);
            }
            else if ( Grounded() )
            {
                psm.ResetPlayerState();
            }
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

            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError(gameObject.name + " couldn't find an Instance of PlayerBase!");
            }
        }

        // Update is called once per frame
        void Update()
        {
            isGrounded = Grounded();

            if ( groundCheckChanged != Grounded() )
            {
                groundCheckChanged = !groundCheckChanged;

                if ( psm.CurrentPlayerState == interactingIdle
                    || psm.CurrentPlayerState == interactingMove )
                {
                    return;
                }

                if ( !Grounded() )
                {
                    psm.SetPlayerState(inAir);
                    playerBase.AreAbilitiesEnabled = false;                    
                }
                else if ( Grounded() )
                {
                    psm.ResetPlayerState();
                    playerBase.AreAbilitiesEnabled = true;
                }
            }
        }

        /// <summary>
        /// Uses four separate Raycasts to track if the player is standing on something or not.
        /// The Raycasts are set to originate from within the players colliders radius.
        /// </summary>
        /// <returns>True if any of the rays hit an object on the groundLayer, false if not.</returns>
        private bool Grounded()
        {
            CalculateGroundCheckRayOriginPoints();

            GroundCheckRay(0, frontRayOrigin);
            GroundCheckRay(1, backRayOrigin);
            GroundCheckRay(2, rightRayOrigin);
            GroundCheckRay(3, leftRayOrigin);

            foreach ( bool groundCheckRay in rays )
            {
                if ( groundCheckRay )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Method used to calculate and update the origin points of the Raycasts
        /// used for the GroundCheck method.
        /// </summary>
        private void CalculateGroundCheckRayOriginPoints()
        {
            frontRayOrigin = transform.position + originHeightOffset + transform.forward * rayOriginOffset;
            backRayOrigin = transform.position + originHeightOffset - transform.forward * rayOriginOffset;
            rightRayOrigin = transform.position + originHeightOffset + transform.right * rayOriginOffset;
            leftRayOrigin = transform.position + originHeightOffset - transform.right * rayOriginOffset;
        }

        /// <summary>
        /// Method that makes a Raycast with given parameters. (Used with an array for
        /// several Raycasts)
        /// </summary>
        /// <param name="index">The index of the Raycast used to store the value of
        /// the Raycast.</param>
        /// <param name="position">The position the Raycast originates from.</param>
        private void GroundCheckRay(int index, Vector3 position)
        {
            if ( !isDrawingRays )
            {
                rays[index] = UnityEngine.Physics.Raycast(position, Vector3.down, rayLength, groundLayer);
            }
            else
            {
                // Can be used to debug and draw the Raycast(s) using the RotaryHeart
                // Physics debug library.
                rays[index] = RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(position, Vector3.down, rayLength, groundLayer, PreviewCondition.Editor, 0, Color.green, Color.red);
            }
        }
    }
}
