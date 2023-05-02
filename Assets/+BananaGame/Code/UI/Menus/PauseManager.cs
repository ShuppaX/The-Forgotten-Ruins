using UnityEngine;

namespace BananaSoup.UI.Menus
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The PauseButtons_Background under Pause_Panel (2)")]
        private GameObject pauseButtonsPanel;

        [SerializeField, Tooltip("The Quit_Panel under the Pause_Panel (2)")]
        private GameObject quitPanel;

        public void OpenQuitPanel()
        {
            if ( pauseButtonsPanel.activeSelf )
            {
                pauseButtonsPanel.SetActive(false);
            }

            if ( !quitPanel.activeSelf )
            {
                quitPanel.SetActive(true);
            }
        }

        public void ReturnToPause()
        {
            if ( quitPanel.activeSelf )
            {
                quitPanel.SetActive(false);
            }

            if ( !pauseButtonsPanel.activeSelf )
            {
                pauseButtonsPanel.SetActive(true);
            }
        }
    }
}
