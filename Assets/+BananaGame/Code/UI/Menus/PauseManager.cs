using UnityEngine;

namespace BananaSoup.UI.Menus
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The PauseButtons_Background under Pause_Panel (2)")]
        private GameObject pauseButtonsPanel;

        [SerializeField, Tooltip("The Quit_Panel under the Pause_Panel (2)")]
        private GameObject quitPanel;

        private bool inQuitMenu = false;
        public bool InQuitMenu => inQuitMenu;

        public void OpenQuitPanel()
        {
            inQuitMenu = true;

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
            inQuitMenu = false;

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
