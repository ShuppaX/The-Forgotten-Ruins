using UnityEngine;

namespace BananaSoup
{
    public class SlopeCheck : MonoBehaviour
    {
        private float slopeCheckRayLengthOffset = 0.0f;
        private float slopeCheckRayLength = 0.0f;
        private float maxAngle = 0.0f;

        private LayerMask groundLayer;

        private RaycastHit slopeHit;

        private void Start()
        {
            slopeCheckRayLengthOffset = GetComponent<PlayerController>().RayLength;
            maxAngle = GetComponent<PlayerController>().MaxSlopeAngle;
            groundLayer = GetComponent<PlayerController>().GroundLayer;
            slopeCheckRayLength = (transform.localScale.y / 2) + slopeCheckRayLengthOffset;
        }

        public bool OnSlope()
        {
            if ( Physics.Raycast(transform.position, Vector3.down, out slopeHit, slopeCheckRayLength, groundLayer) )
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                bool angleLessThanMaxSlopeAngle = (angle < maxAngle);
                bool angleNotZero = (angle != 0);

                if ( angleLessThanMaxSlopeAngle && angleNotZero )
                {
                    return true;
                }
            }

            return false;
        }
    }
}
