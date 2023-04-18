using BananaSoup.PickupSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup.Ability
{
    public class AbilityThrow : MonoBehaviour
    {
        [Tooltip("The transform where the ability's particle effect spawns.")]
        [SerializeField] private Transform handTransform;
        [SerializeField] private List<ThrowBase> enabledAbilities;
        [SerializeField] private List<ThrowBase> disabledAbilities;

        public void ToggleAbilityUsability(ThrowBase ability)
        {
            enabledAbilities.Add(ability);
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

            if ( enabledAbilities.Count > 1 )
            {
                var currentAbility = enabledAbilities[0];
                var nextAbility = enabledAbilities[1];
                enabledAbilities[0] = nextAbility;
                enabledAbilities[1] = currentAbility;
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
