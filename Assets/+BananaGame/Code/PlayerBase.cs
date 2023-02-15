using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class PlayerBase : MonoBehaviour
    {
        private PlayerInput playerInput;
        protected InputAction interactAction;
        private bool isControllable = true;

        public bool IsControllable
        {
            get { return isControllable; }
            set { isControllable = value; }
        }

        private void Awake()
        {
            Setup();
        }

        /// <summary>
        /// Stores the players input to moveInput and enables the playerInput if the gameObject is enabled.
        /// </summary>
        private void OnEnable()
        {
            playerInput.Player.Enable();
        }

        /// <summary>
        /// Disables the playerInput if the game object is disabled.
        /// </summary>
        private void OnDisable()
        {
            playerInput.Player.Disable();
        }

        private void Setup()
        {
            playerInput = new PlayerInput();

            // NOTE: Do we need this? Is this unnecessary?
            interactAction = playerInput.Player.Interact;
        }
    }
}
