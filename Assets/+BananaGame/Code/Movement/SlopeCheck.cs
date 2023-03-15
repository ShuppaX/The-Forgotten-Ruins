using UnityEngine;

namespace BananaSoup
{
    public class SlopeCheck : MonoBehaviour
    {
        [SerializeField, Tooltip("The offset added to the slopeCheck Raycast length.")]
        private float slopeCheckRayLengthOffset = 0.25f;
        [SerializeField, Tooltip("The groundLayer LayerMask.")]
        private LayerMask groundLayer;

        private float slopeCheckRayLength = 0.0f;

        private RaycastHit slopeHit;

        private void Start()
        {
            slopeCheckRayLength = (transform.localScale.y / 2) + slopeCheckRayLengthOffset;
        }

        public bool OnSlope(float maxAngle)
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
