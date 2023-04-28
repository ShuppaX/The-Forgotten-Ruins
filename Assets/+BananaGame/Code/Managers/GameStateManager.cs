using UnityEngine;
using System;

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

        public GameState PreviousGameState
        {
            get => previousGameState;
        }

        // NOTE: WHEN ADDING STATES DO NOT CHANGE THE ORDER
        public enum GameState
        {
            InMainMenu = 0, // Default GameState
            InGame = 1,
            Paused = 2,
            GameOver = 3
        }

        private void Awake()
        {
            if ( Instance == null )
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Method used to set a new GameState and to store the previous GameState so that
        /// it can be used when resetting GameState.
        /// </summary>
        /// <param name="newState">The GameState to set as current GameState.</param>
        public void SetGameState(GameState newState)
        {
            Debug.Log($"Storing {currentGameState} as previousGameState.");
            previousGameState = currentGameState;

            Debug.Log($"Setting currentGameState to be {newState}");
            currentGameState = newState;

            StateChanged();
        }

        /// <summary>
        /// Method to reset the GameState to the previous GameState.
        /// </summary>
        public void ResetGameState()
        {
            Debug.Log($"Setting currentGameState to {previousGameState}");
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
