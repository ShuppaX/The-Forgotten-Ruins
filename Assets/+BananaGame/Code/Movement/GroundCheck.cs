using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine;
using UnityEngine.Events;

namespace BananaSoup
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

        private CapsuleCollider playerCollider;

        [Header("UnityActions to manage PlayerStates")]
        public UnityAction onPlayerGrounded;
        public UnityAction onPlayerInAir;

        public bool IsGrounded
        {
            get { return isGrounded; }
        }

        // Start is called before the first frame update
        void Start()
        {
            playerCollider = GetComponent<CapsuleCollider>();
            groundLayer = GetComponent<PlayerController>().GroundLayer;

            rayLength = (playerCollider.height / 2.0f) + GetComponent<PlayerController>().RayLength;
            rayOriginOffset = playerCollider.radius * colliderRadiusMultiplier;
            originHeightOffset.Set(0.0f, (playerCollider.height / 2.0f), 0.0f);
        }

        // Update is called once per frame
        void Update()
        {
            isGrounded = Grounded();

            if ( groundCheckChanged != Grounded() )
            {
                groundCheckChanged = !groundCheckChanged;
                onPlayerGrounded.Invoke();
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

            onPlayerInAir.Invoke();
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
