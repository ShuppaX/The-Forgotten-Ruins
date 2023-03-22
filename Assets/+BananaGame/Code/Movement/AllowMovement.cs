using UnityEngine;
using RotaryHeart.Lib.PhysicsExtension;

namespace BananaSoup
{
    public class AllowMovement : MonoBehaviour
    {
        [SerializeField]
        private bool isDrawingRay = false;

        private float rayLength = 5.0f;
        private float maxAngle = 0.0f;

        private bool isMovementAllowed = false;

        private LayerMask groundLayer;

        private RaycastHit slopeHit;

        public bool IsMovementAllowed
        {
            get { return isMovementAllowed; }
        }

        private void Start()
        {
            maxAngle = GetComponent<PlayerController>().MaxSlopeAngle;
            groundLayer = GetComponent<PlayerController>().GroundLayer;
        }

        private void Update()
        {
            isMovementAllowed = CanMove();
        }

        /// <summary>
        /// Method used to check if the player can move on the current ground they're
        /// standing on.
        /// </summary>
        /// <returns>True if the player can move on the ground they're on, otherwise false.</returns>
        private bool CanMove()
        {
            bool ray;

            if ( isDrawingRay )
            {
                ray = RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(transform.position, Vector3.down, out slopeHit, rayLength, groundLayer, PreviewCondition.Editor, 0, Color.green, Color.red);
            }
            else
            {
                ray = UnityEngine.Physics.Raycast(transform.position, Vector3.down, out slopeHit, rayLength, groundLayer);
            }

            if ( ray )
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                bool angleLessThanMaxSlopeAngle = (angle < maxAngle);

                if ( angleLessThanMaxSlopeAngle )
                {
                    return true;
                }
            }

            return false;
        }
    }
}
