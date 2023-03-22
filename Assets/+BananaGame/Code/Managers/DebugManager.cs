using BananaSoup.InteractSystem;
using TMPro;
using UnityEngine;

namespace BananaSoup
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
        private GameObject debugUI;

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
                debugUI.SetActive(true);
            }
        }

        public void UpdatePlayerStateText()
        {
            if ( IsDebugActive )
            {
                playerStateText.SetText(PlayerStateManager.Instance.currentPlayerState.ToString());
            }
        }

        public void UpdateMovementSpeedText(float currentMovementspeed)
        {
            if ( IsDebugActive )
            {
                movementSpeedText.SetText("Movementspeed: "
                + currentMovementspeed.ToString());
            }
        }

        public void UpdateGroundCheckText(bool groundCheck)
        {
            if ( IsDebugActive )
            {
                groundCheckText.SetText("GroundCheck: "
                + groundCheck);
            }
        }
    }
}
