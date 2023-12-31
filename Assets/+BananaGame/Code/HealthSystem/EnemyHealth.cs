using System.Collections;
using UnityEngine;

namespace BananaSoup.HealthSystem
{
    public class EnemyHealth : Health
    {
        [SerializeField, Tooltip("Set true if the object should be destroyed on death.")]
        private bool _destroyOnDeath = false;

        private MeleeRaycast enemyBase = null;

        public override void Start()
        {
            base.Start();

            enemyBase = GetComponent<MeleeRaycast>();
            if ( enemyBase == null )
            {
                Debug.LogError($"No component of type MeleeRaycast was found on {name}!");
            }
        }

        /// <summary>
        /// Override to base.DeathCoroutine to have different death actions for enemies.
        /// </summary>
        public override IEnumerator DeathCoroutine()
        {
            yield return BaseDeathRoutine = StartCoroutine(base.DeathCoroutine());

            NullCoroutine(BaseDeathRoutine);

            if ( _destroyOnDeath )
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
            // TODO: Play sound
            enemyBase.DeathSequence();
            enemyBase.IsDead = true;
        }
        
        
    }
}
