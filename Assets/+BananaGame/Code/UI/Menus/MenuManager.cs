using BananaSoup.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup.UI.Menus
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField, Tooltip("In-game UI parent (In-Game_UI)")]
        private Canvas inGameUICanvas = null;

        private ObjectMenuType[] menuPanels = null;

        // References to main menu panels
        private GameObject mainMenuPanel = null;
        private GameObject settingsPanel = null;
        private GameObject pausePanel = null;

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
            Pause = 2
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
            InitializeMenuArray();
            InitializeMenuObjects();
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

            inGameUICanvas.enabled = false;
            playerBase.ToggleAllActions(false);
            settingsPanel.SetActive(false);
            pausePanel.SetActive(false);
        }

        private void InitializeMenuArray()
        {
            menuPanels = (ObjectMenuType[])FindObjectsOfType(typeof(ObjectMenuType));
        }

        private void InitializeMenuObjects()
        {
            mainMenuPanel = GetMenu(MenuType.MainMenu);
            settingsPanel = GetMenu(MenuType.Settings);
            pausePanel = GetMenu(MenuType.Pause);
        }

        private GameObject GetMenu(MenuType menuType)
        {
            for ( int i = 0; i < menuPanels.Length; i++ )
            {
                if ( menuPanels[i].GetComponent<ObjectMenuType>().MenuType == menuType )
                {
                    return menuPanels[i].gameObject;
                }
            }

            Debug.LogError($"No menu of type {menuType} could be found in menus[]!");

            return null;
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
                    break;
                case (inGame):
                    ReturnToInGame();
                    mainMenuPanel.SetActive(false);
                    inGameUICanvas.enabled = true;
                    playerBase.ToggleAllActions(true);
                    break;
                case (paused):
                    // TODO: Implement pause functionality (pause time for example)
                    pausePanel.SetActive(true);
                    inGameUICanvas.enabled = false;
                    break;
                case (gameOver):
                    // TODO: Make a deathscreen panel etc. and activate it here!
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
        /// If the GameState is inMainMenu then toggle mainMenuPanel button press.
        /// If the GameState is paused then toggle pausePanel on button press.
        /// </summary>
        public void ToggleSettings()
        {
            if ( gameStateManager.CurrentGameState == inMainMenu )
            {
                mainMenuPanel.SetActive(!mainMenuPanel.activeSelf);
            }

            if ( gameStateManager.CurrentGameState == paused )
            {
                pausePanel.SetActive(!pausePanel.activeSelf);
            }

            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }

        /// <summary>
        /// Method called from the Play button in main menu.
        /// </summary>
        public void OnPlayButton()
        {
            gameStateManager.SetGameState(inGame);
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
            if ( !pausePanel.activeSelf )
            {
                gameStateManager.SetGameState(paused);
            }
            else
            {
                gameStateManager.SetGameState(inGame);
            }
        }

        /// <summary>
        /// Method called when the GameState is changed to InGame.
        /// Used to check if pausePanel or settingsPanel are active, and if they are
        /// disable them.
        /// </summary>
        private void ReturnToInGame()
        {
            if ( pausePanel.activeSelf )
            {
                pausePanel.SetActive(false);
            }

            if ( settingsPanel.activeSelf )
            {
                settingsPanel.SetActive(false);
            }
        }
    }
}
