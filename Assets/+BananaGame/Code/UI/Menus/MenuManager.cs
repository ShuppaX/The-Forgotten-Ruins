using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.UI.Menus
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField, Tooltip("In-game UI parent (In-Game_UI)")]
        private GameObject inGameUI = null;

        private ObjectMenuType[] menus = null;

        // References to main menu panels
        private GameObject mainMenuPanel = null;
        private GameObject settingsPanel = null;
        private GameObject pausePanel = null;

        public enum MenuType
        {
            MainMenu = 0,
            Settings = 1,
            Pause = 2
        }

        private void Awake()
        {
            InitializeMenuArray();
            InitializeMenuObjects();
        }

        private void Start()
        {
            mainMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            pausePanel.SetActive(false);
        }

        private void InitializeMenuArray()
        {
            menus = (ObjectMenuType[])FindObjectsOfType(typeof(ObjectMenuType));

            for ( int i = 0; i < menus.Length; i++ )
            {
                Debug.Log($"{menus[i].gameObject.name}");
            }
        }

        private void InitializeMenuObjects()
        {
            mainMenuPanel = GetMenu(MenuType.MainMenu);
            settingsPanel = GetMenu(MenuType.Settings);
            pausePanel = GetMenu(MenuType.Pause);
        }

        private GameObject GetMenu(MenuType menuType)
        {
            for ( int i = 0; i < menus.Length; i++ )
            {
                if ( menus[i].GetComponent<ObjectMenuType>().MenuType == menuType )
                {
                    return menus[i].gameObject;
                }
            }

            Debug.LogError($"No menu of type {menuType} could be found in menus[]!");

            return null;
        }
    }
}
