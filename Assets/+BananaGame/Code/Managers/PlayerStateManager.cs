using UnityEngine;
using UnityEngine.Events;

namespace BananaSoup
{
    public class PlayerStateManager : MonoBehaviour
    {
        public static PlayerStateManager Instance { get; private set; }
        private PlayerAnimationManager animationManager;

        [HideInInspector]
        public PlayerState currentPlayerState = 0;

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

        // Methods which are called with UnityEvents in scripts when the
        // currentPlayerState should be updated.
        private void PlayerIdle()
        {
            if ( currentPlayerState == PlayerState.Idle )
            {
                return;
            }

            currentPlayerState = PlayerState.Idle;
            OnStateChanged(currentPlayerState.ToString());
        }

        private void PlayerMoving()
        {
            if ( currentPlayerState == PlayerState.Moving
                || currentPlayerState == PlayerState.Interacting )
            {
                return;
            }

            currentPlayerState = PlayerState.Moving;
            OnStateChanged(currentPlayerState.ToString());
        }

        private void PlayerDashing()
        {
            if ( currentPlayerState == PlayerState.Dashing )
            {
                return;
            }

            currentPlayerState = PlayerState.Dashing;
            OnStateChanged(currentPlayerState.ToString());
        }

        private void PlayerAttacking()
        {
            if ( currentPlayerState == PlayerState.Attacking )
            {
                return;
            }

            currentPlayerState = PlayerState.Attacking;
            OnStateChanged(currentPlayerState.ToString());
        }

        private void PlayerInteracting()
        {
            if ( currentPlayerState == PlayerState.Interacting )
            {
                return;
            }

            currentPlayerState = PlayerState.Interacting;
            OnStateChanged(currentPlayerState.ToString());
        }

        private void PlayerInAir()
        {
            if ( currentPlayerState == PlayerState.InAir
                || currentPlayerState == PlayerState.Dashing )
            {
                return;
            }

            currentPlayerState = PlayerState.InAir;
            OnStateChanged(currentPlayerState.ToString());
        }

        private void PlayerSanding()
        {
            if ( currentPlayerState == PlayerState.Sanding )
            {
                return;
            }

            currentPlayerState = PlayerState.Sanding;
            OnStateChanged(currentPlayerState.ToString());
        }

        private void PlayerSparking()
        {
            if ( currentPlayerState == PlayerState.Sparking )
            {
                return;
            }

            currentPlayerState = PlayerState.Sparking;
            OnStateChanged(currentPlayerState.ToString());
        }

        private void OnStateChanged(string animationName)
        {
            animationManager.SetAnimation(animationName);
        }

        public void SetPlayerState(string caseValue)
        {
            switch ( caseValue )
            {
                case "Idle":
                    PlayerIdle();
                    break;
                case "Moving":
                    PlayerMoving();
                    break;
                case "Dashing":
                    PlayerDashing();
                    break;
                case "Attacking":
                    PlayerAttacking();
                    break;
                case "Interacting":
                    PlayerInteracting();
                    break;
                case "InAir":
                    PlayerInAir();
                    break;
                case "Sanding":
                    PlayerSanding();
                    break;
                case "Sparking":
                    PlayerSparking();
                    break;
                default:
                    PlayerIdle();
                    break;
            }
        }
    }
}
