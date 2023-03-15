using UnityEngine;

namespace BananaSoup
{
    public class PlayerStateManager : MonoBehaviour
    {
        public static PlayerStateManager Instance { get; private set; }

        [HideInInspector]
        public PlayerState currentPlayerState = 0;

        private AbilityDash abilityDash = null;
        private PlayerController playerController = null;
        private GroundCheck groundCheck = null;

        // NOTE: If you add or remove a PlayerState, DON'T CHANGE ID NUMBERS
        public enum PlayerState
        {
            Idle = 0, // Default state
            Moving = 1,
            Dashing = 2,
            Attacking = 3,
            Interacting = 4,
            InAir = 5
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
            TrySubscribing();
        }

        private void OnDisable()
        {
            UnsubscribeAll();
        }

        private void Start()
        {
            Setup();
            TrySubscribing();
        }
        private void Setup()
        {
            abilityDash = GetComponent<AbilityDash>();
            if ( abilityDash == null )
            {
                Debug.LogError("A AbilityDash component couldn't be found on " + gameObject.name + "!");
            }

            playerController = GetComponent<PlayerController>();
            if ( playerController == null )
            {
                Debug.LogError("A PlayerController component couldn't be found on " + gameObject.name + "!");
            }

            groundCheck = GetComponent<GroundCheck>();
            if ( groundCheck == null )
            {
                Debug.LogError("A GroundCheck component couldn't be found on " + gameObject.name + "!");
            }
        }

        private void TrySubscribing()
        {
            if ( abilityDash != null )
            {
                SubscribeAbilityDash();
            }

            if ( playerController != null )
            {
                SubscribePlayerController();
            }

            if ( groundCheck != null )
            {
                SubscribeGroundCheck();
            }
        }

        private void SubscribeAbilityDash()
        {
            abilityDash.onDashAction += PlayerDashing;
            abilityDash.onDashReset += PlayerIdleAfterDashOrMove;
        }
        private void SubscribePlayerController()
        {
            playerController.onPlayerMoveInput += PlayerMoving;
            playerController.onNoPlayerMoveInput += PlayerIdleAfterDashOrMove;
        }

        private void SubscribeGroundCheck()
        {
            groundCheck.onPlayerGroundedAndIdle += PlayerIdle;
            groundCheck.onPlayerInAir += PlayerInAir;
        }

        private void UnsubscribeAll()
        {
            UnsubscribeAbilityDash();
            UnsubscribePlayerController();
            UnsubscribeGroundCheck();
        }

        private void UnsubscribeAbilityDash()
        {
            abilityDash.onDashAction -= PlayerDashing;
            abilityDash.onDashReset -= PlayerIdleAfterDashOrMove;
        }

        private void UnsubscribePlayerController()
        {
            playerController.onPlayerMoveInput -= PlayerMoving;
            playerController.onNoPlayerMoveInput -= PlayerIdleAfterDashOrMove;
        }

        private void UnsubscribeGroundCheck()
        {
            groundCheck.onPlayerGroundedAndIdle -= PlayerIdle;
            groundCheck.onPlayerInAir -= PlayerInAir;
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
