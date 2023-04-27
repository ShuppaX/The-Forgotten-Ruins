using BananaSoup.Managers;
using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine;

namespace BananaSoup.Utilities
{
    public class GroundAhead : MonoBehaviour
    {
        [SerializeField]
        private bool isDrawingSphereCasts = false;

        private float castLengthMultiplier = 2.0f;

        // Default collision radius multiplier and cast origin offset.
        private float colRadMultiplier = 1.5f;
        private float castOriginOffsetDefault = 0.0f;

        // Collision radius multiplier and cast origin offset while interacting
        private float colRadMultiplierOnInteract = 2.25f;
        private float castOriginOffsetInteract = 0.0f;

        private float currentCastOriginOffset = 0.0f;
        private float castLength = 0.0f;

        private bool[] spheres = new bool[3];

        // Vector3 variables to store origin points of sphere casts.
        private Vector3 leftOrigin = Vector3.zero;
        private Vector3 centerOrigin = Vector3.zero;
        private Vector3 rightOrigin = Vector3.zero;

        private Vector3 originHeightOffset = Vector3.zero;

        private float sphereRadius = 0.025f;

        private bool isGroundAhead = false;

        private LayerMask groundLayer;

        // References
        private CapsuleCollider playerCollider = null;
        private PlayerStateManager psm = null;

        // Constant PlayerState used while checking PlayerState
        private const PlayerStateManager.PlayerState interactingIdle = PlayerStateManager.PlayerState.InteractingIdle;
        private const PlayerStateManager.PlayerState interactingMove = PlayerStateManager.PlayerState.InteractingMove;

        public bool IsGroundAhead
        {
            get { return isGroundAhead; }
        }

        // Start is called before the first frame update
        void Start()
        {
            playerCollider = GetComponent<CapsuleCollider>();
            if ( playerCollider == null )
            {
                Debug.LogError($"The component of type {typeof(CapsuleCollider).Name} couldn't be found on the " + gameObject.name + "!");
            }

            psm = PlayerStateManager.Instance;
            if ( psm == null )
            {
                Debug.LogError($"An instance of a component of type {typeof(PlayerStateManager).Name} couldn't be found for the " + gameObject.name + "!");
            }

            groundLayer = GetComponent<PlayerController>().GroundLayer;

            castOriginOffsetDefault = playerCollider.radius * colRadMultiplier;
            castOriginOffsetInteract = playerCollider.radius * colRadMultiplierOnInteract;
            castLength = (transform.localScale.y * castLengthMultiplier);
            originHeightOffset.Set(0.0f, (playerCollider.height / 2.0f), 0.0f);
        }

        private void Update()
        {
            isGroundAhead = CheckIfGroundAhead();
        }

        /// <summary>
        /// Method used to check if there is ground ahead of the player. Use this before
        /// a moving method to prevent the player from moving forward if there is no ground
        /// in front of the player.
        /// </summary>
        /// <returns>False if any of the SphereCasts doesn't hit anything on the groundLayer,
        /// True otherwise.</returns>
        private bool CheckIfGroundAhead()
        {
            CalculateGroundAheadSphereOriginPoints();

            GroundAheadSphereCast(0, leftOrigin);
            GroundAheadSphereCast(1, centerOrigin);
            GroundAheadSphereCast(2, rightOrigin);

            foreach ( bool groundAheadSphere in spheres )
            {
                if ( !groundAheadSphere )
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Method used to set and calculate the origin points of the sphere casts.
        /// The 0.71f is used to get the correct value for the cornering origin points
        /// since the player has a circular / capsulecollider, so that they aren't too far
        /// away from the player.
        /// </summary>
        private void CalculateGroundAheadSphereOriginPoints()
        {
            if ( psm.CurrentPlayerState == interactingIdle
                || psm.CurrentPlayerState == interactingMove )
            {
                currentCastOriginOffset = castOriginOffsetInteract;
            }
            else
            {
                currentCastOriginOffset = castOriginOffsetDefault;
            }

            Debug.Log("currentCastOriginOffset = " + currentCastOriginOffset);

            leftOrigin = (transform.position + originHeightOffset)
                + (transform.forward * currentCastOriginOffset) * 0.71f
                - (transform.right * currentCastOriginOffset) * 0.71f;
            centerOrigin = (transform.position + originHeightOffset)
                + transform.forward * currentCastOriginOffset;
            rightOrigin = (transform.position + originHeightOffset)
                + (transform.forward * currentCastOriginOffset) * 0.71f
                + (transform.right * currentCastOriginOffset) * 0.71f;
        }

        /// <summary>
        /// Method used to make the creation of the SphereCasts from an array easier.
        /// The method creates the SphereCasts using two parameters, the index and the
        /// position/originpoint of the SphereCast.
        /// </summary>
        /// <param name="index">The index of the SphereCast.</param>
        /// <param name="origin">The origin point of the SphereCast.</param>
        private void GroundAheadSphereCast(int index, Vector3 origin)
        {
            if ( !isDrawingSphereCasts )
            {
                RaycastHit hit;
                spheres[index] = UnityEngine.Physics.SphereCast(origin, sphereRadius, Vector3.down, out hit, castLength, groundLayer);
            }
            else
            {
                // Can be used to debug and draw the Raycast(s) using the RotaryHeart
                // Physics debug library.
                spheres[index] = RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(origin, sphereRadius, Vector3.down, castLength, groundLayer, PreviewCondition.Editor, 0, Color.green, Color.red);
            }
        }
    }
}
