using TMPro;
using UnityEngine;

namespace BananaSoup
{
    public class DebugManager : MonoBehaviour
    {
        public static DebugManager Instance { get; private set; }

        [SerializeField]
        private bool IsDebugActive = true;

        [Header("Debug TMP texts from the BaseScene UI")]
        [SerializeField]
        private TMP_Text playerStateText;
        [SerializeField]
        private TMP_Text movementSpeedText;
        [SerializeField]
        private TMP_Text groundCheckText;

        [Header("Debux text gameObjects from the BaseScene UI")]
        [SerializeField]
        private GameObject playerStateDisplay;
        [SerializeField]
        private GameObject movementSpeedDisplay;
        [SerializeField]
        private GameObject groundCheckDisplay;

        private AbilityDash abilityDash = null;
        private PlayerController playerController = null;
        private GroundCheck groundCheck = null;

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
            if ( IsDebugActive )
            {
                TrySubscribing();
            }
        }

        private void OnDisable()
        {
            UnsubscribeAll();
        }

        // Start is called before the first frame update
        private void Start()
        {
            if ( !IsDebugActive )
            {
                playerStateDisplay.SetActive(false);
                movementSpeedDisplay.SetActive(false);
                groundCheckDisplay.SetActive(false);
            }
            else
            {
                Setup();
                TrySubscribing();

                playerStateDisplay.SetActive(true);
                movementSpeedDisplay.SetActive(true);
                groundCheckDisplay.SetActive(true);
            }
        }

        private void Setup()
        {
            abilityDash = PlayerBase.Instance.GetComponent<AbilityDash>();
            if ( abilityDash == null )
            {
                Debug.LogError("A AbilityDash component couldn't be found for the " + gameObject.name + "!");
            }

            playerController = PlayerBase.Instance.GetComponent<PlayerController>();
            if ( playerController == null )
            {
                Debug.LogError("A PlayerController component couldn't be found for the " + gameObject.name + "!");
            }

            groundCheck = PlayerBase.Instance.GetComponent<GroundCheck>();
            if ( groundCheck == null )
            {
                Debug.LogError("A GroundCheck component couldn't be found for the " + gameObject.name + "!");
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
            abilityDash.onDashAction += UpdatePlayerStateText;
            abilityDash.onDashResetToMoving += UpdatePlayerStateText;
            abilityDash.onDashResetToIdle += UpdatePlayerStateText;
        }

        private void SubscribePlayerController()
        {
            playerController.onPlayerMoveInput += UpdatePlayerStateText;
            playerController.onVelocityChanged += UpdateMovementSpeedText;
        }

        private void SubscribeGroundCheck()
        {
            groundCheck.onPlayerGroundedAndIdle += UpdatePlayerStateText;
            groundCheck.onPlayerInAir += UpdatePlayerStateText;
            groundCheck.onGroundedChanged += UpdateGroundCheckText;
        }

        private void UnsubscribeAll()
        {
            UnsubscribeAbilityDash();
            UnsubscribePlayerController();
            UnsubscribeGroundCheck();
        }

        private void UnsubscribeAbilityDash()
        {
            abilityDash.onDashAction -= UpdatePlayerStateText;
            abilityDash.onDashResetToMoving -= UpdatePlayerStateText;
            abilityDash.onDashResetToIdle -= UpdatePlayerStateText;
        }

        private void UnsubscribePlayerController()
        {
            playerController.onPlayerMoveInput -= UpdatePlayerStateText;
            playerController.onVelocityChanged -= UpdateMovementSpeedText;
        }

        private void UnsubscribeGroundCheck()
        {
            groundCheck.onPlayerGroundedAndIdle -= UpdatePlayerStateText;
            groundCheck.onPlayerInAir -= UpdatePlayerStateText;
            groundCheck.onGroundedChanged -= UpdateGroundCheckText;
        }

        private void UpdatePlayerStateText()
        {
            if ( IsDebugActive )
            {
                playerStateText.SetText(PlayerStateManager.Instance.currentPlayerState.ToString());
            }
        }

        private void UpdateMovementSpeedText()
        {
            if ( IsDebugActive )
            {
                movementSpeedText.SetText("Movementspeed: "
                + playerController.PlayerMovementspeed.ToString());
            }
        }

        private void UpdateGroundCheckText()
        {
            if ( IsDebugActive )
            {
                groundCheckText.SetText("GroundCheck: "
                + groundCheck.IsGrounded);
            }
        }
    }
}
