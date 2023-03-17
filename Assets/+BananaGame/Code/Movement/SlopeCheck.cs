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

        /// <summary>
        /// Method used to check if the player is on a slope. The method checks the
        /// angle between Vector3.up and a RaycastHits normal below the player and then
        /// if the angle is less than the maximum allowed and that the angle is not 0.
        /// </summary>
        /// <returns>True if the player is on a slope, otherwise false.</returns>
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
