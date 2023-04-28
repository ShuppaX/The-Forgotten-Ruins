using BananaSoup.Managers;
using UnityEngine;

namespace BananaSoup.UI.Menus
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField, Tooltip("In-game UI parent (In-Game_UI)")]
        private GameObject inGameUI = null;

        private ObjectMenuType[] menuPanels = null;

        // References to main menu panels
        private GameObject mainMenuPanel = null;
        private GameObject settingsPanel = null;
        private GameObject pausePanel = null;

        // Reference to GameStateManager
        private GameStateManager gameStateManager;

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
                case (inMainMenu):
                    ToggleGameObject(inGameUI, false);
                    ToggleGameObject(mainMenuPanel, true);
                    break;
                case (inGame):
                    ToggleGameObject(mainMenuPanel, false);
                    ToggleGameObject(inGameUI, true);
                    break;
                case (paused):
                    ToggleGameObject(pausePanel, true);
                    break;
                case (gameOver):
                    // TODO: Make a deathscreen panel etc. and activate it here!
                    break;
                default:
                    Debug.LogError($"GameStateManager has an incorrect GameState! This is a bug!");
                    break;
            }
        }

        /// <summary>
        /// Method used to set a GameObject active or inactive.
        /// </summary>
        /// <param name="panel">The GameObject to activate/disable.</param>
        /// <param name="value">True to activate, false to disable.</param>
        private void ToggleGameObject(GameObject panel, bool value)
        {
            panel.SetActive(value);
        }

        private void SubscribeEvent()
        {
            GameStateManager.OnGameStateChanged += GameStateChanged;
        }

        private void UnsubscribeEvent()
        {
            GameStateManager.OnGameStateChanged -= GameStateChanged;
        }

        public void ToggleSettings()
        {

        }
    }
}
