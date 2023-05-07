using UnityEngine;

namespace BananaSoup.SaveSystem
{
    public class PlayerSpawnManager : MonoBehaviour
    {
        public static PlayerSpawner[] spawners;
        private int spawnIndex = 0;

        public int SetSpawnIndex { set => spawnIndex = value; }
        public int GetSpawnersCount => spawners.Length;

        private void Awake()
        {
            spawners = GetComponentsInChildren<PlayerSpawner>();
            if ( spawners == null )
            {
                Debug.LogError(this + "'s Spawners array is null and can't be!");
                return;
            }
        }

        public void Setup()
        {
            // Spawn player to the corresponding checkpoint
            spawners[spawnIndex].TeleportPlayer();
        }
    }
}
