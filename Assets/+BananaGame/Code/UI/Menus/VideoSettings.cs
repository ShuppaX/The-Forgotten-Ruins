using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace BananaSoup
{
    public class VideoSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullscreenToggle;

        private Resolution[] resolutions;
        private List<Resolution> filteredResolutions;

        private int currentRefreshRate;
        private int currentResolutionIndex = 0;

        private void Start()
        {
            InitializeResolutions();
            SetToggleOnStart();
        }

        public void ToggleFullscreen()
        {
            if ( Screen.fullScreenMode == FullScreenMode.Windowed)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                return;
            }

            if ( Screen.fullScreenMode == FullScreenMode.FullScreenWindow )
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }
        }

        private void SetToggleOnStart()
        {
            switch ( Screen.fullScreenMode )
            {
                case FullScreenMode.Windowed:
                    fullscreenToggle.SetIsOnWithoutNotify(false);
                    break;
                case FullScreenMode.FullScreenWindow:
                    fullscreenToggle.SetIsOnWithoutNotify(true);
                    break;
            }
        }

        private void InitializeResolutions()
        {
            resolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();

            resolutionDropdown.ClearOptions();
            currentRefreshRate = Screen.currentResolution.refreshRate;

            for ( int i = 0; i < resolutions.Length; i++ )
            {
                if ( resolutions[i].refreshRate == currentRefreshRate )
                {
                    filteredResolutions.Add(resolutions[i]);
                }
            }

            List<string> resolutionOptions = new List<string>();
            for ( int i = 0; i < filteredResolutions.Count; i++ )
            {
                string resolutionOption = filteredResolutions[i].width
                    + "x" + filteredResolutions[i].height
                    + " " + filteredResolutions[i].refreshRate + "Hz";

                resolutionOptions.Add(resolutionOption);

                if ( filteredResolutions[i].width == Screen.width
                    && filteredResolutions[i].height == Screen.height )
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(resolutionOptions);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution newResolution = filteredResolutions[resolutionIndex];

            switch ( Screen.fullScreenMode )
            {
                case FullScreenMode.Windowed:
                    
                    Screen.SetResolution(newResolution.width, newResolution.height, false);
                    break;
                case FullScreenMode.FullScreenWindow:
                    Screen.SetResolution(newResolution.width, newResolution.height, true);
                    break;
            }
        }
    }
}
