using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public class InteractableSphere : Interactable
    {
        internal override void Interact()
        {
            base.Interact();

            Debug.Log(gameObject.name + " Interact()");
        }

        internal override void InteractCompleted()
        {
            base.InteractCompleted();

            Debug.Log(gameObject.name + " InteractCompleted()");
        }
    }
}
