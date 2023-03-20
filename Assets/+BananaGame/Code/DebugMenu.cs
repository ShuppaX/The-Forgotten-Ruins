using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BananaSoup
{
    public class DebugMenu : MonoBehaviour
    {
        [SerializeField] private GameObject debugMenuUI;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            if ( debugMenuUI == null )
            {
                Debug.LogError(this + "'s debugMenu is null and it shouldn't be!");
            }

            debugMenuUI.SetActive(false);
        }

        public void OnDebugMenuToggle()
        {
            if ( !debugMenuUI.activeInHierarchy )
            {
                debugMenuUI.SetActive( true );
            }
            else
            {
                debugMenuUI.SetActive ( false );
            }
        }

        public void OnResetLevel()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            Debug.Log("Current Scene is: " + currentScene + ". Reloading it.");
            SceneManager.LoadScene(currentScene);
        }
    }
}
