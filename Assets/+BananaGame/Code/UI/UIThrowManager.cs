using BananaSoup.Ability;
using UnityEngine;
using UnityEngine.UI;

namespace BananaSoup.UI
{
    public class UIThrowManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The parent gameObject (Throwable_Display)" +
            "which has the throwable indicator and arrows.")]
        private GameObject throwableDisplay;
        [SerializeField, Tooltip("The gameObject (Throwable_Indicator)" +
            "which has the image for current throwable.")]
        private Image throwableImage;

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
                throwableDisplay.SetActive(false);
            }
        }

        /// <summary>
        /// Method used to get component dependency/dependencies.
        /// </summary>
        /// <typeparam name="T">The component to get.</typeparam>
        /// <param name="comp">Use a component as parameter if you want to get
        /// a dependency from a component. For example an instance.</param>
        /// <returns>The desired component.</returns>
        private T GetDependency<T>(Component comp = null) where T : Component
        {
            T component;

            if ( comp != null )
            {
                component = comp.GetComponent<T>();
            }
            else
            {
                component = GetComponent<T>();
            }

            if ( component == null )
            {
                Debug.LogError($"The component of type {typeof(T).Name} couldn't" +
                    $"be found for the " + gameObject.name + "!");
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
            throwableImage.sprite = newSprite;
        }

        /// <summary>
        /// Method used to update currentThrowable and then call UpdateImage with
        /// currentThrowables public UIDisplay sprite.
        /// </summary>
        public void UpdateCurrentThrowable()
        {
            if ( !throwableDisplay.activeSelf )
            {
                throwableDisplay.SetActive(true);
            }

            currentThrowable = abilityThrow.CurrentAbility;

            if ( currentThrowable == null )
            {
                Debug.LogError($"currentThrowable is null and can't be used to update current" +
                    $"throwable for" + gameObject.name + "!");
            }

            UpdateImage(currentThrowable.UIDisplay);
        }
    }
}
