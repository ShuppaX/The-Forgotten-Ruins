using UnityEngine;
using UnityEngine.Events;

namespace BananaSoup
{
    public class PlayerStateManager : MonoBehaviour
    {
        public static PlayerStateManager Instance { get; private set; }

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
            InvokeStateChangeEvent();
        }

        private void PlayerMoving()
        {
            if ( currentPlayerState == PlayerState.Moving
                || currentPlayerState == PlayerState.Interacting )
            {
                return;
            }

            currentPlayerState = PlayerState.Moving;
            InvokeStateChangeEvent();
        }

        private void PlayerDashing()
        {
            if ( currentPlayerState == PlayerState.Dashing )
            {
                return;
            }

            currentPlayerState = PlayerState.Dashing;
            InvokeStateChangeEvent();
        }

        private void PlayerAttacking()
        {
            if ( currentPlayerState == PlayerState.Attacking )
            {
                return;
            }

            currentPlayerState = PlayerState.Attacking;
            InvokeStateChangeEvent();
        }

        private void PlayerInteracting()
        {
            if ( currentPlayerState == PlayerState.Interacting )
            {
                return;
            }

            currentPlayerState = PlayerState.Interacting;
            InvokeStateChangeEvent();
        }

        private void PlayerInAir()
        {
            if ( currentPlayerState == PlayerState.InAir
                || currentPlayerState == PlayerState.Dashing )
            {
                return;
            }

            currentPlayerState = PlayerState.InAir;
            InvokeStateChangeEvent();
        }

        private void PlayerSanding()
        {
            if ( currentPlayerState == PlayerState.Sanding )
            {
                return;
            }

            currentPlayerState = PlayerState.Sanding;
            InvokeStateChangeEvent();
        }

        private void PlayerSparking()
        {
            if (currentPlayerState == PlayerState.Sparking )
            {
                return;
            }

            currentPlayerState = PlayerState.Sparking;
            InvokeStateChangeEvent();
        }

        private void InvokeStateChangeEvent()
        {
            stateChanged.Invoke();
        }

        public void SetPlayerState(string caseValue)
        {
            switch (caseValue)
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
