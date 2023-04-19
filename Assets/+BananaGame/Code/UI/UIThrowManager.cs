using BananaSoup.Ability;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BananaSoup.UI
{
    public class UIThrowManager : MonoBehaviour
    {
        [SerializeField]
        private Image throwIndicator;
        [SerializeField]
        private Sprite noThrowable;
        [SerializeField]
        private Color noThrowableColor;

        private PlayerBase playerBase = null;
        private AbilityThrow abilityThrow = null;

        private ThrowBase currentThrowable = null;

        private void OnEnable()
        {
            TrySubscribing();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Start()
        {
            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError(gameObject.name + " couldn't access an instance of PlayerBase!");
            }

            abilityThrow = GetDependency<AbilityThrow>(playerBase);

            TrySubscribing();

            if ( currentThrowable == null )
            {
                throwIndicator.sprite = noThrowable;
                throwIndicator.color = noThrowableColor;
            }
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
                Debug.LogError($"The component of type {typeof(T).Name} couldn't be found for the " + gameObject.name + "!");
            }

            return component;
        }

        private void TrySubscribing()
        {
            if ( abilityThrow == null )
            {
                return;
            }

            abilityThrow.AbilityChanged += UpdateCurrentThrowable;
        }

        private void Unsubscribe()
        {
            abilityThrow.AbilityChanged -= UpdateCurrentThrowable;
        }

        private void UpdateImage(Sprite newSprite)
        {
            Debug.Log("Trying to update throwable display image!");

            throwIndicator.sprite = newSprite;
            throwIndicator.color = Color.white;
        }

        public void UpdateCurrentThrowable()
        {
            currentThrowable = abilityThrow.CurrentAbility;

            UpdateImage(currentThrowable.UIDisplay);
        }
    }
}
