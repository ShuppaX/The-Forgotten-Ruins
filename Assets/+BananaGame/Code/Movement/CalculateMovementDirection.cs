using UnityEngine;

namespace BananaSoup
{
    public class CalculateMovementDirection : MonoBehaviour
    {
        [SerializeField]
        private bool isDrawingRaycast = false;

        private float rayLength = 10.0f;

        private RaycastHit calculatorHit;
        private Vector3 calculatedDirection;

        /// <summary>
        /// Method used to calculate correct movement direction based on the plane the
        /// gameObject is on.
        /// </summary>
        /// <param name="direction">The base movement direction (usually movement input).</param>
        /// <returns>The calculated movement direction as a Vector3.</returns>
        public Vector3 CalculateDirection(Vector3 direction)
        {
            Physics.Raycast(transform.position, Vector3.down, out calculatorHit, rayLength);
            calculatedDirection = Vector3.ProjectOnPlane(direction, calculatorHit.normal);
            return calculatedDirection;
        }

        private void OnDrawGizmos()
        {
            if (isDrawingRaycast)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(calculatorHit.point, calculatedDirection);
            }
        }
    }
}
