using System.Collections;
using UnityEngine;

namespace BananaSoup
{
    public class PlayerHealth : Health
    {
        private PlayerBase playerBase = null;
        //private Rigidbody rb = null;

        public override void Start()
        {
            playerBase = PlayerBase.Instance;

            if ( playerBase == null )
            {
                Debug.LogError("PlayerHealth couldn't find an Instance of PlayerBase!");
            }

            //rb = GetDependency<Rigidbody>();

            base.Start();
        }

        /// <summary>
        /// Method to simplify getting components and to throw an error if it's null
        /// this improves readability.
        /// This method is by Sami Kojo-Fry from 2023 Tank Game.
        /// </summary>
        /// <typeparam name="T">The name of the component to get.</typeparam>
        /// <returns>The wanted component if it's found.</returns>
        private T GetDependency<T>() where T : Component
        {
            T component = GetComponent<T>();
            if ( component == null )
            {
                Debug.LogError($"The component of type {typeof(T).Name} couldn't be found on the " + gameObject.name + "!");
            }

            return component;
        }

        /// <summary>
        /// Override to base.DeathCoroutine to have players death have different actions
        /// from other objects.
        /// </summary>
        public override IEnumerator DeathCoroutine()
        {
            yield return BaseDeathRoutine = StartCoroutine(base.DeathCoroutine());

            NullCoroutine(BaseDeathRoutine);

            Debug.Log("Player died!");

            NullCoroutine(DeathRoutine);
        }

        /// <summary>
        /// Method which is called from DeathRoutine(), use this to start death animation
        /// and death sound.
        /// </summary>
        public override void OnDeath()
        {
            playerBase.ToggleAllActions(false);

            // TODO: Start animation
            // TODO: Play sound
        }

        public override void Reset()
        {
            base.Reset();

            playerBase.ToggleAllActions(true);
        }
    }
}
