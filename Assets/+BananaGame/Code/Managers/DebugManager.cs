using BananaSoup.Utilities;
using TMPro;
using UnityEngine;

namespace BananaSoup.Managers
{
    public class DebugManager : MonoBehaviour
    {
        [Header("Debug TMP texts from the BaseScene UI")]
        [SerializeField]
        private TMP_Text playerStateText;
        [SerializeField]
        private TMP_Text movementSpeedText;
        [SerializeField]
        private TMP_Text groundCheckText;

        // References
        private PlayerController playerController = null;
        private GroundCheck groundCheck = null;

        // Start is called before the first frame update
        private void Start()
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
        }

        private void Update()
        {
            UpdatePlayerStateText();
            UpdateGroundCheckText();
            UpdateMovementSpeedText(GetMovementSpeed());
        }

        /// <summary>
        /// Method to update PlayerState text, used in Update().
        /// </summary>
        private void UpdatePlayerStateText()
        {
            playerStateText.SetText(PlayerStateManager.Instance.CurrentPlayerState.ToString());
        }

        /// <summary>
        /// Method used to update GroundCheck text, used in Update().
        /// </summary>
        private void UpdateGroundCheckText()
        {
            groundCheckText.SetText("GroundCheck: " + groundCheck.IsGrounded);
        }

        /// <summary>
        /// Method used to get the players movement speed, used in Update() as a
        /// parameter for UpdateMovementSpeedText().
        /// </summary>
        private float GetMovementSpeed()
        {
            return Mathf.Round(playerController.Rb.velocity.magnitude);
        }

        /// <summary>
        /// Method used to update movement speed text, used in Update().
        /// </summary>
        /// <param name="currentMovementspeed">The current movement speed.</param>
        private void UpdateMovementSpeedText(float currentMovementspeed)
        {
            movementSpeedText.SetText("Movementspeed: " + currentMovementspeed.ToString());
        }
    }
}
