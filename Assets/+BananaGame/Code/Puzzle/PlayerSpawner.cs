using UnityEngine;

namespace BananaSoup.SaveSystem
{
    public class PlayerSpawner : MonoBehaviour
    {
        public void TeleportPlayer()
        {
            if ( PlayerBase.Instance != null )
            {
                PlayerBase.Instance.transform.position = transform.position;
                PlayerBase.Instance.transform.rotation = transform.rotation;
            }
            else
            {
                Debug.LogWarning("The player prefab not found. " + gameObject + " couldn't relocate the player prefab.");
            }
        }
    }
}
