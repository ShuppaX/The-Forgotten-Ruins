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

        /// <summary>
        /// Method used to check if the player can move on the current ground they're
        /// standing on.
        /// </summary>
        /// <param name="maxAngle">The maximum allowed slope angle.</param>
        /// <returns>True if the player can move on the ground they're on, otherwise false.</returns>
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
