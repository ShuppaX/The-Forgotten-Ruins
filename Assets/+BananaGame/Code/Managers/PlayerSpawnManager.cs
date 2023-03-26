using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class PlayerSpawnManager : MonoBehaviour
    {
        [SerializeField] public static PlayerSpawner[] spawners;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            spawners = GetComponentsInChildren<PlayerSpawner>();
            if ( spawners == null )
            {
                Debug.LogError(this + "'s Spawners array is null and can't be!");
                return;
            }

            // Set the first Spawner as the starting point of the level in the Spawners array.
            spawners[0].IsStartingPoint = true;
        }
    }
}
