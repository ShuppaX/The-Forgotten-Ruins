using BananaSoup.HealthSystem;
using BananaSoup.SaveSystem;
using System;
using UnityEngine;

namespace BananaSoup.PickupSystem
{
    public class PickupHealth : Pickup
    {
        [SerializeField] private int healAmount = 1;

        public override void Start()
        {
            playerPrefsKey = name;
            CheckIsSaved(playerPrefsKey);
        }

        public override void Loot()
        {
            if ( !PlayerPrefs.HasKey(playerPrefsKey) && PlayerPrefs.GetInt(playerPrefsKey) != isLooted )
            {
                PlayerHealth playerHealth = PlayerBase.Instance.GetComponent<PlayerHealth>();
                playerHealth.IncreaseHealth(healAmount); 
            }

            DisablePickup();
            SetToPlayerPrefs(playerPrefsKey);
        }
    }
}
