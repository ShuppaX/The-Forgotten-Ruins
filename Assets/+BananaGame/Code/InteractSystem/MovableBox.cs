using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public class MovableBox : Interactable
    {
        //[SerializeField] private InteractPoint[] interactionPoints;

        protected internal override void Interact()
        {
            Debug.Log("MovableBox");
        }

        // NOTE: this is probably unnecessary. Let it be here for now just for testing purposes.
        protected override void Start()
        {
            base.Start();
        }
    }
}
