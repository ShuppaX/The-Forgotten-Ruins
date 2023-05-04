using BananaSoup.Managers;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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

        // References to main menu panels
        private GameObject mainMenuPanel = null;
        private GameObject settingsPanel = null;
        private GameObject pausePanel = null;

        // References to Instances
        private GameStateManager gameStateManager = null;
        private PlayerBase playerBase = null;

        // Reference to MenuButtonHandler
        private MenuButtonHandler buttonHandler = null;

        // Constant GameStates used in comparing current GameState
        private const GameStateManager.GameState start = GameStateManager.GameState.Start;
        private const GameStateManager.GameState playing = GameStateManager.GameState.Playing;
        private const GameStateManager.GameState paused = GameStateManager.GameState.Paused;
        private const GameStateManager.GameState settings = GameStateManager.GameState.Settings;
        private const GameStateManager.GameState gameOver = GameStateManager.GameState.GameOver;

        public enum MenuType
        {
            MainMenu = 0,
            Settings = 1,
            Pause = 2,
            DeathScreen = 3
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

            buttonHandler = GetComponent<MenuButtonHandler>();
            if ( buttonHandler == null )
            {
                Debug.LogError($"No component of type MenuButtonHandler was found on the {name}!");
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

            ChangeSelectedButton(buttonHandler.MainMenuDefaultButton);
        }

        /// <summary>
        /// Method called when GameState is changed.
        /// </summary>
        public void GameStateChanged()
        {
            switch ( gameStateManager.CurrentGameState )
            {
                case (start):
                    inGameUICanvas.enabled = false;
                    mainMenuPanel.SetActive(true);
                    ChangeSelectedButton(buttonHandler.MainMenuDefaultButton);
                    break;
                case (playing):
                    ChangingGameStateToInGame();
                    inGameUICanvas.enabled = true;
                    break;
                case (paused):
                    pausePanel.SetActive(true);
                    inGameUICanvas.enabled = false;
                    break;
                case (settings):
                    settingsPanel.SetActive(true);
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
        /// Method called when pressing any of the set pause buttons.
        /// If the pausePanel isn't active then set the GameState to paused, if it is
        /// set the GameState to inGame.
        /// </summary>
        public void OnPause(InputAction.CallbackContext context)
        {
            if ( gameStateManager.CurrentGameState == start
                || gameStateManager.CurrentGameState == settings
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
                ChangeSelectedButton(buttonHandler.PauseDefaultButton);
                playerBase.ToggleAllActions(false);
                Time.timeScale = 0;
            }
            else
            {
                pausePanel.SetActive(false);
                gameStateManager.SetGameState(playing);
            }
        }

        /// <summary>
        /// Method called when pressing the back button in pauseMenu.
        /// </summary>
        public void OnPauseBackButton()
        {
            if ( gameStateManager.CurrentGameState == paused )
            {
                pausePanel.SetActive(false);
                gameStateManager.SetGameState(playing);
            }
        }

        /// <summary>
        /// Method called when the GameState is changed to InGame.
        /// Used to check if pausePanel or settingsPanel are active, and if they are
        /// disable them.
        /// </summary>
        private void ChangingGameStateToInGame()
        {
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
        /// Method called when play button in main menu is pressed.
        /// Used to deactivate the mainMenuPanel.
        /// </summary>
        public void OnPlayButton()
        {
            mainMenuPanel.SetActive(false);
        }

        public void OnSettingsButton()
        {
            TryToggleMenuAndPausePanel();

            if ( gameStateManager.CurrentGameState != settings )
            {
                gameStateManager.SetGameState(settings);
                ChangeSelectedButton(buttonHandler.SettingsDefaultButton, true);
            }
        }

        public void OnSettingsBackButton()
        {
            if ( gameStateManager.CurrentGameState == settings )
            {
                settingsPanel.SetActive(false);
                gameStateManager.ResetGameState();
                buttonHandler.SelectPreviousButton();
            }
        }

        private void TryToggleMenuAndPausePanel()
        {
            if ( gameStateManager.CurrentGameState == start )
            {
                mainMenuPanel.SetActive(!mainMenuPanel.activeSelf);
            }

            if ( gameStateManager.CurrentGameState == paused )
            {
                pausePanel.SetActive(!pausePanel.activeSelf);
            }
        }

        /// <summary>
        /// Method used to call the SetSelectedButton from MenuButtonHandler component.
        /// </summary>
        /// <param name="button">The button to select.</param>
        private void ChangeSelectedButton(GameObject button, bool storePrevious = false)
        {
            buttonHandler.SetSelectedButton(button, storePrevious);
        }

        /// <summary>
        /// Method called when entering the quit menu in pause menu.
        /// Used to set the selected button to quitDefaultButton.
        /// </summary>
        public void OnEnterQuitMenu()
        {
            ChangeSelectedButton(buttonHandler.QuitDefaultButton, true);
        }

        /// <summary>
        /// Method called when exiting the quit menu in pause menu.
        /// Used to set the selected button to pauseDefaultButton.
        /// </summary>
        public void OnExitQuitMenu()
        {
            buttonHandler.SelectPreviousButton();
        }
    }
}
