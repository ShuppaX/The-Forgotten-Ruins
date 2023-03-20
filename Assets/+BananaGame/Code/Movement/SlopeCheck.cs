using UnityEngine;

namespace BananaSoup
{
    public class SlopeCheck : MonoBehaviour
    {
        private float rayLengthOffset = 0.0f;
        private float rayLength = 0.0f;
        private float maxAngle = 0.0f;

        private bool isOnSlope = false;

        private Vector3 originHeightOffset = Vector3.zero;

        private LayerMask groundLayer;

        private RaycastHit slopeHit;

        private CapsuleCollider playerCollider;

        public bool IsOnSlope
        {
            get { return isOnSlope; }
        }

        private void Start()
        {
            playerCollider = GetComponent<CapsuleCollider>();

            rayLengthOffset = GetComponent<PlayerController>().RayLength;
            maxAngle = GetComponent<PlayerController>().MaxSlopeAngle;
            groundLayer = GetComponent<PlayerController>().GroundLayer;
            rayLength = playerCollider.height + rayLengthOffset;
            originHeightOffset.Set(0.0f, (playerCollider.height / 2.0f), 0.0f);
        }

        private void Update()
        {
            isOnSlope = OnSlope();
        }

        /// <summary>
        /// Method used to check if the player is on a slope. The method checks the
        /// angle between Vector3.up and a RaycastHits normal below the player and then
        /// if the angle is less than the maximum allowed and that the angle is not 0.
        /// </summary>
        /// <returns>True if the player is on a slope, otherwise false.</returns>
        public bool OnSlope()
        {
            if ( Physics.Raycast(transform.position + originHeightOffset, Vector3.down, out slopeHit, rayLength, groundLayer) )
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                bool angleLessThanMaxSlopeAngle = (angle < maxAngle);
                bool angleNotZero = (angle != 0.0f);

                if ( angleLessThanMaxSlopeAngle && angleNotZero )
                {
                    return true;
                }
            }

            return false;
        }
    }
}
