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
            AbilityDash.Instance.onDashAction += UpdatePlayerStateText;
            AbilityDash.Instance.onDashReset += UpdatePlayerStateText;
            PlayerController.Instance.onPlayerGroundedAndIdle += UpdatePlayerStateText;
            PlayerController.Instance.onPlayerInAir += UpdatePlayerStateText;
            PlayerController.Instance.onPlayerMoveInput += UpdatePlayerStateText;
            PlayerController.Instance.onPlayerMoveInput += UpdateMovementSpeedText;
            PlayerController.Instance.onNoPlayerMoveInput += UpdateMovementSpeedText;
            PlayerController.Instance.onGroundCheck += UpdateGroundCheckText;
        }

        private void OnDisable()
        {
            AbilityDash.Instance.onDashAction -= UpdatePlayerStateText;
            AbilityDash.Instance.onDashReset -= UpdatePlayerStateText;
            PlayerController.Instance.onPlayerGroundedAndIdle -= UpdatePlayerStateText;
            PlayerController.Instance.onPlayerInAir -= UpdatePlayerStateText;
            PlayerController.Instance.onPlayerMoveInput -= UpdatePlayerStateText;
            PlayerController.Instance.onPlayerMoveInput -= UpdateMovementSpeedText;
            PlayerController.Instance.onNoPlayerMoveInput -= UpdateMovementSpeedText;
            PlayerController.Instance.onGroundCheck -= UpdateGroundCheckText;
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
                playerStateDisplay.SetActive(true);
                movementSpeedDisplay.SetActive(true);
                groundCheckDisplay.SetActive(true);
            }
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
                + PlayerController.Instance.PlayerMovementspeed.ToString());
            }
        }

        public void UpdateGroundCheckText()
        {
            if ( IsDebugActive )
            {
                groundCheckText.SetText("GroundCheck: "
                + PlayerController.Instance.IsGrounded);
            }
        }
    }
}
