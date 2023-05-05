using BananaSoup.HealthSystem;
using System;
using UnityEngine;

namespace BananaSoup.PickupSystem
{
    public class PickupHealth : Pickup
    {
        [SerializeField] private int healAmount = 1;
        //public static event Action OnEventLooted;

        public override void Loot()
        {
            //OnEventLooted.Invoke();

            PlayerHealth playerHealth = PlayerBase.Instance.GetComponent<PlayerHealth>();
            playerHealth.IncreaseHealth(healAmount);

            DisablePickup();
        }
    }
}
