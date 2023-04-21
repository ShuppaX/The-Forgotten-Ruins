using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace BananaSoup.Ability
{
    public class AbilityThrow : MonoBehaviour
    {
        [Tooltip("The transform where the ability's particle effect spawns.")]
        [SerializeField] private Transform handTransform;
        private List<ThrowBase> enabledAbilities = new List<ThrowBase>();
        private List<ThrowBase> disabledAbilities = new List<ThrowBase>();

        private ThrowBase currentAbility;

        public event UnityAction ThrowableChanged;

        public ThrowBase CurrentAbility
        {
            get => currentAbility;
        }

        public void ToggleAbilityUsability(ThrowBase ability)
        {
            enabledAbilities.Add(ability);
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

            currentAbility = enabledAbilities[0];

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
    }
}
