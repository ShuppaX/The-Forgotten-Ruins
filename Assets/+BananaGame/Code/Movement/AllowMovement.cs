using UnityEngine;

namespace BananaSoup
{
    public class AllowMovement : MonoBehaviour
    {
        private float rayLength = 0.0f;

        private LayerMask groundLayer;

        private RaycastHit slopeHit;

        private void Start()
        {
            rayLength = GetComponent<PlayerController>().RayLength;
            groundLayer = GetComponent<PlayerController>().GroundLayer;
        }

        public bool CanMove(float maxAngle)
        {
            if ( Physics.Raycast(transform.position, Vector3.down, out slopeHit, rayLength, groundLayer) )
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
