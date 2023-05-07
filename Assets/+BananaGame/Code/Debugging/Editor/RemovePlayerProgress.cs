using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BananaSoup.SaveSystem;

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
