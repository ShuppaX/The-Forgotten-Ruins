using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class Interact : PlayerBase
    {
        public void OnInteract(InputAction.CallbackContext context)
        {
            if ( context.performed )
            {
                Debug.Log("Interacted");
            }
        }
    }
}
