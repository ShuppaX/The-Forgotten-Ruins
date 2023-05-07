using BananaSoup.InteractSystem;
using BananaSoup.PickupSystem;
using BananaSoup.SaveSystem;
using UnityEngine;

namespace BananaSoup.SaveSystem
{
    public class RemoveSaveData : MonoBehaviour
    {
        public void OnClearProgress()
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

            // HACK: Calling this from here to ensure New Game works in final presentation
            PlayerSpawnManager spawnManager;
            spawnManager = FindObjectOfType<PlayerSpawnManager>();
            if ( spawnManager == null )
            {
                Debug.LogError($"{name} is missing a reference to a PlayerSpawnManager!");
            }
            spawnManager.SetSpawnIndex = 0;
            spawnManager.Setup();
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
