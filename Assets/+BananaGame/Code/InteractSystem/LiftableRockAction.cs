using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public class LiftableRockAction : Interactable
    {
        private Vector3 colliderOriginalScales;
        private Vector3 colliderShrinkedScales = new Vector3(0.05f, 0.05f, 0.05f);

        private PlayerBase playerBase = null;
        private BoxCollider col = null;
        private LiftableRockLiftPoint liftPoint = null;
        private LiftableRockDropPoint dropPoint = null;
        private Rigidbody rb = null;

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
                Debug.LogError($"{gameObject.name} is missing a Collider component!");
            }

            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError($"{gameObject.name} couldn't find an instance of PlayerBase!");
            }

            liftPoint = playerBase.GetComponentInChildren<LiftableRockLiftPoint>();
            if ( liftPoint == null )
            {
                Debug.LogError($"{gameObject.name} couldn't find a LiftableRockLiftPoint from PlayerBase's children!");
            }

            dropPoint = playerBase.GetComponentInChildren<LiftableRockDropPoint>();
            if ( dropPoint == null )
            {
                Debug.LogError($"{gameObject.name} couldn't find a LiftableRockDropPoint from PlayerBase's children!");
            }

            rb = GetComponent<Rigidbody>();
            if ( rb == null )
            {
                Debug.LogError($"{gameObject.name} is missing a Rigidbody component!");
            }
        }

        internal override void Interact()
        {
            base.Interact();

            rb.useGravity = false;

            // Scale rock's collider
            ScaleRockCollider();
        }

        internal override void InteractCompleted()
        {
            base.InteractCompleted();

            rb.useGravity = true;

            SetRockToDropPoint();
            ScaleRockCollider();
        }

        public void ScaleRockCollider()
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

        public void SetRockToDropPoint()
        {
            transform.position = dropPoint.DropPointPosition;
        }

        private void FixedUpdate()
        {
            if ( IsInteracting )
            {
                transform.position = liftPoint.transform.position;
                transform.rotation = playerBase.transform.rotation;
            }
        }
    }
}
