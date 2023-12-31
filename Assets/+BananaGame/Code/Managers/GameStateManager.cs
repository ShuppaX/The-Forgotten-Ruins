using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace BananaSoup.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        private GameState currentGameState;
        private GameState previousGameState;

        public static event Action OnGameStateChanged;

        public GameState CurrentGameState
        {
            get => currentGameState;
        }

        // NOTE: WHEN ADDING STATES DO NOT CHANGE THE ORDER
        public enum GameState
        {
            Start = 0, // Default GameState
            Playing = 1,
            Paused = 2,
            GameOver = 3,
            Settings = 4
        }

        private void Awake()
        {
            if ( Instance == null )
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            if ( !Debug.isDebugBuild || !Application.isEditor )
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                InputSystem.DisableDevice(Mouse.current);
            }
            else
            {
                InputSystem.EnableDevice(Mouse.current);
            }

            //InputSystem.EnableDevice(Mouse.current);
        }

        /// <summary>
        /// Method used to set a new GameState and to store the previous GameState so that
        /// it can be used when resetting GameState.
        /// </summary>
        /// <param name="newState">The GameState to set as current GameState.</param>
        public void SetGameState(GameState newState)
        {
            previousGameState = currentGameState;
            currentGameState = newState;
            StateChanged();
        }

        public void ResetGameState()
        {
            currentGameState = previousGameState;
            StateChanged();
        }

        /// <summary>
        /// Method used to invoke the event Action OnStateChanged if it isn't null.
        /// </summary>
        private void StateChanged()
        {
            if ( OnGameStateChanged != null )
            {
                OnGameStateChanged();
            }
        }
    }
}
