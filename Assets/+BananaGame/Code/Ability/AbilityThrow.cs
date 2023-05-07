using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace BananaSoup.Ability
{
    public class AbilityThrow : MonoBehaviour
    {
        [Tooltip("The transform where the ability's particle effect spawns.")]
        [SerializeField] private Transform handTransform;
        private List<ThrowBase> enabledAbilities = new List<ThrowBase>();
        private List<ThrowBase> disabledAbilities = new List<ThrowBase>();

        private ThrowBase currentAbility;

        private int activeAbilities = 0;

        public static event Action ThrowableChanged;

        public ThrowBase CurrentAbility
        {
            get => currentAbility;
        }

        public int ActiveAbilities
        {
            get => activeAbilities;
        }

        public void ToggleAbilityUsability(ThrowBase ability)
        {
            if ( enabledAbilities.Contains(ability) )
            {
                return;
            }

            enabledAbilities.Add(ability);
            OnEnableAbility();
            currentAbility = enabledAbilities[0];

            if ( ThrowableChanged != null )
            {
                ThrowableChanged();
            }

            disabledAbilities.Remove(ability);
        }

        private void Awake()
        {
            GetAllAbilitiesToList();
        }

        private void GetAllAbilitiesToList()
        {
            ThrowBase[] throwAbility = GetComponents<ThrowBase>();
            foreach ( ThrowBase ability in throwAbility )
            {
                disabledAbilities.Add(ability);
            }
        }

        public void OnChangeAbility(InputAction.CallbackContext context)
        {
            if ( !context.performed )
            {
                return;
            }

            if ( !PlayerBase.Instance.IsThrowableLooted )
            {
                return;
            }

            if ( enabledAbilities.Count > 1 )
            {
                var currentAbility = enabledAbilities[0];
                var nextAbility = enabledAbilities[1];
                enabledAbilities[0] = nextAbility;
                enabledAbilities[1] = currentAbility;
            }

            if ( enabledAbilities.Count > 0 )
            {
                currentAbility = enabledAbilities[0];
            }

            if ( ThrowableChanged != null )
            {
                ThrowableChanged();
            }
        }

        public void OnAbility(InputAction.CallbackContext context)
        {
            if ( !PlayerBase.Instance.IsThrowableLooted )
            {
                return;
            }

            if ( !PlayerBase.Instance.AreAbilitiesEnabled )
            {
                return;
            }

            if ( context.performed )
            {
                enabledAbilities[0].OnStartingToThrow(handTransform);
            }
        }

        private void OnEnableAbility()
        {
            activeAbilities++;
        }
    }
}
