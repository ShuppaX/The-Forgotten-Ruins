using UnityEngine;

namespace BananaSoup
{
    public class AllowMovement : MonoBehaviour
    {
        private float allowMovementRayLengthOffset = 0.0f;
        private float allowMovementRayLength = 0.0f;

        private LayerMask groundLayer;

        private RaycastHit slopeHit;

        private void Start()
        {
            allowMovementRayLengthOffset = GetComponent<PlayerController>().CheckRayLengthOffset;
            groundLayer = GetComponent<PlayerController>().GroundLayer;
            allowMovementRayLength = (transform.localScale.y / 2) + allowMovementRayLengthOffset;
        }

        public bool CanMove(float maxAngle)
        {
            if ( Physics.Raycast(transform.position, Vector3.down, out slopeHit, allowMovementRayLength, groundLayer) )
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
