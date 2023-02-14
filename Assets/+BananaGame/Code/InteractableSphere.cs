using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class InteractableSphere : Interactable
    {
        protected internal override void Interact()
        {
            Debug.Log("InteractableSphere");
        }
    }
}
