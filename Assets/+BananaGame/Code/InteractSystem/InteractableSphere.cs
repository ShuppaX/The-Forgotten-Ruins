using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public class InteractableSphere : Interactable
    {
        protected internal override void Interact()
        {
            Debug.Log("InteractableSphere");
        }
    }
}
