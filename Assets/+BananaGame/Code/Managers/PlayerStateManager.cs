using UnityEngine;
using UnityEngine.Events;

namespace BananaSoup.Managers
{
    public class PlayerStateManager : MonoBehaviour
    {
        public static PlayerStateManager Instance { get; private set; }

        private PlayerAnimationManager animationManager = null;
        private PlayerController playerController = null;

        private PlayerState currentPlayerState = PlayerState.Idle;

        public PlayerState CurrentPlayerState
        {
            get => currentPlayerState;
        }

        private PlayerState previousPlayerState = PlayerState.Idle;

        public UnityAction stateChanged;

        // NOTE: If you add or remove a PlayerState, DON'T CHANGE ID NUMBERS
        public enum PlayerState
        {
            Idle = 0, // Default state
            Moving = 1,
            Dashing = 2,
            Attacking = 3,
            InteractingIdle = 4,
            InAir = 5,
            Sanding = 6,
            Sparking = 7,
            Dead = 8,
            InteractingMove = 9,
            PickingUp = 10,
            PuttingDown = 11
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

        private void Setup()
        {
            animationManager = GetComponent<PlayerAnimationManager>();
            if ( animationManager == null )
            {
                Debug.Log(this + " is missing reference to the PlayerAnimationManager and it is required!");
            }

            playerController = GetComponent<PlayerController>();
            if ( playerController == null )
            {
                Debug.Log(this + " is missing reference to the PlayerController and it is required!");
            }
        }

        /// <summary>
        /// Method that is called when the playerState changes (On set or reset) to
        /// activate a corresponding animation for the player.
        /// </summary>
        private void OnStateChanged()
        {
            animationManager.ResetTrigger(previousPlayerState.ToString());

            if ( currentPlayerState == PlayerState.Dead )
            {
                animationManager.SetAnimation(PlayerState.Idle.ToString());
                return;
            }

            animationManager.SetAnimation(currentPlayerState.ToString());

            stateChanged();
        }

        /// <summary>
        /// Method used to set the playerState at the beginning of an action.
        /// Check if the current playerState is same as the new one to avoid constantly
        /// changing the playerState to the same state.
        /// </summary>
        /// <param name="newPlayerState"></param>
        public void SetPlayerState(PlayerState newPlayerState)
        {
            if ( currentPlayerState == newPlayerState )
            {
                return;
            }

            if ( currentPlayerState == PlayerState.Dead )
            {
                return;
            }

            previousPlayerState = currentPlayerState;
            currentPlayerState = newPlayerState;
            OnStateChanged();
        }

        /// <summary>
        /// Method used to reset the playerState after an action.
        /// Goes to Moving if there is any moveInput active and to Idle if there isn't.
        /// </summary>
        public void ResetPlayerState()
        {
            if ( currentPlayerState == PlayerState.Dead )
            {
                return;
            }

            if ( currentPlayerState == PlayerState.PickingUp )
            {
                return;
            }

            previousPlayerState = currentPlayerState;

            if ( playerController.HasMoveInput )
            {
                currentPlayerState = PlayerState.Moving;
            }
            else
            {
                currentPlayerState = PlayerState.Idle;
            }

            OnStateChanged();
        }
    }
}
