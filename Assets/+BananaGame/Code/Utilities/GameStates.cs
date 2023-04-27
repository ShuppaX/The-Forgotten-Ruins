using UnityEngine;

namespace BananaSoup
{
    public class GameStates : MonoBehaviour
    {
        public GameState gameState;

        public enum GameState
        {
            InMainMenu = 0,
            InGame = 2,
            Paused = 3,
            GameOver = 4
        }
    }
}
