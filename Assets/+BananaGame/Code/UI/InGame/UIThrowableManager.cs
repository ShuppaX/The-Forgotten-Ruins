using BananaSoup.Ability;
using UnityEngine;
using UnityEngine.UI;

namespace BananaSoup.UI.InGame
{
    public class UIThrowableManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The gameObject which has the Image component" +
            " (Throwable_Image) to display currentThrowable.")]
        private GameObject throwableImageDisplay;
        [SerializeField, Tooltip("The parent gameObject (Throwable_SelectionArrows)" +
            "under which are the selection arrows for changing the current throwable.")]
        private GameObject throwableSelectionArrows;
        [SerializeField, Tooltip("The gameObject (Throwable_Indicator)" +
            "which has the image for current throwable.")]
        private Image throwableImage;

        // References
        private PlayerBase playerBase = null;
        private AbilityThrow abilityThrow = null;
        private ThrowBase currentThrowable = null;

        private void OnEnable()
        {
            AbilityThrow.ThrowableChanged += UpdateCurrentThrowable;
        }

        private void OnDisable()
        {
            AbilityThrow.ThrowableChanged -= UpdateCurrentThrowable;
        }

        private void Start()
        {
            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError(gameObject.name + " couldn't access an instance of PlayerBase!");
            }

            abilityThrow = GetDependency<AbilityThrow>(playerBase);

            if ( !playerBase.IsThrowableLooted )
            {
                ShowImage(false);
                ShowSelectionArrows(false);
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
            if ( !throwableImageDisplay.activeSelf )
            {
                ShowImage(true);
            }

            if ( abilityThrow.ActiveAbilities > 1
                && !throwableSelectionArrows.activeSelf )
            {
                ShowSelectionArrows(true);
            }

            currentThrowable = abilityThrow.CurrentAbility;

            if ( currentThrowable == null )
            {
                Debug.LogError($"currentThrowable is null and can't be used to update current" +
                    $"throwable for" + gameObject.name + "!");
            }

            UpdateImage(currentThrowable.UIDisplay);
        }

        /// <summary>
        /// Method used to set the throwableDisplayImage active or inactive.
        /// </summary>
        /// <param name="value">True to set active, false to set inactive.</param>
        private void ShowImage(bool value)
        {
            throwableImageDisplay.SetActive(value);
        }

        /// <summary>
        /// Method used to set the throwableSelectionArrows active or inactive.
        /// </summary>
        /// <param name="value">True to set active, false to set inactive.</param>
        private void ShowSelectionArrows(bool value)
        {
            throwableSelectionArrows.SetActive(value);
        }
    }
}
