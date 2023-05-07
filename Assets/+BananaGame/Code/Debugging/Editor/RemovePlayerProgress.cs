using UnityEngine;
using UnityEditor;
using BananaSoup.SaveSystem;
using BananaSoup.PickupSystem;
using BananaSoup.InteractSystem;

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

            LiftableRockAction[] rocks = FindObjectsOfType<LiftableRockAction>();
            foreach ( var rock in rocks )
            {
                ClearPlayerPrefs(rock.name + "PosX");
                ClearPlayerPrefs(rock.name + "PosY");
                ClearPlayerPrefs(rock.name + "PosZ");
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
