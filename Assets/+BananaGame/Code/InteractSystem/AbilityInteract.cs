using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup.InteractSystem
{
    [RequireComponent(typeof(PlayerBase))]
    public class AbilityInteract : MonoBehaviour
    {
        [SerializeField] private float maxInteractDistance = 1.0f;
        [SerializeField] private float sphereRadius = 1.0f;
        [SerializeField] private LayerMask interactableLayers;
        [Tooltip("The speed that player character will travel to the InteractPoint when interacted with an Interactable.")]
        [SerializeField] float moveSpeed = 2.0f;

        private PlayerBase playerBase;
        private bool isInteracting;
        private Vector3 interactPoint;

        // Gizmo
        private float currentHitDistance;
        private Color interactGizmoColor = Color.green;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            playerBase = GetComponent<PlayerBase>();
            if ( playerBase == null )
            {
                Debug.LogError("A PlayerBase couldn't be found on the " + gameObject + "!");
            }
        }

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
                playerBase.IsControllable = true;
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
                interactable.Interact();

                InteractPoint closestPoint = interactable.GetClosestInteractPointToPlayer(transform.position);

                isInteracting = true;
                interactPoint = closestPoint.Position;
                playerBase.IsControllable = false;
            }
        }

        private void FixedUpdate()
        {
            if ( isInteracting )
            {
                MoveToInteractPoint();
            }
        }

        private void MoveToInteractPoint()
        {
            // TODO: Turn player towards to the interact point.

            // Move our position a step closer to the target.
            var step = moveSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, interactPoint, step);

            // Set target Y to same as players Y so player won't be trying to go up or downwards.
            interactPoint.y = transform.position.y;

            // Check if the position of the player and Interactable are approximately equal.
            if ( Vector3.Distance(transform.position, interactPoint) < 0.01f )
            {
                isInteracting = false;
                playerBase.IsControllable = true;

                // TODO: Turn player's face towards to the Interactable.
                // TODO: Add 2 IK points for the InteractPoint and move player's hands towards them (IK).
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = interactGizmoColor;
            Gizmos.DrawWireSphere(transform.position + transform.forward * currentHitDistance, sphereRadius);
        }
    }
}
