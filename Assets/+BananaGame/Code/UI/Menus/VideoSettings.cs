using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BananaSoup
{
    public class VideoSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;

        private Resolution[] resolutions;
        private List<Resolution> filteredResolutions;

        private int currentRefreshRate;
        private int currentResolutionIndex = 0;

        private void Start()
        {
            InitializeResolutions();
        }

        public void ToggleFullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
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
            if ( !Screen.fullScreen )
            {
                Screen.fullScreen = false;
            }

            Resolution newResolution = filteredResolutions[resolutionIndex];
            Screen.SetResolution(newResolution.width, newResolution.height, true);
        }
    }
}
