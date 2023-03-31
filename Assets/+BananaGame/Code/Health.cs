using System;
using UnityEngine;

namespace BananaSoup
{
    public class Health : MonoBehaviour, IHealth
    {
        private int _currentHealth = 0;
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private int startingHealth = 3;
        [SerializeField] private float wasHitResetTimer = 1.5f;

        private bool wasHit = false;

        public event Action<int> HealthChanged;

        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = Mathf.Clamp(value, 0, maxHealth);
                if ( HealthChanged != null ) HealthChanged(_currentHealth);
            }
        }

        int IHealth.MaxHealth => maxHealth;
        public bool IsAlive => _currentHealth > 0;
        public bool WasHit
        {
            get => wasHit;
            set => wasHit = value;
        }

        public void Setup()
        {
            Reset();
        }

        public void IncreaseHealth(int amount)
        {
            if ( amount < 0 ) Debug.LogWarning("Negative hp detected in IncreaseHealth");
            _currentHealth += amount;
        }

        public void DecreaseHealth(int amount)
        {
            if ( amount < 0 ) return;
            if ( wasHit ) return;

            CurrentHealth -= amount;
            wasHit = true;

            Invoke(nameof(ResetWasHit), wasHitResetTimer);
        }

        private void ResetWasHit()
        {
            wasHit = false;
            Debug.Log("Reset wasHit on +" + gameObject.name);
        }

        public void Reset()
        {
            CurrentHealth = startingHealth;
        }

        public virtual void OnDeath()
        {
            // TODO: Start animation
            // TODO: Play sound
        }
    }
}