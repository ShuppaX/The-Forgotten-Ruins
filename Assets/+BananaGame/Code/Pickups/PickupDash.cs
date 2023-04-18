using System;
using UnityEngine;


namespace BananaSoup.PickupSystem
{
    public class PickupDash : Pickup
    {
        //public static event Action OnEventLooted;

        public override void Loot()
        {
            PlayerBase.Instance.IsDashLooted = true;

            // TODO: Enable event and listen it somewhere else to enable UI element
            //OnEventLooted.Invoke();

            DestroyPickup();
        }
    }
}
