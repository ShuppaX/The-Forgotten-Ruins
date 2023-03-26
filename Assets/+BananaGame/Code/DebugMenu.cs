using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BananaSoup
{
    public class DebugMenu : MonoBehaviour
    {
        [SerializeField] private GameObject debugMenuParent;
        [Header("Panels")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject teleportPanel;
        [Header("Teleport buttons")]
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private GameObject teleportButtonsParent;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            SetUIObjectInactive(debugMenuParent, true);
            SetUIObjectInactive(menuPanel, false);
            SetUIObjectInactive(teleportPanel, false);
            InstantiateTeleportingLocations();
        }

        private void SetUIObjectInactive(GameObject gameObjectUI, bool value)
        {
            if ( gameObjectUI == null )
            {
                Debug.LogError(this + "'s " + gameObjectUI.name + " is null and it shouldn't be!");
            }

            gameObjectUI.SetActive(value);
        }

        private void InstantiateTeleportingLocations()
        {
            for ( int i = 0; i < PlayerSpawnManager.spawners.Length; i++ )
            {
                var spawnPoint = PlayerSpawnManager.spawners[i];

                // Instantiate button
                GameObject button = Instantiate(buttonPrefab, teleportButtonsParent.transform);
                button.name = spawnPoint.name;

                // Set button's text according to the teleport location name
                button.GetComponentInChildren<TMP_Text>().text = spawnPoint.name;

                // Set OnClick listener
                button.GetComponent<Button>().onClick.AddListener(spawnPoint.TeleportPlayer);
            }
        }

        #region Toggle UI Panels
        public void OnDebugPanelToggle()
        {
            if ( !menuPanel.activeInHierarchy )
            {
                menuPanel.SetActive(true);
            }
            else
            {
                menuPanel.SetActive(false);
                teleportPanel.SetActive(false);
            }
        }

        public void OnTeleportPanelToggle()
        {
            if ( !teleportPanel.activeInHierarchy )
            {
                teleportPanel.SetActive(true);
            }
            else
            {
                teleportPanel.SetActive(false);
            }
        }
        #endregion

        public void OnResetLevel()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            Debug.Log("Current Scene is: " + currentScene + ". Reloading it.");
            SceneManager.LoadScene(currentScene);
        }

        public void OnExitGame()
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}
