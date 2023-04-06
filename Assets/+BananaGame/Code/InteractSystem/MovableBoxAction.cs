using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public class MovableBoxAction : Interactable
    {
        private Rigidbody rb;
        private Vector3 offset;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            if ( rb == null )
            {
                Debug.LogError(gameObject + " is missing a Rigidbody component!");
            }
        }

        internal override void Interact()
        {
            base.Interact();

            // Enable movement controls, so the player could move itself and a box
            PlayerBase.Instance.IsMovable = true;

            // Set offset between a Movable Box and the player.
            offset = transform.position - PlayerBase.Instance.transform.position;
        }

        internal override void InteractCompleted()
        {
            base.InteractCompleted();
        }

        private void FixedUpdate()
        {
            if ( IsInteracting )
            {
                rb.MovePosition(PlayerBase.Instance.transform.position + offset);
            }
        }
    }
}
