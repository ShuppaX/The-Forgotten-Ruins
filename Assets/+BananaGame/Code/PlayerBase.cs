using UnityEngine;

namespace BananaSoup
{
    public class PlayerBase : MonoBehaviour
    {
        public static PlayerBase Instance { get; private set; }
        private PlayerInput playerInput;
        private bool areAbilitiesEnabled = true;
        private bool isInteractingEnabled = true;
        private bool isMovable = true;
        private bool isTurnable = true;
        private bool canDash = true;
        private bool isSwordLooted = false;

        public bool AreAbilitiesEnabled
        {
            get { return areAbilitiesEnabled; }
            set { areAbilitiesEnabled = value; }
        }

        public bool IsInteractingEnabled
        {
            get { return isInteractingEnabled; }
            set { isInteractingEnabled = value; }
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

        public bool CanDash
        {
            get { return canDash; }
            set { canDash = value; }
        }

        public bool IsSwordLooted
        {
            get { return isSwordLooted; }
            set { isSwordLooted = value; }
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

        public void ToggleAllActions(bool value)
        {
            AreAbilitiesEnabled = value;
            CanDash = value;
            IsInteractingEnabled = value;
            IsMovable = value;
            IsTurnable = value;
        }
    }
}
