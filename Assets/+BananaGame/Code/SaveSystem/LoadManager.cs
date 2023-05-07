using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.SaveSystem
{
    public class LoadManager : MonoBehaviour
    {
        private PlayerSpawnManager spawnManager;

        private void Start()
        {
            spawnManager = FindObjectOfType<PlayerSpawnManager>();
            if ( spawnManager == null )
            {
                Debug.LogError($"{name} is missing a reference to a PlayerSpawnManager!");
            }

            if ( PlayerPrefs.HasKey(SaveManager.saveKeyCheckpoint) )
            {
                spawnManager.SetSpawnIndex = PlayerPrefs.GetInt(SaveManager.saveKeyCheckpoint);
            }

            spawnManager.Setup();
        }
    }
}
