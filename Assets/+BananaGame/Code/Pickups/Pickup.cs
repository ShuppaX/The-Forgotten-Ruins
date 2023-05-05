using UnityEngine;

namespace BananaSoup.PickupSystem
{
    public class Pickup : MonoBehaviour, ILootable
    {
        public virtual void Loot() { }

        private void OnTriggerEnter(Collider other)
        {
            if ( other.GetComponent<PlayerBase>() != null )
            {
                Loot();
            }
        }

        // NOTE: Pool for health pickups?
        public void DisablePickup()
        {
            gameObject.SetActive(false);
        }
    }
}
