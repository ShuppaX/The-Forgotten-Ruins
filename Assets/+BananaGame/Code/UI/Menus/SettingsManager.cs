using UnityEngine;

namespace BananaSoup
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The Audio_Panel under the Settings_Panel.")]
        private GameObject audioPanel = null;

        [SerializeField, Tooltip("The Gamepad_Panel under the Settings_Panel.")]
        private GameObject gamepadPanel = null;

        [SerializeField, Tooltip("The KBM_Panel under the Settings_Panel.")]
        private GameObject kbmPanel = null;

        public void ToggleAudioPanelActive()
        {
            TryDisablePanel(gamepadPanel);
            TryDisablePanel(kbmPanel);

            TryActivatePanel(audioPanel);
        }

        public void ToggleGamepadPanelActive()
        {
            TryDisablePanel(audioPanel);
            TryDisablePanel(kbmPanel);

            TryActivatePanel(gamepadPanel);
        }

        public void ToggleKBMPanelActive()
        {
            TryDisablePanel(audioPanel);
            TryDisablePanel(gamepadPanel);

            TryActivatePanel(kbmPanel);
        }

        public void TogglePanelsOnBack()
        {
            TryActivatePanel(audioPanel);

            TryDisablePanel(gamepadPanel);
            TryDisablePanel(kbmPanel);
        }

        /// <summary>
        /// Method used to try to activate an inactive panel.
        /// Checks if the panel isn't active and then sets it active if it isn't.
        /// </summary>
        /// <param name="panel">The panel to activate.</param>
        private void TryActivatePanel(GameObject panel)
        {
            if ( !panel.activeSelf )
            {
                panel.SetActive(true);
            }
        }

        /// <summary>
        /// Method used to try to deactivate a active panel.
        /// Checks if the panel is active and then sets it inactive if it is.
        /// </summary>
        /// <param name="panel">The panel to disable.</param>
        private void TryDisablePanel(GameObject panel)
        {
            if ( panel.activeSelf )
            {
                panel.SetActive(false);
            }
        }
    }
}
