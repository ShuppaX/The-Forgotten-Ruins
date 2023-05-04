using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BananaSoup.UI.Menus
{
    public class MenuButtonHandler : MonoBehaviour
    {
        private List<GameObject> defaultButtons = new List<GameObject>();

        private GameObject previousButton = null;

        public bool IsPreviousButtonNull
        {
            get => previousButton;
        }

        // References to default button in each menu
        private GameObject mainMenuDefaultButton = null;
        private GameObject settingsDefaultButton = null;
        private GameObject pauseDefaultButton = null;
        private GameObject quitDefaultButton = null;
        private GameObject deathScreenDefaultButton = null;

        // Public properties of all the default button GameObjects.
        public GameObject MainMenuDefaultButton
        {
            get => mainMenuDefaultButton;
        }

        public GameObject SettingsDefaultButton
        {
            get => settingsDefaultButton;
        }

        public GameObject PauseDefaultButton
        {
            get => pauseDefaultButton;
        }

        public GameObject QuitDefaultButton
        {
            get => quitDefaultButton;
        }

        public GameObject DeathScreenDefaultButton
        {
            get => deathScreenDefaultButton;
        }

        public enum DefaultButton
        {
            InMainMenu = 0,
            InSettings = 1,
            InPause = 2,
            InQuit = 3,
            InDeathScreen = 4
        }

        private void Awake()
        {
            InitializeButtonList();
            GetButtonObjectReferences();
        }

        /// <summary>
        /// Method used to go through the children of this GameObject and find all
        /// GameObjects with the component ObjectDefaultButtonIn and then add them to
        /// the defaultButtons list.
        /// </summary>
        private void InitializeButtonList()
        {
            ObjectDefaultButtonIn[] defaultButtonChildren = GetComponentsInChildren<ObjectDefaultButtonIn>(true);

            foreach ( ObjectDefaultButtonIn button in defaultButtonChildren )
            {
                defaultButtons.Add(button.gameObject);
            }
        }

        /// <summary>
        /// Method used to get the default button GameObjecte references.
        /// </summary>
        private void GetButtonObjectReferences()
        {
            mainMenuDefaultButton = GetButton(DefaultButton.InMainMenu);
            settingsDefaultButton = GetButton(DefaultButton.InSettings);
            pauseDefaultButton = GetButton(DefaultButton.InPause);
            quitDefaultButton = GetButton(DefaultButton.InQuit);
            //deathScreenDefaultbutton = GetButton(DefaultButton.InDeathScreen);
        }

        /// <summary>
        /// Method used to get the specified button from defaultButtons list.
        /// </summary>
        /// <param name="button">The button you want to find.</param>
        /// <returns></returns>
        private GameObject GetButton(DefaultButton button)
        {
            for ( int i = 0; i < defaultButtons.Count; i++ )
            {
                if ( defaultButtons[i].GetComponent<ObjectDefaultButtonIn>().DefaultButton == button )
                {
                    return defaultButtons[i].gameObject;
                }
            }

            Debug.LogError($"Button with enum value {button} couldn't be found in defaultButtons!");
            return null;
        }

        /// <summary>
        /// Method used to null the selected GameObject from EventSYstem and then set
        /// the GameObject to be the one entered as a parameter.
        /// </summary>
        /// <param name="button">The button you want to select.</param>
        public void SetSelectedButton(GameObject button)
        {
            if ( EventSystem.current.currentSelectedGameObject != null
                && previousButton == null )
            {
                previousButton = EventSystem.current.currentSelectedGameObject;
            }

            // Remove currently selected object for EventSystem
            EventSystem.current.SetSelectedGameObject(null);

            // Set selected object for EventSystem
            EventSystem.current.SetSelectedGameObject(button);
        }

        public void SelectPreviousButton()
        {
            if ( previousButton != null )
            {
                SetSelectedButton(previousButton);
                previousButton = null;
            }
        }

        /// <summary>
        /// Method called when entering the quit menu in pause menu.
        /// Used to set the selected button to quitDefaultButton.
        /// </summary>
        public void OnEnterQuitMenu()
        {
            SetSelectedButton(quitDefaultButton);
        }

        /// <summary>
        /// Method called when exiting the quit menu in pause menu.
        /// Used to set the selected button to pauseDefaultButton.
        /// </summary>
        public void OnExitQuitMenu()
        {
            SetSelectedButton(previousButton);
        }
    }
}
