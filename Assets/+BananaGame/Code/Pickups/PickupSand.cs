using BananaSoup.SaveSystem;
using System;

namespace BananaSoup.PickupSystem
{
    public class PickupSand : Pickup
    {
        public static event Action OnEventLooted;

        public override void Start()
        {
            playerPrefsKey = SaveManager.saveKeySandPickup;
            CheckIsSaved(playerPrefsKey);
        }

        public override void Loot()
        {
            PlayerBase.Instance.IsThrowableLooted = true;

            if ( OnEventLooted != null )
            {
                OnEventLooted();
            }

            DisablePickup();
            SetToPlayerPrefs(playerPrefsKey);
        }
    }
}
