using UnityEngine;

namespace BananaSoup
{
    public class AllowMovement : MonoBehaviour
    {
        [SerializeField, Tooltip("The offset added to the AllowMovement Raycast length.")]
        private float allowMovementRayLengthOffset = 0.25f;
        [SerializeField, Tooltip("The groundLayer LayerMask.")]
        private LayerMask groundLayer;

        private float allowMovementRayLength = 0.0f;

        private RaycastHit slopeHit;

        private void Start()
        {
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
