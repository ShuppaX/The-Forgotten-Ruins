using BananaSoup.Ability;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class AbilityThrow : MonoBehaviour
    {
        [Tooltip("The transform where the ability's particle effect spawns.")]
        [SerializeField] private Transform handTransform;
        [SerializeField] private List<AbilityThrowBase> throwAbilities;

        public void OnChangeAbility(InputAction.CallbackContext context)
        {
            if ( context.performed )
            {
                var currentAbility = throwAbilities[0];
                var nextAbility = throwAbilities[1];
                throwAbilities[0] = nextAbility;
                throwAbilities[1] = currentAbility;
            }
        }

        public void OnAbility(InputAction.CallbackContext context)
        {
            if ( !PlayerBase.Instance.AreAbilitiesEnabled )
            {
                return;
            }

            if ( context.performed )
            {
                throwAbilities[0].OnStartingToThrow(handTransform);
            }
        }
    }
}
