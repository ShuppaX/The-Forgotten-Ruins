using System;

namespace BananaSoup.PickupSystem
{
    public class PickupSword : Pickup
    {
        public static event Action OnEventLooted;

        public override void Loot()
        {
            PlayerBase.Instance.IsSwordLooted = true;
            OnEventLooted.Invoke();

            DisablePickup();
        }
    }
}
