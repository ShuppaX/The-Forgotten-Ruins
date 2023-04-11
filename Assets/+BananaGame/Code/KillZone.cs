using UnityEngine;

namespace BananaSoup
{
    public class KillZone : MonoBehaviour
    {
        private Collider killTrigger;

        private void Start()
        {
            killTrigger = GetComponent<Collider>();
            if ( killTrigger == null )
            {
                Debug.LogError(name + " is missing a reference to a Kill Trigger!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.LogWarning(other.name + " fell of the map. " + this + " teleported it back to the start position.");
            other.transform.position = PlayerSpawnManager.spawners[0].transform.position;
        }
    }
}
