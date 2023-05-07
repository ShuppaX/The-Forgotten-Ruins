using BananaSoup.HealthSystem;
using UnityEngine;

namespace BananaSoup.SaveSystem
{
    public class Checkpoint : MonoBehaviour
    {
        private Collider saveTriggerZone;
        private SaveManager saveManager;
        private bool isCheckpointActivated;

        private void Start()
        {
            saveTriggerZone = GetComponent<Collider>();
            if ( saveTriggerZone == null )
            {
                Debug.LogError($"{name} is missing a reference to a Collider (Save Trigger Zone)!");
            }

            saveManager = FindObjectOfType<SaveManager>();
            if ( saveManager == null )
            {
                Debug.LogError($"{name} is missing a reference to a SaveManager!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ( other.TryGetComponent(out PlayerBase player) )
            {
                if ( isCheckpointActivated )
                {
                    return;
                }

                for ( int i = 0; i < PlayerSpawnManager.spawners.Length; i++ )
                {
                    if ( name.Equals(PlayerSpawnManager.spawners[i].name) )
                    {
                        isCheckpointActivated = true;

                        saveManager.SetCheckpoint(SaveManager.saveKeyCheckpoint, i);
                        break;
                    }
                }

                SettingAndSavingPlayerPrefs();
            }
        }

        private static void SettingAndSavingPlayerPrefs()
        {

            int currentPlayerHealth = PlayerBase.Instance.GetComponent<Health>().CurrentHealth;
            PlayerPrefs.SetInt(SaveManager.saveKeyHealth, currentPlayerHealth);

            SaveManager.Instance.SaveProgress();
        }
    }
}
