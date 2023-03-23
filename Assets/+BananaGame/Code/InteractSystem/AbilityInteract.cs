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

        private bool hasSelectedInteractable;
        private bool isLookingAtTarget = true;

        private Vector3 interactPoint;

        private PlayerBase playerBase = null;
        private Interactable currentInteractable = null;
        private PlayerStateManager psm = null;
        private DebugManager debug = null;

        [Header("Constant strings used for PlayerState handling")]
        public const string interacting = "Interacting";
        public const string interactOver = "Idle";

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
            debug = DebugManager.Instance;

            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError("A PlayerBase couldn't be found on the " + gameObject.name + "!");
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
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

            // If the player has already selected an Interactable and moving towards it,
            // cancel the movement and don't select a new Interactable target.
            if ( hasSelectedInteractable )
            {
                hasSelectedInteractable = false;
                psm.SetPlayerState(interactOver);
                debug.UpdatePlayerStateText();
                SetPlayerInputs(true);
                return;
            }

            // If the player is already interacting with an Interactable, cancel on going interact and don't start a new one.
            if ( currentInteractable != null )
            {
                if ( currentInteractable.IsInteracting == true )
                {
                    psm.SetPlayerState(interactOver);
                    debug.UpdatePlayerStateText();
                    currentInteractable.InteractCompleted();
                    SetPlayerInputs(true);
                    return;
                }
            }

            // TODO: Does this need both raycast AND TryGetComponent?

            // Check are there any Interactables in the range of the player.
            // If not, return.
            RaycastHit hit;
            if ( !Physics.SphereCast(transform.position, sphereRadius, transform.forward, out hit, maxInteractDistance, interactableLayers) )
            {
                currentHitDistance = maxInteractDistance;
                interactGizmoColor = Color.green;
                return;
            }

            // An Interactable is in the range. Set variables for the gizmo
            currentHitDistance = hit.distance;
            interactGizmoColor = Color.red;

            // Check does the physical object have an Interactable component 
            if ( hit.transform.TryGetComponent(out Interactable interactable) )
            {
                SetPlayerInputs(false);
                psm.SetPlayerState(interacting);
                debug.UpdatePlayerStateText();
                hasSelectedInteractable = true;

                currentInteractable = interactable;
                InteractPoint closestPoint = interactable.GetClosestInteractPointToPlayer(transform.position);
                interactPoint = closestPoint.Position;
            }
        }

        private void SetPlayerInputs(bool value)
        {
            playerBase.AreAbilitiesEnabled = value;
            playerBase.IsTurnable = value;
            playerBase.IsMovable = value;
        }

        private void MoveToInteractPoint()
        {
            // TODO: Turn player towards to the interact point. By code or navmesh?

            // Move our position a step closer to the target.
            var step = moveSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, interactPoint, step);

            // Set target Y to same as players Y so player won't be trying to go up or downwards.
            interactPoint.y = transform.position.y;

            // Check if the position of the player and Interactable are approximately equal.
            if ( Vector3.Distance(transform.position, interactPoint) < 0.01f )
            {
                hasSelectedInteractable = false;
                currentInteractable.Interact();

                // Set bool that the player is not looking at the target Interactable. FixedUpdate will rotate player towards it.
                isLookingAtTarget = false;

                // TODO: Add 2 IK points for the InteractPoint and move player's hands towards them (IK).
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

        // TODO: Remove currentInteractable when interaction completed?

        private void OnDrawGizmos()
        {
            Gizmos.color = interactGizmoColor;
            Gizmos.DrawWireSphere(transform.position + transform.forward * currentHitDistance, sphereRadius);
        }
    }
}
