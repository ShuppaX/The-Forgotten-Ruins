using UnityEngine;
using UnityEngine.EventSystems;

namespace BananaSoup
{
    public class SettingsButtonHandler : MonoBehaviour
    {
        [SerializeField, Tooltip("Audio_Panel audio pagebutton.")]
        private GameObject audioPageButton;

        [SerializeField, Tooltip("Video_Panel video pagebutton.")]
        private GameObject videoPageButton;

        [SerializeField, Tooltip("Gamepad_Panel gamepad pagebutton.")]
        private GameObject gamepadPageButton;

        [SerializeField, Tooltip("KB_Panel keyboard pagebutton.")]
        private GameObject keyboardPageButton;

        public void GoToAudioPage()
        {
            SetSelectedButton(audioPageButton);
        }

        public void GoToVideoPage()
        {
            SetSelectedButton(videoPageButton);
        }

        public void GoToGamepadPage()
        {
            SetSelectedButton(gamepadPageButton);
        }

        public void GoToKeyboardPage()
        {
            SetSelectedButton(keyboardPageButton);
        }

        public void SetSelectedButton(GameObject button)
        {
            // Remove currently selected object for EventSystem
            EventSystem.current.SetSelectedGameObject(null);

            // Set selected object for EventSystem
            EventSystem.current.SetSelectedGameObject(button);
        }
    }
}
