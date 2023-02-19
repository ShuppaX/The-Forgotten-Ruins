using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public class MovableBox : Interactable
    {
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            if ( rb == null )
            {
                Debug.LogError(gameObject + " is missing a Rigidbody component!");
            }
        }

        // TODO: Move box by one unit per push?

        internal override void Interact()
        {
            base.Interact();

            // Enable movement controls, so the player could move itself and a box
            PlayerBase.Instance.IsMovable = true;

            rb.isKinematic = false;
            //IsInteracting = true;
        }

        internal override void InteractCompleted()
        {
            base.InteractCompleted();

            rb.isKinematic = true;
            //IsInteracting = false;
        }
    }
}
