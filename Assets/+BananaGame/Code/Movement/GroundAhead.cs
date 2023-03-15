using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine;

namespace BananaSoup
{
    public class GroundAhead : MonoBehaviour
    {
        private float groundAheadRadiusMultiplier = 1.5f;
        private float groundAheadOriginHeight = 0.5f;
        private float groundAheadRayLengthMultiplier = 2.0f;
        private float groundAheadSphereOffset = 0.0f;
        private float groundAheadRayLength = 0.0f;

        private bool[] groundAheadSpheres = new bool[3];

        private Vector3 groundAheadLeftOrigin = Vector3.zero;
        private Vector3 groundAheadCenterOrigin = Vector3.zero;
        private Vector3 groundAheadRightOrigin = Vector3.zero;

        private Vector3 groundAheadOriginHeightOffset = Vector3.zero;

        private float groundAheadSphereRadius = 0.025f;

        private LayerMask groundLayer;

        [SerializeField]
        private bool isDrawingSphereCasts = false;

        private CapsuleCollider playerCollider = null;

        public bool IsGroundAhead
        {
            get { return CheckIfGroundAhead(); }
        }

        // Start is called before the first frame update
        void Start()
        {
            playerCollider = GetComponent<CapsuleCollider>();
            groundLayer = GetComponent<PlayerController>().GroundLayer;

            groundAheadSphereOffset = playerCollider.radius * groundAheadRadiusMultiplier;
            groundAheadRayLength = (transform.localScale.y * groundAheadRayLengthMultiplier);
            groundAheadOriginHeightOffset.Set(0, groundAheadOriginHeight, 0);
        }

        /// <summary>
        /// Method used to check if there is ground ahead of the player. Used in FixedUpdate()
        /// to determine if the player can walk forward or not.
        /// </summary>
        /// <returns>False if any of the SphereCasts don't hit anything on the groundLayer,
        /// True otherwise.</returns>
        private bool CheckIfGroundAhead()
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

            //wasPushed = false;
            return true;
        }

        private void CalculateGroundAheadSphereOriginPoints()
        {
            groundAheadLeftOrigin = (transform.position + groundAheadOriginHeightOffset)
                + (transform.forward * groundAheadSphereOffset) * 0.71f
                - (transform.right * groundAheadSphereOffset) * 0.71f;
            groundAheadCenterOrigin = (transform.position + groundAheadOriginHeightOffset)
                + transform.forward * groundAheadSphereOffset;
            groundAheadRightOrigin = (transform.position + groundAheadOriginHeightOffset)
                + (transform.forward * groundAheadSphereOffset) * 0.71f
                + (transform.right * groundAheadSphereOffset) * 0.71f;
        }

        private void GroundAheadSphereCast(int index, Vector3 position)
        {
            if ( !isDrawingSphereCasts )
            {
                RaycastHit hit;
                groundAheadSpheres[index] = UnityEngine.Physics.SphereCast(position, groundAheadSphereRadius, Vector3.down, out hit, groundAheadRayLength, groundLayer);
            }
            else
            {
                // Can be used to debug and draw the Raycast(s) using the RotaryHeart
                // Physics debug library.
                groundAheadSpheres[index] = RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(position, groundAheadSphereRadius, Vector3.down, groundAheadRayLength, groundLayer, PreviewCondition.Editor, 0, Color.green, Color.red);
            }
        }
    }
}
