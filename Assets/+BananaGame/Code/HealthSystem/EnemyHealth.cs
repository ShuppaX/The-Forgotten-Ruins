using System.Collections;
using UnityEngine;

namespace BananaSoup
{
    public class EnemyHealth : Health
    {
        [SerializeField, Tooltip("Set true if the object should be destroyed on death.")]
        private bool _destroyOnDeath = false;

        /// <summary>
        /// Override to base.DeathCoroutine to have different death actions for enemies.
        /// </summary>
        public override IEnumerator DeathCoroutine()
        {
            yield return BaseDeathRoutine = StartCoroutine(base.DeathCoroutine());

            NullCoroutine(BaseDeathRoutine);

            if ( _destroyOnDeath )
            {
                Destroy(gameObject);
            }

            NullCoroutine(DeathRoutine);
        }

        /// <summary>
        /// Method which is called from DeathRoutine(), use this to start death animation
        /// and death sound.
        /// </summary>
        public override void OnDeath()
        {
            // TODO: Start animation
            // TODO: Play sound
        }
    }
}
