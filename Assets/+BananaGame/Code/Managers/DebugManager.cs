using BananaSoup.Utilities;
using TMPro;
using UnityEngine;

namespace BananaSoup.Managers
{
    public class DebugManager : MonoBehaviour
    {
        public static DebugManager Instance { get; private set; }

        [SerializeField]
        private bool IsDebugActive = false;

        [Header("Debug TMP texts from the BaseScene UI")]
        [SerializeField]
        private TMP_Text playerStateText;
        [SerializeField]
        private TMP_Text movementSpeedText;
        [SerializeField]
        private TMP_Text groundCheckText;

        [Space]

        [SerializeField]
        private GameObject debugUI = null;

        private float latestMovementSpeed = 0.0f;

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

        // Start is called before the first frame update
        private void Start()
        {
            if ( !IsDebugActive )
            {
                debugUI.SetActive(false);
            }
            else
            {
                playerController = PlayerBase.Instance.GetComponent<PlayerController>();
                if ( playerController == null )
                {
                    Debug.LogError($"{gameObject.name} couldn't find a PlayerController from PlayerBase!");
                }

                groundCheck = PlayerBase.Instance.GetComponent<GroundCheck>();
                if ( groundCheck == null )
                {
                    Debug.LogError($"{gameObject.name} couldn't find a GroundCheck from PlayerBase!");
                }

                debugUI.SetActive(true);
            }
        }

        private void Update()
        {
            UpdatePlayerStateText();
            UpdateGroundCheckText();
            GetMovementSpeed();
        }

        private void UpdatePlayerStateText()
        {
            if ( IsDebugActive )
            {
                playerStateText.SetText(PlayerStateManager.Instance.CurrentPlayerState.ToString());
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

        private void GetMovementSpeed()
        {
            if ( latestMovementSpeed != playerController.Rb.velocity.sqrMagnitude )
            {
                latestMovementSpeed = playerController.Rb.velocity.sqrMagnitude;
                UpdateMovementSpeedText(Mathf.Round(playerController.Rb.velocity.magnitude));
            }
        }

        private void UpdateMovementSpeedText(float currentMovementspeed)
        {
            if ( IsDebugActive )
            {
                movementSpeedText.SetText("Movementspeed: "
                + currentMovementspeed.ToString());
            }
        }
    }
}
