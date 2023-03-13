using UnityEngine;

namespace BananaSoup
{
    public class PlayerStateManager : MonoBehaviour
    {
        public static PlayerStateManager Instance { get; private set; }

        [HideInInspector]
        public PlayerState currentPlayerState = 0;

        // NOTE: If you add or remove a PlayerState, DON'T CHANGE ID NUMBERS
        public enum PlayerState
        {
            Idle        = 0, // Default state
            Moving      = 1,
            Dashing     = 2,
            Attacking   = 3,
            Interacting = 4,
            InAir       = 5
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

        private void OnEnable()
        {
            AbilityDash.Instance.onDashAction += PlayerDashing;
            AbilityDash.Instance.onDashReset += PlayerIdleAfterDashOrMove;
            PlayerController.Instance.onPlayerGroundedAndIdle += PlayerIdle;
            PlayerController.Instance.onPlayerInAir += PlayerInAir;
            PlayerController.Instance.onPlayerMoveInput += PlayerMoving;
            PlayerController.Instance.onNoPlayerMoveInput += PlayerIdleAfterDashOrMove;
        }

        private void OnDisable()
        {
            AbilityDash.Instance.onDashAction -= PlayerDashing;
            AbilityDash.Instance.onDashReset -= PlayerIdleAfterDashOrMove;
            PlayerController.Instance.onPlayerGroundedAndIdle -= PlayerIdle;
            PlayerController.Instance.onPlayerInAir -= PlayerInAir;
            PlayerController.Instance.onPlayerMoveInput -= PlayerMoving;
            PlayerController.Instance.onNoPlayerMoveInput -= PlayerIdleAfterDashOrMove;
        }

        // Public methods which are called with UnityEvents in scripts when the
        // currentPlayerState should be updated.
        public void PlayerIdle()
        {
            if ( currentPlayerState == PlayerState.Moving 
                || currentPlayerState == PlayerState.Dashing )
            {
                return;
            }

            currentPlayerState = PlayerState.Idle;
        }

        private void PlayerIdleAfterDashOrMove()
        {
            currentPlayerState = PlayerState.Idle;
        }

        public void PlayerMoving()
        {
            if ( currentPlayerState == PlayerState.Dashing )
            {
                return;
            }

            currentPlayerState = PlayerState.Moving;
        }

        public void PlayerDashing()
        {
            currentPlayerState = PlayerState.Dashing;
        }

        public void PlayerAttacking()
        {
            currentPlayerState = PlayerState.Attacking;
        }

        public void PlayerInteracting()
        {
            currentPlayerState = PlayerState.Interacting;
        }

        public void PlayerInAir()
        {
            currentPlayerState = PlayerState.InAir;
        }
    }
}
