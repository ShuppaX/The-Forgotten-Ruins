using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class MovableBox : Interactable
    {
        protected internal override void Interact()
        {
            Debug.Log("MovableBox");
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}
