using UnityEngine;
using UnityEngine.Events;

namespace BananaSoup
{
    public class PlayerStateManager : MonoBehaviour
    {
        public static PlayerStateManager Instance { get; private set; }
        private PlayerAnimationManager animationManager;

        [HideInInspector]
        public PlayerState currentPlayerState = PlayerState.Idle;
        private PlayerState lastPlayerState = PlayerState.Idle;

        public UnityAction stateChanged;

        // NOTE: If you add or remove a PlayerState, DON'T CHANGE ID NUMBERS
        public enum PlayerState
        {
            Idle = 0, // Default state
            Moving = 1,
            Dashing = 2,
            Attacking = 3,
            Interacting = 4,
            InAir = 5,
            Sanding = 6,
            Sparking = 7
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
        }

        private void OnStateChanged(string animationName)
        {
            animationManager.SetAnimation(animationName);
        }

        public void SetPlayerState(PlayerState newPlayerState)
        {
            if ( currentPlayerState == newPlayerState )
            {
                return;
            }

            lastPlayerState = currentPlayerState;
            currentPlayerState = newPlayerState;
            OnStateChanged(currentPlayerState.ToString());
        }

        public void ResetPlayerState()
        {
            currentPlayerState = lastPlayerState;
            OnStateChanged(currentPlayerState.ToString());
        }
    }
}
