using System.Collections;
using UnityEngine;

namespace BananaSoup
{
    public class PlayerHealth : Health
    {
        /// <summary>
        /// Override to base.DeathCoroutine to have players death have different actions
        /// from other objects.
        /// </summary>
        public override IEnumerator DeathCoroutine()
        {
            yield return BaseDeathRoutine = StartCoroutine(base.DeathCoroutine());

            NullCoroutine(BaseDeathRoutine);

            // TODO: Disable gravity etc.

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
