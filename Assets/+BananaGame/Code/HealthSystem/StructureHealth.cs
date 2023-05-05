using System.Collections;
using UnityEngine;

namespace BananaSoup.HealthSystem
{
    public class StructureHealth : Health
    {
        [SerializeField, Tooltip("Set true if the object should be destroyed on death.")]
        private bool _disableOnDeath = false;

        public override void Start()
        {
            Setup();
        }

        /// <summary>
        /// Method used to decrease the objects currentHealth if it's more than 0.
        /// </summary>
        /// <param name="amount">The amount how much the CurrentHealth should be decreased.</param>
        public override void DecreaseHealth(int amount)
        {
            if ( amount < 0 )
            {
                Debug.LogWarning("Negative amount given to DecreaseHealth on " + gameObject.name + "!");
            }

            Debug.Log(gameObject.name + " took " + amount + " damage!");
            CurrentHealth -= amount;
        }

        /// <summary>
        /// Override to base.DeathCoroutine to have different death actions for enemies.
        /// </summary>
        public override IEnumerator DeathCoroutine()
        {
            yield return BaseDeathRoutine = StartCoroutine(base.DeathCoroutine());

            NullCoroutine(BaseDeathRoutine);

            if ( _disableOnDeath )
            {
                gameObject.SetActive(false);
            }

            NullCoroutine(DeathRoutine);
        }

        /// <summary>
        /// Method which is called from DeathRoutine(), use this to start death animation
        /// and death sound.
        /// </summary>
        public override void OnDeath()
        {
            
        }
    }
}
