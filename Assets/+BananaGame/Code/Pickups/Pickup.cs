using UnityEngine;

namespace BananaSoup.PickupSystem
{
    public class Pickup : MonoBehaviour, ILootable
    {
        public virtual void Loot() { }

        public void DestroyPickup()
        {
            Destroy(gameObject);
        }
    }
}
