using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class AbilityInteract : PlayerBase
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
