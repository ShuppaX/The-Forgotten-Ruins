using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public abstract class Interactable : MonoBehaviour
    {
        //public Interactable()
        //{
        //    Debug.Log("Interactable - Constructor called");
        //}

        protected virtual void Start()
        {
            Debug.Log("Interactable - Start called");
        }

        protected internal abstract void Interact();
    }
}
