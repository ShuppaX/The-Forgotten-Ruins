using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class Health : MonoBehaviour, IHealth
    {

        private int currentHealth = 0;
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private int startingHealth = 3;
    
        public event Action<int> HealthChanged;


        public int CurrentHealth
        {
            get { return currentHealth; }
            private set
            {
                currentHealth = Mathf.Clamp(value, 0, maxHealth);
                if (HealthChanged != null)
                {
                    HealthChanged(currentHealth);
                }
            }

        }

        int IHealth.MaxHealth => maxHealth;
        public bool IsAlive => currentHealth > 0;
        
        
        public void Setup()
        {
            Reset();
        }

        public void IncreaseHealth(int amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Negative hp detected in IncreaseHealth");
            }
            currentHealth += amount;
        }

        public void DecreaseHEalth(int amount)
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            CurrentHealth = startingHealth;        }
    }
}
