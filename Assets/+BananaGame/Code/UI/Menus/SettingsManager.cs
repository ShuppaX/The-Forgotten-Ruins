using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField, Tooltip("")]
        private GameObject audioPanel = null;

        [SerializeField, Tooltip("")]
        private GameObject gamepadPanel = null;

        [SerializeField, Tooltip("")]
        private GameObject kbmPanel = null;

        public void ToggleAudioPanelActive()
        {
            if ( gamepadPanel.activeSelf )
            {
                gamepadPanel.SetActive(false);
            }

            if ( kbmPanel.activeSelf )
            {
                kbmPanel.SetActive(false);
            }

            if ( !audioPanel.activeSelf )
            {
                audioPanel.SetActive(true);
            }
        }

        public void ToggleGamepadPanelActive()
        {
            if ( audioPanel.activeSelf )
            {
                audioPanel.SetActive(false);
            }

            if ( kbmPanel.activeSelf )
            {
                kbmPanel.SetActive(false);
            }

            if ( !gamepadPanel.activeSelf )
            {
                gamepadPanel.SetActive(true);
            }
        }

        public void ToggleKBMPanelActive()
        {
            if ( audioPanel.activeSelf )
            {
                audioPanel.SetActive(false);
            }

            if ( gamepadPanel.activeSelf )
            {
                gamepadPanel.SetActive(false);
            }

            if ( !kbmPanel.activeSelf )
            {
                kbmPanel.SetActive(true);
            }
        }

        public void TogglePanelsOnBack()
        {
            if ( !audioPanel.activeSelf )
            {
                audioPanel.SetActive(true);
            }

            if ( gamepadPanel.activeSelf )
            {
                gamepadPanel.SetActive(false);
            }

            if ( kbmPanel.activeSelf )
            {
                kbmPanel.SetActive(false);
            }
        }
    }
}
