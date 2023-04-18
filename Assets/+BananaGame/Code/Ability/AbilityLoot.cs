using UnityEngine;

namespace BananaSoup.PickupSystem
{
    public class AbilityLoot : MonoBehaviour
    {
        [SerializeField, Tooltip("Set the radius of the loot collider here, not from the collider itself.")]
        private float lootRadius = 0.5f;

        [SerializeField] private SphereCollider looterCollider;

        private void OnValidate()
        {
            if ( looterCollider != null )
            {
                looterCollider.isTrigger = true;
                looterCollider.radius = lootRadius;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ( other.TryGetComponent(out ILootable pickup) )
            {
                pickup.Loot();
            }
        }
    }
}
