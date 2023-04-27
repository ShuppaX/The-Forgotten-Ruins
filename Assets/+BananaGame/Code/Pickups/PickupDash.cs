using System;


namespace BananaSoup.PickupSystem
{
    public class PickupDash : Pickup
    {
        public static event Action OnEventLooted;

        public override void Loot()
        {
            PlayerBase.Instance.IsDashLooted = true;

            if ( OnEventLooted != null )
            {
                OnEventLooted();
            }

            DestroyPickup();
        }
    }
}
