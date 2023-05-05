using BananaSoup.DamageSystem;
using System;
using System.Collections;
using UnityEngine;

namespace BananaSoup.HealthSystem
{
    public class Health : MonoBehaviour, IHealth
    {
        [SerializeField]
        private int _maxHealth = 3;
        [SerializeField]
        private int _startingHealth = 3;
        [Space]
        [SerializeField, Tooltip("The object can only take hits after this time has passed after a hit.")]
        private float _wasHitResetTime = 1.5f;
        [Space]
        [SerializeField, Tooltip("The transition time to death.")]
        private float _deathTransitionTime = 2.0f;

        private int _currentHealth = 0;

        private bool _wasHit = false;

        private DamageFlash damageFlash = null;

        private Coroutine _deathRoutine = null;
        private Coroutine _baseDeathRoutine = null;

        public event Action<int> HealthChanged;

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                if ( HealthChanged != null )
                {
                    HealthChanged(_currentHealth);
                }
            }
        }

        public bool IsAlive => _currentHealth > 0;

        public bool WasHit
        {
            get => _wasHit;
            set => _wasHit = value;
        }

        public Coroutine DeathRoutine
        {
            get => _deathRoutine;
            set => _deathRoutine = value;
        }

        public Coroutine BaseDeathRoutine
        {
            get => _baseDeathRoutine;
            set => _baseDeathRoutine = value;
        }

        public int MaxHealth
        {
            get => _maxHealth;
        }

        private void OnEnable()
        {
            HealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            HealthChanged -= OnHealthChanged;

            NullCoroutine(_baseDeathRoutine);
            NullCoroutine(_deathRoutine);
        }

        public void NullCoroutine(Coroutine routine)
        {
            if ( routine != null )
            {
                StopCoroutine(routine);
                routine = null;
            }
        }

        public virtual void Start()
        {
            Setup();

            damageFlash = GetComponent<DamageFlash>();
            if ( damageFlash == null )
            {
                Debug.LogError($"{gameObject.name} is missing the component of type {typeof(DamageFlash).Name}!");
            }
        }

        public void Setup()
        {
            Reset();
        }

        /// <summary>
        /// Method used to check if the objects currentHealth is above 0 when it is changed
        /// if it is return, when it isn't start the objects DeathRoutine.
        /// </summary>
        private void OnHealthChanged(int health)
        {
            //Debug.Log(gameObject.name + "'s CurrentHealth changed!");
            if ( health > 0 )
            {
                return;
            }

            if ( _deathRoutine == null )
            {
                _deathRoutine = StartCoroutine(DeathCoroutine());
            }
        }

        /// <summary>
        /// Method used to increase the objects currentHealth if it's less than maxHealth.
        /// </summary>
        /// <param name="amount">The (positive) amount with how much the CurrentHealth should be increased.</param>
        public virtual void IncreaseHealth(int amount)
        {
            if ( amount < 0 )
            {
                Debug.LogWarning("Negative amount given to IncreaseHealth on " + gameObject.name + "!");
            }

            CurrentHealth += amount;
        }

        /// <summary>
        /// Method used to decrease the objects currentHealth if it's more than 0.
        /// </summary>
        /// <param name="amount">The amount how much the CurrentHealth should be decreased.</param>
        public virtual void DecreaseHealth(int amount)
        {
            if ( amount < 0 )
            {
                Debug.LogWarning("Negative amount given to DecreaseHealth on " + gameObject.name + "!");
            }

            if ( CurrentHealth <= 0 )
            {
                return;
            }

            if ( _wasHit )
            {
                //Debug.Log(gameObject.name + "was hit recently and can't take more damage!");
                return;
            }

            //Debug.Log(gameObject.name + " took " + amount + " damage!");
            CurrentHealth -= amount;
            damageFlash.CallDamageFlash();
            _wasHit = true;
            Invoke(nameof(ResetWasHit), _wasHitResetTime);
        }

        /// <summary>
        /// Method used to reset wasHit. (Called from DecreaseHealth after reset timer.)
        /// </summary>
        private void ResetWasHit()
        {
            //Debug.Log(gameObject.name + " called ResetWasHit");
            _wasHit = false;
            //Debug.Log("Reset wasHit on +" + gameObject.name);
        }

        /// <summary>
        /// Method used to reset the objects after death / on Start.
        /// </summary>
        public virtual void Reset()
        {
            _currentHealth = _startingHealth;
        }

        /// <summary>
        /// The base for the DeathRoutine. Overwrite this in inheriting scripts.
        /// </summary>
        public virtual IEnumerator DeathCoroutine()
        {
            OnDeath();
            yield return new WaitForSeconds(_deathTransitionTime);
        }

        /// <summary>
        /// Overwrite this in inheriting scripts with corresponding death sound and
        /// death animation.
        /// </summary>
        public virtual void OnDeath()
        {
        }
    }
}