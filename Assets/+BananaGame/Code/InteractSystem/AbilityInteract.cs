using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup.InteractSystem
{
    public class AbilityInteract : PlayerBase
    {
        [SerializeField] private float maxInteractDistance = 1.0f;
        [SerializeField] private float sphereRadius = 1.0f;
        [SerializeField] private LayerMask interactableLayers;

        private float currentHitDistance;
        private Color interactGizmoColor = Color.green;

        private bool isInteracting;
        private Vector3 target;

        public void OnInteract(InputAction.CallbackContext context)
        {
            // Check did user performed Interact Ability.
            // If not, return.
            if ( !context.performed )
            {
                return;
            }

            // If already interacting with an Interactable, cancel on going interact.
            if ( isInteracting )
            {
                isInteracting = false;
                IsControllable = true;
                return;
            }

            // Check are there any Interactables in the range of the player.
            // If not, return.
            RaycastHit hit;
            if ( !Physics.SphereCast(transform.position, sphereRadius, transform.forward, out hit, maxInteractDistance, interactableLayers) )
            {
                currentHitDistance = maxInteractDistance;
                interactGizmoColor = Color.green;
                return;
            }

            // An Interactable is in the range.
            currentHitDistance = hit.distance;
            interactGizmoColor = Color.red;

            if ( hit.transform.TryGetComponent(out Interactable interactable) )
            {
                //Debug.Log("Interacted with: " + interactable.transform.name);
                interactable.Interact();

                var closestPoint = interactable.GetClosestInteractPointToPlayer(transform.position);
                //Debug.Log("Closest interact point: " + closestPoint);

                isInteracting = true;
                target = closestPoint.Position;
            }
        }

        private void FixedUpdate()
        {
            if ( isInteracting )
            {
                IsControllable = false;
                MoveToInteractPoint();
            }
        }

        private void MoveToInteractPoint()
        {
            // TODO: Turn player towards to the interact point.

            float speed = 2.0f;

            // Move our position a step closer to the target.
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            // Set target Y to same as players Y so player won't be trying to go up or downwards.
            target.y = transform.position.y;
            // Check if the position of the cube and sphere are approximately equal.
            if ( Vector3.Distance(transform.position, target) < 0.001f )
            {
                Debug.Log("Moved to interaction point");
                // Swap the position of the cylinder.
                //target *= -1.0f;
                isInteracting = false;

                // TODO: Turn player's face towards to the Interactable.
                // TODO: Move player's hands to on the Interactable (IK).
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = interactGizmoColor;
            Gizmos.DrawWireSphere(transform.position + transform.forward * currentHitDistance, sphereRadius);
        }
    }
}
