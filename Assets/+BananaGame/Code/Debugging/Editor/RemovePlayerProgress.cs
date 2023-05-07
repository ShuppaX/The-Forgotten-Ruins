using UnityEngine;
using UnityEditor;
using BananaSoup.SaveSystem;
using BananaSoup.PickupSystem;

namespace BananaSoup
{
    public class RemovePlayerProgress : MonoBehaviour
    {
        [MenuItem("Banana Soup/Clear Player Progress")]
        private static void ClearProgress()
        {
            ClearPlayerPrefs(SaveManager.saveKeyCheckpoint);
            ClearPlayerPrefs(SaveManager.saveKeyDashPickup);
            ClearPlayerPrefs(SaveManager.saveKeySparkPickup);
            ClearPlayerPrefs(SaveManager.saveKeySwordPickup);
            ClearPlayerPrefs(SaveManager.saveKeySandPickup);
            ClearPlayerPrefs(SaveManager.saveKeyHealth);

            PickupHealth[] healths = FindObjectsOfType<PickupHealth>();
            foreach ( var heart in healths )
            {
                ClearPlayerPrefs(heart.name);
            }
        }

        private static void ClearPlayerPrefs(string key)
        {
            if ( PlayerPrefs.HasKey(key) )
            {
                PlayerPrefs.DeleteKey(key);
                Debug.Log($"PlayerPrefs [{key}] cleared");
            }
        }
    }
}
