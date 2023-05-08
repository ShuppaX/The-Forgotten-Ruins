using BananaSoup.InteractSystem;
using System;
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
            int currentSpawnIndex = 0;
            spawnManager = FindObjectOfType<PlayerSpawnManager>();
            if ( spawnManager == null )
            {
                Debug.LogError($"{name} is missing a reference to a PlayerSpawnManager!");
            }

            //if ( PlayerPrefs.GetInt(SaveManager.saveKeyCheckpoint) > 0 )
            //{
            //    currentSpawnIndex = spawnManager.SpawnIndex;

            //    // Teleport player to "loading zone" to wait in safe.
            //    spawnManager.SpawnIndex = spawnManager.GetSpawnersCount - 1;
            //}
            //else
            //{
            //    spawnManager.SpawnIndex = 0;
            //}
            spawnManager.Setup();

            //if ( spawnManager.SpawnIndex == spawnManager.GetSpawnersCount - 1 )
            //{
            //    spawnManager.SpawnIndex = currentSpawnIndex;
            //}
        }

        public void OnLoadGame()
        {
            spawnManager = FindObjectOfType<PlayerSpawnManager>();
            if ( spawnManager == null )
            {
                Debug.LogError($"{name} is missing a reference to a PlayerSpawnManager!");
            }

            SetLiftableRocks();

            if ( PlayerPrefs.HasKey(SaveManager.saveKeyCheckpoint) )
            {
                spawnManager.SpawnIndex = PlayerPrefs.GetInt(SaveManager.saveKeyCheckpoint);
            }

            spawnManager.Setup();
        }

        private void SetLiftableRocks()
        {
            LiftableRockAction[] rocks = FindObjectsOfType<LiftableRockAction>();
            foreach ( var rock in rocks )
            {
                rock.OnLoad();
            }
        }
    }
}
