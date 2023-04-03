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

        /// <summary>
        /// Method used to check if the objects currentHealth is above 0 when it is changed
        /// if it is return, when it isn't start the objects DeathRoutine.
        /// </summary>
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

        /// <summary>
        /// Method used to increase the objects currentHealth if it's less than maxHealth.
        /// </summary>
        /// <param name="amount">The (positive) amount with how much the currentHealth should be increased.</param>
        public void IncreaseHealth(int amount)
        {
            if ( amount < 0 ) Debug.LogWarning("Negative hp detected in IncreaseHealth");

            if ( _currentHealth < _maxHealth )
            {
                _currentHealth += amount;
            }
        }

        /// <summary>
        /// Method used to decrease the objects currentHealth if it's more than 0.
        /// </summary>
        /// <param name="amount">The amount how much the currentHealth should be decreased.</param>
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

        /// <summary>
        /// Method used to reset wasHit. (Called from DecreaseHealth after reset timer.)
        /// </summary>
        private void ResetWasHit()
        {
            _wasHit = false;
            //Debug.Log("Reset wasHit on +" + gameObject.name);
        }

        /// <summary>
        /// Method used to reset the objects health.
        /// </summary>
        public void Reset()
        {
            _currentHealth = _startingHealth;
        }

        /// <summary>
        /// Method which is called from DeathRoutine(), used to start death animation
        /// and death sound.
        /// </summary>
        public virtual void OnDeath()
        {
            // TODO: Start animation
            // TODO: Play sound
        }
        
        /// <summary>
        /// DeathRoutine which calls OnDeath and then waits for a set transition time,
        /// then destroys the gameObject if destroyOnDeath is set to true.
        /// </summary>
        /// <returns></returns>
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