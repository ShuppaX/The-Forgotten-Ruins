using BananaSoup.Ability;
using UnityEngine;
using UnityEngine.UI;

namespace BananaSoup.UI
{
    public class UIThrowManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The UI element, which displays the current throwable sprite.")]
        private Image throwIndicator;
        [SerializeField, Tooltip("The sprite when there is no throwable available/picked up.")]
        private Sprite noThrowable;

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
            }
        }

        /// <summary>
        /// Method used to get component dependency/dependencies.
        /// </summary>
        /// <typeparam name="T">The component to get.</typeparam>
        /// <param name="playerBase">Use the playerBase as a parameter here, if the
        /// component to get is from PlayerBase's instance.</param>
        /// <returns>The desired component.</returns>
        private T GetDependency<T>(PlayerBase playerBase = null) where T : Component
        {
            T component;

            if ( playerBase != null )
            {
                component = playerBase.GetComponent<T>();
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

        /// <summary>
        /// Method used to subscribe to abilityThrows AbilityChanged.
        /// </summary>
        private void TrySubscribing()
        {
            if ( abilityThrow == null )
            {
                return;
            }

            abilityThrow.AbilityChanged += UpdateCurrentThrowable;
        }

        /// <summary>
        /// Method used to unsubscribe abilityThrows AbilityChanged.
        /// </summary>
        private void Unsubscribe()
        {
            abilityThrow.AbilityChanged -= UpdateCurrentThrowable;
        }

        /// <summary>
        /// Method used to update the sprite of the throwIndicator.
        /// </summary>
        /// <param name="newSprite">The new sprite to be used.</param>
        private void UpdateImage(Sprite newSprite)
        {
            throwIndicator.sprite = newSprite;
        }

        /// <summary>
        /// Method used to update currentThrowable and then call UpdateImage with
        /// currentThrowables public UIDisplay sprite.
        /// </summary>
        public void UpdateCurrentThrowable()
        {
            if ( currentThrowable == null )
            {
                Debug.LogError($"currentThrowable is null and can't be used to update current" +
                    $"throwable for" + gameObject.name + "!");
            }

            currentThrowable = abilityThrow.CurrentAbility;

            UpdateImage(currentThrowable.UIDisplay);
        }
    }
}
