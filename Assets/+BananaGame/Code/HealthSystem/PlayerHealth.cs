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

        // References
        private PlayerBase playerBase = null;
        private PlayerStateManager psm = null;
        private GameStateManager gameStateManager = null;
        private RagdollOnDeath ragdollOnDeath = null;
        
        // Constant PlayerState used to change state on death.
        private const PlayerStateManager.PlayerState dead = PlayerStateManager.PlayerState.Dead;

        // Constant GameStates used to check in reset to compare if CurrentGameState is this.
        private const GameStateManager.GameState inGame = GameStateManager.GameState.InGame;

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

            gameStateManager = GameStateManager.Instance;
            if ( gameStateManager == null )
            {
                Debug.LogError("PlayerHealth couldn't find an Instance of GameStateManager!");
            }

            ragdollOnDeath = GetComponent<RagdollOnDeath>();
            if ( ragdollOnDeath == null )
            {
                Debug.LogError($"PlayerHealth couldn't find a RagdollOnDeath component on {name}!");
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

            ragdollOnDeath.EnableRagdoll();
            // TODO: Ragdoll or something?
            // TODO: Play sound
        }

        public override void Reset()
        {
            base.Reset();

            if ( gameStateManager.CurrentGameState == inGame )
            {
                playerBase.ToggleAllActions(true);
            }
        }
    }
}
