using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public class LiftableRockAction : Interactable
    {
        private PlayerBase playerBase = null;
        private BoxCollider col = null;
        private LiftableRockLiftPoint liftPoint = null;
        private Vector3 colliderOriginalScales;
        private Vector3 colliderShrinkedScales = new Vector3(0.05f, 0.05f, 0.05f);

        void Start()
        {
            GetReferences();

            colliderOriginalScales = col.size;
        }

        private void GetReferences()
        {
            col = GetComponent<BoxCollider>();
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
        }

        internal override void Interact()
        {
            base.Interact();

            // Enable movement controls
            playerBase.IsMovable = true;

            // Enable character turning
            playerBase.IsTurnable = true;

            // Scale rock's collider
            ScaleRockCollider();
        }

        internal override void InteractCompleted()
        {
            base.InteractCompleted();

            ScaleRockCollider();
        }

        private void ScaleRockCollider()
        {
            if ( col.size == colliderOriginalScales )
            {
                col.size = colliderShrinkedScales;
            }
            else
            {
                col.size = colliderOriginalScales;
            }
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
