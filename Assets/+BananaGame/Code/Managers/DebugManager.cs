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
                Debug.LogError("Couldn't find a Component of type AbilityDash on the " + gameObject.name + "!");
            }

            playerController = PlayerBase.Instance.GetComponent<PlayerController>();
            if ( playerController == null )
            {
                Debug.LogError("Couldn't find a Component of type PlayerController on the " + gameObject.name + "!");
            }
        }

        private void TrySubscribing()
        {
            if ( abilityDash != null )
            {
                SubscribeAbilityDash();
            }

            if (playerController != null )
            {
                SubscribePlayerController();
            }
        }

        private void SubscribeAbilityDash()
        {
            abilityDash.onDashAction += UpdatePlayerStateText;
            abilityDash.onDashReset += UpdatePlayerStateText;
        }

        private void SubscribePlayerController()
        {
            playerController.onPlayerGroundedAndIdle += UpdatePlayerStateText;
            playerController.onPlayerInAir += UpdatePlayerStateText;
            playerController.onPlayerMoveInput += UpdatePlayerStateText;
            playerController.onVelocityChanged += UpdateMovementSpeedText;
            playerController.onGroundCheck += UpdateGroundCheckText;
        }

        private void UnsubscribeAll()
        {
            UnsubscribeAbilityDash();
            UnsubscribePlayerController();
        }

        private void UnsubscribeAbilityDash()
        {
            abilityDash.onDashAction -= UpdatePlayerStateText;
            abilityDash.onDashReset -= UpdatePlayerStateText;
        }

        private void UnsubscribePlayerController()
        {
            playerController.onPlayerGroundedAndIdle -= UpdatePlayerStateText;
            playerController.onPlayerInAir -= UpdatePlayerStateText;
            playerController.onPlayerMoveInput -= UpdatePlayerStateText;
            playerController.onVelocityChanged -= UpdateMovementSpeedText;
            playerController.onGroundCheck -= UpdateGroundCheckText;
        }

        public void UpdatePlayerStateText()
        {
            if ( IsDebugActive )
            {
                playerStateText.SetText(PlayerStateManager.Instance.currentPlayerState.ToString());
            }
        }

        public void UpdateMovementSpeedText()
        {
            if ( IsDebugActive )
            {
                movementSpeedText.SetText("Movementspeed: "
                + playerController.PlayerMovementspeed.ToString());
            }
        }

        public void UpdateGroundCheckText()
        {
            if ( IsDebugActive )
            {
                groundCheckText.SetText("GroundCheck: "
                + playerController.IsGrounded);
            }
        }
    }
}
