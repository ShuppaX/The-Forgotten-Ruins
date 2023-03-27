using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class Health : MonoBehaviour, IHealth
    {

        private int _currentHealth = 0;
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private int startingHealth = 3;
    
        public event Action<int> HealthChanged;
        
        public int CurrentHealth
        {
            get { return _currentHealth; }
            private set
            {
                _currentHealth = Mathf.Clamp(value, 0, maxHealth);
                if (HealthChanged != null)
                {
                    HealthChanged(_currentHealth);
                }
            }

        }

        int IHealth.MaxHealth => maxHealth;
        public bool IsAlive => _currentHealth > 0;
        
        
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
            _currentHealth += amount;
        }

        public void DecreaseHealth(int amount)
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            CurrentHealth = startingHealth;        }
    }
}
