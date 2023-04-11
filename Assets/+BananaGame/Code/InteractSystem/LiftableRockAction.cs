using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public class LiftableRockAction : Interactable
    {
        private PlayerBase playerBase = null;
        private Rigidbody rb = null;
        private Collider col = null;
        private LiftableRockLiftPoint liftPoint = null;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if ( rb == null )
            {
                Debug.LogError(gameObject.name + " is missing a Rigidbody component!");
            }

            col = GetComponent<Collider>();
            if ( col == null )
            {
                Debug.LogError(gameObject.name + " is missing a Collider component!");
            }

            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError(gameObject.name + " couldn't find an instance of PlayerBase!");
            }

            liftPoint = playerBase.GetComponentInChildren<LiftableRockLiftPoint>();
            if ( liftPoint == null )
            {
                Debug.LogError(gameObject.name + " couldn't find a LiftableRockLiftPoint from PlayerBase's children!");
            }

            if ( !col.enabled )
            {
                col.enabled = true;
            }
        }

        internal override void Interact()
        {
            base.Interact();

            // Enable movement controls
            playerBase.IsMovable = true;

            // Enable character turning
            playerBase.IsTurnable = true;

            // Disable rocks collider
            ToggleRockCollider();
        }

        internal override void InteractCompleted()
        {
            base.InteractCompleted();

            ToggleRockCollider();
        }

        private void ToggleRockCollider()
        {
            col.enabled = !col.enabled;
        }

        private void FixedUpdate()
        {
            if ( IsInteracting )
            {
                transform.position = liftPoint.transform.position;
            }
        }
    }
}
