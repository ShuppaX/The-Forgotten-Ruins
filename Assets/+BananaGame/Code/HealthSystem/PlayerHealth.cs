using System.Collections;
using UnityEngine;
using BananaSoup.Managers;
using System;

namespace BananaSoup.HealthSystem
{
    public class PlayerHealth : Health
    {
        private bool godMode = false;
        public bool GodMode
        {
            get => godMode;
            set => godMode = value;
        }

        private PlayerBase playerBase = null;
        private PlayerStateManager psm = null;
        private DamageFlash damageFlash = null;

        private const PlayerStateManager.PlayerState dead = PlayerStateManager.PlayerState.Dead;

        public static event Action PlayerHealthChanged;

        public override void Start()
        {
            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError("PlayerHealth couldn't find an Instance of PlayerBase!");
            }

            psm = PlayerStateManager.Instance;
            if ( psm == null )
            {
                Debug.LogError("PlayerHealth couldn't find an Instance of PlayerStateManager!");
            }

            damageFlash = GetComponent<DamageFlash>();
            if ( damageFlash == null )
            {
                Debug.LogError(gameObject.name + $" is missing the component of type {typeof(Component).Name}!");
            }

            base.Start();
        }

        /// <summary>
        /// Override to base.DeathCoroutine to have players death have different actions
        /// from other objects.
        /// </summary>
        public override IEnumerator DeathCoroutine()
        {
            if ( godMode )
            {
                NullCoroutine(DeathRoutine);
                yield break;
            }

            psm.SetPlayerState(dead);
            yield return BaseDeathRoutine = StartCoroutine(base.DeathCoroutine());

            NullCoroutine(BaseDeathRoutine);

            Debug.Log("Player died!");

            NullCoroutine(DeathRoutine);
        }

        public override void DecreaseHealth(int amount)
        {
            base.DecreaseHealth(amount);

            if ( PlayerHealthChanged != null )
            {
                PlayerHealthChanged();
            }

            damageFlash.CallDamageFlash();
        }

        public override void IncreaseHealth(int amount)
        {
            base.IncreaseHealth(amount);

            if ( PlayerHealthChanged != null )
            {
                PlayerHealthChanged();
            }
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
