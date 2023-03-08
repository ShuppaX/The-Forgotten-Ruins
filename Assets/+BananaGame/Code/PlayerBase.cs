using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class PlayerBase : MonoBehaviour
    {
        public static PlayerBase Instance { get; private set; }
        private PlayerInput playerInput;
        private bool areAbilitiesEnabled = true;
        private bool isMovable = true;
        private bool isTurnable = true;

        [SerializeField]
        private TMP_Text playerStateText;

        [HideInInspector]
        public PlayerState playerState = 0;

        public enum PlayerState
        {
            Idle        = 0, // Default state
            Moving      = 1,
            Dashing     = 2,
            Attacking   = 3,
            Interacting = 4,
            InAir       = 5
        }

        public bool AreAbilitiesEnabled
        {
            get { return areAbilitiesEnabled; }
            set { areAbilitiesEnabled = value; }
        }

        public bool IsMovable
        {
            get { return isMovable; }
            set { isMovable = value; }
        }

        public bool IsTurnable
        {
            get { return isTurnable; }
            set { isTurnable = value; }
        }

        private void Awake()
        {
            if ( Instance == null )
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            Setup();
        }

        private void Update()
        {
            playerStateText.SetText(playerState.ToString());
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
        }
    }
}
