using BananaSoup.SaveSystem;
using System;
using UnityEngine;

namespace BananaSoup.PickupSystem
{
    public class PickupDash : Pickup
    {
        public static event Action OnEventLooted;

        public override void Start()
        {
            playerPrefsKey = SaveManager.saveKeyDashPickup;
            CheckIsSaved(playerPrefsKey);
        }

        public override void Loot()
        {
            PlayerBase.Instance.IsDashLooted = true;

            if ( OnEventLooted != null )
            {
                OnEventLooted();
            }

            DisablePickup();
            SetToPlayerPrefs(playerPrefsKey);
        }
    }
}
