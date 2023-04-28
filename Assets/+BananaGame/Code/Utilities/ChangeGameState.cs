using BananaSoup.Managers;
using UnityEngine;

namespace BananaSoup.Utilities
{
    public class ChangeGameState : MonoBehaviour
    {
        [SerializeField]
        private GameStateManager.GameState stateToChangeTo;

        private GameStateManager gameStateManager;

        private void Start()
        {
            gameStateManager = GameStateManager.Instance;
            if ( gameStateManager == null )
            {
                Debug.LogError($"{name} couldn't find an instance of GameStateManager!");
            }
        }

        public void CallChangeGameState()
        {
            gameStateManager.SetGameState(stateToChangeTo);
        }
    }
}
