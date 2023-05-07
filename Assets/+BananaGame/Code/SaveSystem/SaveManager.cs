using UnityEngine;

namespace BananaSoup.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }
        public static string saveKeyCheckpoint = "Checkpoint";
        public static string saveKeyDashPickup = "DashPickup";
        public static string saveKeySparkPickup = "SparkPickup";
        public static string saveKeySandPickup = "SandPickup";
        public static string saveKeySwordPickup = "SwordPickup";

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

        // NOTE: Save is OnDisable for faster testing
        private void OnDisable()
        {
            SaveProgress();
        }

        public void SaveProgress()
        {
            Debug.Log("Saving PlayerPrefs");
            PlayerPrefs.Save();
        }

        // Save Player unlocked skills, int
        // Save killed enemies, int
        // Save looted pickups, int
        // Save position of liftable rocks, float (vector3)
        // Save torch
        // Save player HP

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
                Debug.Log($"Setting {key}: {value} data to be saved");
                PlayerPrefs.SetInt(key, value);
            }
        }

        public void SetInt(string key, int value)
        {
            if ( !PlayerPrefs.HasKey(key) || value != PlayerPrefs.GetInt(key) )
            {
                Debug.Log($"Setting {key}: {value} data to be saved");
                PlayerPrefs.SetInt(key, value);
            }
        }
    }
}
