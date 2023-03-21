using BananaSoup.InteractSystem;
using UnityEngine;
using UnityEngine.Events;

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
        private AbilityInteract interact = null;
        private AbilityBlindingSand blindingSand = null;
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

            interact = GetComponent<AbilityInteract>();
            if (interact == null )
            {
                Debug.LogError("A AbilityInteract component couldn't be found on " + gameObject.name + "!");
            }

            blindingSand = GetComponent<AbilityBlindingSand>();
            if ( blindingSand == null )
            {
                Debug.LogError("A AbilityBlindingSand component couldn't be found on " + gameObject.name + "!");
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

            if ( interact != null )
            {
                SubscribeInteract();
            }

            if ( blindingSand != null )
            {
                SubscribeBlindingSand();
            }
        }

        private void SubscribeAbilityDash()
        {
            abilityDash.onDashAction += PlayerDashing;
            abilityDash.onDashResetToMoving += PlayerMoving;
            abilityDash.onDashResetToIdle += PlayerIdle;
        }
        private void SubscribePlayerController()
        {
            playerController.onPlayerMoveInput += PlayerMoving;
            playerController.onNoPlayerMoveInput += PlayerIdle;
        }

        private void SubscribeGroundCheck()
        {
            groundCheck.onPlayerGrounded += PlayerIdle;
            groundCheck.onPlayerInAir += PlayerInAir;
        }

        private void SubscribeInteract()
        {
            interact.onInteracting += PlayerInteracting;
            interact.onInteractionCancelled += PlayerIdle;
        }

        private void SubscribeBlindingSand()
        {
            blindingSand.onSanding += PlayerSanding;
            blindingSand.onSandingFinished += PlayerIdle;
        }

        private void UnsubscribeAll()
        {
            UnsubscribeAbilityDash();
            UnsubscribePlayerController();
            UnsubscribeGroundCheck();
            UnsubscribeInteract();
            UnsubscribeBlindingSand();
        }

        private void UnsubscribeAbilityDash()
        {
            abilityDash.onDashAction -= PlayerDashing;
            abilityDash.onDashResetToMoving -= PlayerMoving;
            abilityDash.onDashResetToIdle -= PlayerIdle;
        }

        private void UnsubscribePlayerController()
        {
            playerController.onPlayerMoveInput -= PlayerMoving;
            playerController.onNoPlayerMoveInput -= PlayerIdle;
        }

        private void UnsubscribeGroundCheck()
        {
            groundCheck.onPlayerGrounded -= PlayerIdle;
            groundCheck.onPlayerInAir -= PlayerInAir;
        }

        private void UnsubscribeInteract()
        {
            interact.onInteracting -= PlayerInteracting;
            interact.onInteractionCancelled -= PlayerIdle;
        }

        private void UnsubscribeBlindingSand()
        {
            blindingSand.onSanding -= PlayerSanding;
            blindingSand.onSandingFinished -= PlayerIdle;
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
    }
}
