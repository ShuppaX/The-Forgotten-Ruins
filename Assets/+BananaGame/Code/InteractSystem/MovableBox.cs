using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public class MovableBox : Interactable
    {
        // NOTE: this is probably unnecessary. Let it be here for now just for testing purposes.
        protected override void Start()
        {
            base.Start();
        }

        protected internal override void Interact()
        {
            Debug.Log("MovableBox Interact");

            // TODO: Release controls, so the player is able to move box.

            // TODO: Move box only by X or Z axis depending where the player is facing.
            // TODO: Turn isKinematic off
            // TODO: Turn off X or Z constrain 
        }

        protected internal override void InteractCompleted()
        {
            Debug.Log("MovableBox InteractCompleted");
        }
    }
}
