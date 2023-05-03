using BananaSoup.Managers;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;

namespace BananaSoup.UI.Menus
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField, Tooltip("In-game UI parent (In-Game_UI)")]
        private Canvas inGameUICanvas = null;

        [Space]

        [SerializeField, Tooltip("SeeThroughEffect GameObject which has the component" +
            " that allows seeing through set objects.")]
        private CustomPassVolume seeThroughEffect = null;

        private List<GameObject> menuPanels = new List<GameObject>();
        private List<GameObject> defaultButtons = new List<GameObject>();

        // References to main menu panels
        private GameObject mainMenuPanel = null;
        private GameObject settingsPanel = null;
        private GameObject pausePanel = null;

        // References to default button in each menu
        private GameObject mainMenuDefaultButton = null;
        private GameObject settingsDefaultButton = null;
        private GameObject pauseDefaultButton = null;
        private GameObject quitDefaultButton = null;
        private GameObject deathScreenDefaultbutton = null;

        // References to Instances
        private GameStateManager gameStateManager = null;
        private PlayerBase playerBase = null;

        // Constant GameStates used in comparing current GameState
        private const GameStateManager.GameState inMainMenu = GameStateManager.GameState.InMainMenu;
        private const GameStateManager.GameState inGame = GameStateManager.GameState.InGame;
        private const GameStateManager.GameState paused = GameStateManager.GameState.Paused;
        private const GameStateManager.GameState gameOver = GameStateManager.GameState.GameOver;

        public enum MenuType
        {
            MainMenu = 0,
            Settings = 1,
            Pause = 2,
            DeathScreen = 3
        }

        public enum DefaultButton
        {
            InMainMenu = 0,
            InSettings = 1,
            InPause = 2,
            InQuit = 3,
            InDeathScreen = 4
        }

        private void OnEnable()
        {
            SubscribeEvent();
        }

        private void OnDisable()
        {
            UnsubscribeEvent();
        }

        private void Awake()
        {
            InitializeMenuList();
            GetMenuObjectReferences();

            InitializeButtonList();
            GetButtonObjectReferences();
        }

        private void Start()
        {
            gameStateManager = GameStateManager.Instance;
            if ( gameStateManager == null )
            {
                Debug.LogError($"{name} couldn't find an instance of GameStateManager!");
            }

            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError($"{name} couldn't find an instance of PlayerBase!");
            }

            Setup();
        }

        /// <summary>
        /// Method used to initialize the menuPanels array with all objects that have
        /// the component "ObjectMenuType".
        /// </summary>
        private void InitializeMenuList()
        {
            ObjectMenuType[] childObjects = GetComponentsInChildren<ObjectMenuType>(true);

            foreach ( ObjectMenuType child in childObjects )
            {
                menuPanels.Add(child.gameObject);
            }
        }

        /// <summary>
        /// Method used to get the references of the different UI panels.
        /// </summary>
        private void GetMenuObjectReferences()
        {
            mainMenuPanel = GetMenu(MenuType.MainMenu);
            settingsPanel = GetMenu(MenuType.Settings);
            pausePanel = GetMenu(MenuType.Pause);
        }

        /// <summary>
        /// Method used to go through the menuPanels array and get the desired
        /// menuPanel of the specified menuType.
        /// </summary>
        /// <param name="menuType">The MenuType of the GameObject you want to find.</param>
        /// <returns>The GameObject which has the specified MenuType.</returns>
        private GameObject GetMenu(MenuType menuType)
        {
            for ( int i = 0; i < menuPanels.Count; i++ )
            {
                if ( menuPanels[i].GetComponent<ObjectMenuType>().MenuType == menuType )
                {
                    return menuPanels[i].gameObject;
                }
            }

            Debug.LogError($"No menu of type {menuType} could be found in menuPanels[]!");
            return null;
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
        /// Called in Start(). Used to disable inGameUICanvas, all other panels and
        /// the players input/actions.
        /// </summary>
        private void Setup()
        {
            if ( !mainMenuPanel.activeSelf )
            {
                mainMenuPanel.SetActive(true);
            }

            if ( inGameUICanvas.enabled )
            {
                inGameUICanvas.enabled = false;
            }

            if ( settingsPanel.activeSelf )
            {
                settingsPanel.SetActive(false);
            }

            if ( pausePanel.activeSelf )
            {
                pausePanel.SetActive(false);
            }

            if ( seeThroughEffect.enabled )
            {
                seeThroughEffect.enabled = false;
            }

            playerBase.ToggleAllActions(false);

            SetSelectedButton(mainMenuDefaultButton);
        }

        /// <summary>
        /// Method called when GameState is changed.
        /// </summary>
        public void GameStateChanged()
        {
            switch ( gameStateManager.CurrentGameState )
            {
                // Set inGameUICanvas disabled and activate mainMenuPanel.
                case (inMainMenu):
                    inGameUICanvas.enabled = false;
                    mainMenuPanel.SetActive(true);
                    SetSelectedButton(mainMenuDefaultButton);
                    break;
                case (inGame):
                    ChangingGameStateToInGame();
                    mainMenuPanel.SetActive(false);
                    inGameUICanvas.enabled = true;
                    break;
                case (paused):
                    pausePanel.SetActive(true);
                    inGameUICanvas.enabled = false;
                    playerBase.ToggleAllActions(false);
                    SetSelectedButton(pauseDefaultButton);
                    break;
                case (gameOver):
                    // TODO: Make a deathscreen panel etc. and activate it here!
                    //SetSelectedButton(deathScreenDefaultButton);
                    break;
                default:
                    Debug.LogError("GameStateManager has an incorrect GameState! This is a bug!");
                    break;
            }
        }

        /// <summary>
        /// Method used to subscribe events.
        /// </summary>
        private void SubscribeEvent()
        {
            GameStateManager.OnGameStateChanged += GameStateChanged;
        }

        /// <summary>
        /// Method used to unsubscribe events.
        /// </summary>
        private void UnsubscribeEvent()
        {
            GameStateManager.OnGameStateChanged -= GameStateChanged;
        }

        /// <summary>
        /// Method called from the Settings button in main menu and from in-game in the
        /// pause menu.
        /// If the GameState is inMainMenu then toggle mainMenuPanel on button press.
        /// If the GameState is paused then toggle pausePanel on button press.
        /// </summary>
        public void ToggleSettings()
        {
            if ( gameStateManager.CurrentGameState == inMainMenu )
            {
                mainMenuPanel.SetActive(!mainMenuPanel.activeSelf);

                if ( mainMenuPanel.activeSelf )
                {
                    SetSelectedButton(mainMenuDefaultButton);
                }
            }

            if ( gameStateManager.CurrentGameState == paused )
            {
                pausePanel.SetActive(!pausePanel.activeSelf);

                if ( pausePanel.activeSelf )
                {
                    SetSelectedButton(pauseDefaultButton);
                }
            }

            settingsPanel.SetActive(!settingsPanel.activeSelf);

            if ( settingsPanel.activeSelf )
            {
                SetSelectedButton(settingsDefaultButton);
            }
        }

        /// <summary>
        /// Method called when pressing any of the set pause buttons.
        /// If the pausePanel isn't active then set the GameState to paused, if it is
        /// set the GameState to inGame.
        /// </summary>
        public void OnPause(InputAction.CallbackContext context)
        {
            if ( gameStateManager.CurrentGameState == inMainMenu
                || gameStateManager.CurrentGameState == gameOver )
            {
                return;
            }

            if ( !context.performed )
            {
                return;
            }

            if ( !pausePanel.activeSelf )
            {
                gameStateManager.SetGameState(paused);
                Time.timeScale = 0;
            }
            else
            {
                gameStateManager.SetGameState(inGame);
            }
        }

        /// <summary>
        /// Method called when pressing the back button in pauseMenu.
        /// </summary>
        public void OnPauseBackButton()
        {
            if ( gameStateManager.CurrentGameState == paused )
            {
                gameStateManager.SetGameState(inGame);
            }
        }

        /// <summary>
        /// Method called when the GameState is changed to InGame.
        /// Used to check if pausePanel or settingsPanel are active, and if they are
        /// disable them.
        /// </summary>
        private void ChangingGameStateToInGame()
        {
            if ( pausePanel.activeSelf )
            {
                pausePanel.SetActive(false);
            }

            if ( settingsPanel.activeSelf )
            {
                settingsPanel.SetActive(false);
            }

            if ( Time.timeScale != 1 )
            {
                Time.timeScale = 1;
            }

            if ( !seeThroughEffect.enabled )
            {
                seeThroughEffect.enabled = true;
            }

            playerBase.ToggleAllActions(true);
        }

        /// <summary>
        /// Method called from Quit buttons to exit the application while in build and
        /// while in editor toggle playmode off.
        /// </summary>
        public void QuitGame()
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }

        /// <summary>
        /// Method used to null the selected GameObject from EventSYstem and then set
        /// the GameObject to be the one entered as a parameter.
        /// </summary>
        /// <param name="button">The button you want to select.</param>
        private void SetSelectedButton(GameObject button)
        {
            // Remove currently selected object for EventSystem
            EventSystem.current.SetSelectedGameObject(null);

            // Set selected object for EventSystem
            EventSystem.current.SetSelectedGameObject(button);
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
            SetSelectedButton(pauseDefaultButton);
        }

        /// <summary>
        /// Method called when play button in main menu is pressed.
        /// Used to deactivate the mainMenuPanel.
        /// </summary>
        public void OnPlayButton()
        {
            mainMenuPanel.SetActive(false);
        }
    }
}
