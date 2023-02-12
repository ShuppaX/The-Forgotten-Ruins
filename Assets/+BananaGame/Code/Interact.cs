using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class Interact : PlayerBase
    {
        private void OnInteract(InputAction.CallbackContext context)
        {
            bool isInteract = interactAction.phase == InputActionPhase.Performed;
            Debug.Log("Interact bool: " + isInteract + ", phase: " + interactAction.phase);
        }
    }
}
