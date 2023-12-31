using UnityEngine;

namespace BananaSoup.UI.Menus
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The Audio_Panel under the Settings_Panel.")]
        private GameObject audioPanel = null;
        [SerializeField, Tooltip("The Video_Panel under the Settings_Panel.")]
        private GameObject videoPanel = null;
        [SerializeField, Tooltip("The Gamepad_Panel under the Settings_Panel.")]
        private GameObject gamepadPanel = null;
        [SerializeField, Tooltip("The KBM_Panel under the Settings_Panel.")]
        private GameObject kbmPanel = null;

        private void Start()
        {
            ToggleAudioPanelActive();
        }

        public void ToggleAudioPanelActive()
        {
            TryDisablePanel(videoPanel);
            TryDisablePanel(gamepadPanel);
            TryDisablePanel(kbmPanel);

            TryActivatePanel(audioPanel);
        }

        public void ToggleVideoPanelActive()
        {
            TryDisablePanel(audioPanel);
            TryDisablePanel(gamepadPanel);
            TryDisablePanel(kbmPanel);

            TryActivatePanel(videoPanel);
        }

        public void ToggleGamepadPanelActive()
        {
            TryDisablePanel(audioPanel);
            TryDisablePanel(videoPanel);
            TryDisablePanel(kbmPanel);

            TryActivatePanel(gamepadPanel);
        }

        public void ToggleKBMPanelActive()
        {
            TryDisablePanel(audioPanel);
            TryDisablePanel(videoPanel);
            TryDisablePanel(gamepadPanel);

            TryActivatePanel(kbmPanel);
        }

        public void TogglePanelsOnBack()
        {
            TryActivatePanel(audioPanel);

            TryDisablePanel(videoPanel);
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
