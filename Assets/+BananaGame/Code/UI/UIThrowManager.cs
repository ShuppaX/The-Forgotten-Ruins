using BananaSoup.Ability;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BananaSoup.UI
{
    public class UIThrowManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject throwIndicator;
        private Image currentImage;

        private PlayerBase playerBase = null;

        private ThrowBase currentAbility = null;

        private void Start()
        {
            currentImage = throwIndicator.GetComponent<Image>();
            playerBase = PlayerBase.Instance;

            currentAbility = GetDependency<AbilityThrow>().CurrentAbility;
        }

        private T GetDependency<T>(PlayerBase instance = null) where T : Component
        {
            T component;

            if ( instance != null )
            {
                component = instance.GetComponent<T>();
            }
            else
            {
                component = GetComponent<T>();
            }

            if ( component == null )
            {
                Debug.LogError($"The component of type {typeof(T).Name} couldn't be found on the " + instance.name
                    + " for the " + gameObject.name + "!");
            }

            return component;
        }

        public void UpdateImage(Image newImage)
        {
            currentImage = newImage;
        }
    }
}
