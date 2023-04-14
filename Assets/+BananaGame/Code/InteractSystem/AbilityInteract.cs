using UnityEngine;
using UnityEngine.InputSystem;
using BananaSoup.Managers;

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

        [Header("Constant strings used for PlayerState handling")]
        private const PlayerStateManager.PlayerState interacting = PlayerStateManager.PlayerState.Interacting;

        private bool hasSelectedInteractable;
        private bool isLookingAtTarget = true;
        private Vector3 interactPoint;
        private PlayerBase playerBase = null;
        private Interactable currentInteractable = null;
        private PlayerStateManager psm = null;

        // Gizmo
        private float currentHitDistance;
        private Color interactGizmoColor = Color.green;

        private void Start()
        {
            Setup();
        }

        private void FixedUpdate()
        {
            if ( !isLookingAtTarget )
            {
                TurnPlayerTowardsInteractable();
            }

            if ( hasSelectedInteractable )
            {
                MoveToInteractPoint();
            }
        }

        private void Setup()
        {
            psm = PlayerStateManager.Instance;
            if ( psm == null )
            {
                Debug.LogError("A PlayerStateManager couldn't be found on the " + gameObject.name + "!");
            }

            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError("A PlayerBase couldn't be found on the " + gameObject.name + "!");
            }

            // Adding Spheres radius to the interact distance so the distance is calculated from the player's middle point.
            maxInteractDistance += sphereRadius;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            // Check is Interacting enabled.
            // If not, return.
            if ( !PlayerBase.Instance.IsInteractingEnabled )
            {
                return;
            }

            // Check did user performed Interact Ability.
            // If not, return.
            if ( !context.performed )
            {
                return;
            }

            // Cancelling Interaction.
            // If the player has already selected an Interactable and moving towards it,
            // cancel the movement and don't select a new Interactable target.
            if ( hasSelectedInteractable )
            {
                hasSelectedInteractable = false;
                psm.ResetPlayerState();
                SetPlayerInputs(true);
                return;
            }

            // Cancelling Interaction.
            // If the player is already interacting with an Interactable, cancel on going interact and don't start a new one.
            if ( currentInteractable != null )
            {
                if ( currentInteractable.IsInteracting == true )
                {
                    psm.ResetPlayerState();
                    currentInteractable.InteractCompleted();
                    SetPlayerInputs(true);
                    return;
                }
            }

            // Check are there any Interactables in the range of the player.
            Vector3 spheresCastingLocation = (transform.position + (transform.forward * -sphereRadius));
            RaycastHit hit;
            if ( Physics.SphereCast(spheresCastingLocation, sphereRadius, transform.forward, out hit, maxInteractDistance, interactableLayers) )
            {
                // An Interactable is in the range. Set variables for the debug gizmo.
                currentHitDistance = hit.distance;
                interactGizmoColor = Color.red;

                // Check does the physical object have an Interactable component,
                // if it has continue.
                if ( hit.transform.TryGetComponent(out Interactable interactable) )
                {
                    SetPlayerInputs(false);
                    psm.SetPlayerState(interacting);
                    hasSelectedInteractable = true;

                    currentInteractable = interactable;
                    InteractPoint closestPoint = interactable.GetClosestInteractPointToPlayer(transform.position);
                    interactPoint = closestPoint.Position;
                }
            }
            else
            {
                currentHitDistance = maxInteractDistance;
                interactGizmoColor = Color.green;
            }
        }

        private void SetPlayerInputs(bool value)
        {
            playerBase.AreAbilitiesEnabled = value;
            playerBase.IsTurnable = value;
            playerBase.IsMovable = value;
            playerBase.CanDash = value;
        }

        private void MoveToInteractPoint()
        {
            // TODO: Turn player towards to the interact point. By code or navmesh?

            // Set target Y to same as players Y so player won't be trying to go up or downwards.
            interactPoint.y = transform.position.y;

            // Calculate a step to move.
            float step = moveSpeed * Time.deltaTime;

            // Move the player position a step closer to the target.
            transform.position = Vector3.MoveTowards(transform.position, interactPoint, step);

            // Check if the position of the player and Interactable are approximately equal.
            if ( Vector3.Distance(transform.position, interactPoint) < 0.01f )
            {
                hasSelectedInteractable = false;
                currentInteractable.Interact();

                // Set bool that the player is not looking at the target Interactable. FixedUpdate will rotate player towards it.
                isLookingAtTarget = false;
            }
        }

        private void TurnPlayerTowardsInteractable()
        {
            // TODO: Turn the player slowly towards the target, not instantly.
            Vector3 interactable = currentInteractable.transform.position;
            interactable.y = transform.position.y;
            transform.LookAt(interactable);
            isLookingAtTarget = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = interactGizmoColor;

            Vector3 spheresCastingLocation = (transform.position + (transform.forward * -sphereRadius));
            Gizmos.DrawWireSphere(spheresCastingLocation + transform.forward * currentHitDistance, sphereRadius);
        }
    }
}
