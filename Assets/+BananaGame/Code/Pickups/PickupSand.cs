using System;

namespace BananaSoup.PickupSystem
{
    public class PickupSand : Pickup
    {
        public static event Action OnEventLooted;

        public override void Loot()
        {
            PlayerBase.Instance.IsThrowableLooted = true;
            OnEventLooted.Invoke();

            DestroyPickup();
        }
    }
}
