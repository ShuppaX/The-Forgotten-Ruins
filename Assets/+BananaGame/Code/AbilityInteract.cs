using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class AbilityInteract : PlayerBase
    {
        [SerializeField] private float maxInteractDistance = 1.0f;
        [SerializeField] private float sphereRadius = 1.0f;
        [SerializeField] private LayerMask interactableLayers;

        private float currentHitDistance;
        private Color interactGizmoColor = Color.green;

        public void OnInteract(InputAction.CallbackContext context)
        {
            if ( context.performed )
            {
                RaycastHit hit;
                if(Physics.SphereCast(transform.position, sphereRadius, transform.forward, out hit, maxInteractDistance, interactableLayers) )
                {
                    //Debug.Log("Interacted with: " + hit.transform.name);
                    currentHitDistance = hit.distance;
                    interactGizmoColor = Color.red;

                    if ( hit.transform.TryGetComponent(out Interactable interactable) )
                    {
                        Debug.Log("Interacted with: " + interactable.transform.name);
                        interactable.Interact();
                    }
                }
                else
                {
                    currentHitDistance = maxInteractDistance;
                    interactGizmoColor = Color.green;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = interactGizmoColor;
            Gizmos.DrawWireSphere(transform.position + transform.forward * currentHitDistance, sphereRadius);
        }
    }
}
