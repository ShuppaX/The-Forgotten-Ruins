using UnityEngine;

namespace BananaSoup
{
    public class CalculateMovementDirection : MonoBehaviour
    {
        /// <summary>
        /// Method used to calculate correct movement direction based on the plane the
        /// gameObject is on.
        /// </summary>
        /// <param name="hit">RaycastHit, which is used to store the information of the hit.</param>
        /// <param name="rayLength">The wanted length of the Raycast used to check the ground.</param>
        /// <param name="rayLengthOffset">The wanted offset to lengthen the Raycast.</param>
        /// <param name="direction">The base movement direction (usually movement input).</param>
        /// <returns>The calculated movement direction as a Vector3.</returns>
        public Vector3 CalculateDirection(RaycastHit hit, float rayLength, float rayLengthOffset, Vector3 direction)
        {
            Physics.Raycast(transform.position, Vector3.down, out hit, rayLength + rayLengthOffset);
            return Vector3.ProjectOnPlane(direction, hit.normal).normalized;
        }
    }
}