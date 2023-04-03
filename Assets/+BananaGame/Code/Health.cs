using System;
using System.Collections;
using UnityEngine;

namespace BananaSoup
{
    public class Health : MonoBehaviour, IHealth
    {
        [SerializeField]
        private int _maxHealth = 3;
        [SerializeField]
        private int _startingHealth = 3;
        [SerializeField, Tooltip("The object can only take hits after this time has passed after a hit.")]
        private float _wasHitResetTimer = 1.5f;
        [SerializeField, Tooltip("Set true if the object should be destroyed on death.")]
        private bool _destroyOnDeath = false;
        // TODO: Decide if this is necessary.
        [SerializeField, Tooltip("The transition time to death.")]
        private float _deathTransitionTime = 2.0f;

        private int _currentHealth = 0;
        private int _latestHealth = 0;

        private bool _wasHit = false;

        private Coroutine _deathRoutine = null;

        public bool IsAlive => _currentHealth > 0;
        public bool WasHit
        {
            get => _wasHit;
            set => _wasHit = value;
        }

        private void Start()
        {
            Setup();
        }

        private void Update()
        {
            OnHealthChanged();
        }

        public void Setup()
        {
            Reset();
        }

        private void OnHealthChanged()
        {
            if ( _latestHealth != _currentHealth )
            {
                _latestHealth = _currentHealth;

                if ( _currentHealth > 0 )
                {
                    return;
                }

                if ( _deathRoutine == null )
                {
                    _deathRoutine = StartCoroutine(DeathRoutine());
                }
            }
        }

        public void IncreaseHealth(int amount)
        {
            if ( amount < 0 ) Debug.LogWarning("Negative hp detected in IncreaseHealth");

            if ( _currentHealth < _maxHealth )
            {
                _currentHealth += amount;
            }
        }

        public void DecreaseHealth(int amount)
        {
            if ( amount < 0 ) return;
            if ( _wasHit ) return;

            if ( _currentHealth > 0 )
            {
                _currentHealth -= amount;

                _wasHit = true;

                Invoke(nameof(ResetWasHit), _wasHitResetTimer);
            }
        }

        private void ResetWasHit()
        {
            _wasHit = false;
            //Debug.Log("Reset wasHit on +" + gameObject.name);
        }

        public void Reset()
        {
            _currentHealth = _startingHealth;
        }

        public virtual void OnDeath()
        {
            // TODO: Start animation
            // TODO: Play sound
        }

        private IEnumerator DeathRoutine()
        {
            OnDeath();
            yield return new WaitForSeconds(_deathTransitionTime);

            if ( _destroyOnDeath )
            {
                Destroy(gameObject);
            }
        }
    }
}