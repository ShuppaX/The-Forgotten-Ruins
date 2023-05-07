using BananaSoup.SaveSystem;
using System;

namespace BananaSoup.PickupSystem
{
    public class PickupSword : Pickup
    {
        public static event Action OnEventLooted;

        public override void Start()
        {
            playerPrefsKey = SaveManager.saveKeySwordPickup;
            CheckIsSaved(playerPrefsKey);
        }

        public override void Loot()
        {
            PlayerBase.Instance.IsSwordLooted = true;

            if ( OnEventLooted != null )
            {
                OnEventLooted();
            }

            DisablePickup();
            SetToPlayerPrefs(playerPrefsKey);
        }
    }
}
