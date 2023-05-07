using BananaSoup.InteractSystem;
using UnityEngine;

namespace BananaSoup.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        [SerializeField] private bool showDebugLines = false;
        private LiftableRockAction[] rocks;

        // The Location of the player
        public static string saveKeyCheckpoint = "Checkpoint";

        // Skills and loots
        public static string saveKeyDashPickup = "DashPickup";
        public static string saveKeySparkPickup = "SparkPickup";
        public static string saveKeySandPickup = "SandPickup";
        public static string saveKeySwordPickup = "SwordPickup";

        // The player health
        public static string saveKeyHealth = "PlayerHealth";

        public LiftableRockAction[] GetRocks => rocks;
        public bool ShowDebugLines => showDebugLines;

        private void Awake()
        {
            if ( Instance == null )
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            rocks = FindObjectsOfType<LiftableRockAction>();
        }

        // NOTE: Remove this OnDisable when SaveSystem is done.
        private void OnDisable()
        {
            //SaveProgress();
        }

        public void SaveProgress()
        {
            if ( showDebugLines )
            {
                Debug.Log("Saving PlayerPrefs"); 
            }
            PlayerPrefs.Save();
        }

        // Save killed enemies, int
        // Save torch

        /// <summary>
        /// Checking and setting a Checkpoint to PlayerPrefs. Is value (index) is smaller
        /// than already saved, it won't try to set new one.
        /// </summary>
        /// <param name="key">String key for the PlayerPrefs</param>
        /// <param name="value">Value to be saved</param>
        public void SetCheckpoint(string key, int value)
        {
            if ( !PlayerPrefs.HasKey(key) || value > PlayerPrefs.GetInt(key) )
            {
                if ( showDebugLines )
                {
                    Debug.Log($"Setting {key}: {value} data to be saved"); 
                }
                PlayerPrefs.SetInt(key, value);
            }
        }

        public void SetInt(string key, int value)
        {
            if ( !PlayerPrefs.HasKey(key) || value != PlayerPrefs.GetInt(key) )
            {
                if ( showDebugLines )
                {
                    Debug.Log($"Setting {key}: {value} data to be saved"); 
                }
                PlayerPrefs.SetInt(key, value);
            }
        }

        public void SetVector3(string key, float value)
        {
            if ( showDebugLines )
            {
                Debug.Log($"Setting {key}: {value} data to be saved"); 
            }
            PlayerPrefs.SetFloat(key, value);
        }
    }
}
