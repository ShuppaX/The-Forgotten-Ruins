using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

namespace BananaSoup.Utilities
{
    public class BaseSceneLoader : MonoBehaviour
    {
        [SerializeField, Scene] private int baseScene;

        private void Awake()
        {
            LoadBaseScene();
        }

        /// <summary>
        /// Go through every loaded Scene in the game. If BaseScene loaded, end the method. Otherwise load BaseScene as Additive.
        /// BaseScene contains GameObjects that are required in every scene like a camera or player.
        /// </summary>
        private void LoadBaseScene()
        {
            for ( int i = 0; i < SceneManager.sceneCount; i++ )
            {
                if ( SceneManager.GetSceneAt(i).buildIndex == baseScene )
                {
                    Debug.Log("Base Scene already loaded. End BaseScene loading.");
                    return;
                }
            }

            SceneManager.LoadSceneAsync(baseScene, LoadSceneMode.Additive);
        }
    }
}
