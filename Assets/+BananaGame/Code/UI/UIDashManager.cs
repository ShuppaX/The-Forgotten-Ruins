using BananaSoup.Ability;
using BananaSoup.PickupSystem;
using UnityEngine;
using UnityEngine.UI;

namespace BananaSoup.UI
{
    public class UIDashManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The parent gameObject (Dash_Display) with the" +
            "Image component.")]
        private GameObject dashDisplay;
        [SerializeField, Tooltip("The colour of the image while dash is off cooldown.")]
        private Color offCooldown;
        [SerializeField, Tooltip("The colour of the image while dash is on cooldown.")]
        private Color onCooldown;

        private bool isDashCooldownActive = false;

        // References
        private Image dashImage;
        private PlayerBase playerBase;
        private AbilityDash abilityDash;

        private void OnEnable()
        {
            PickupDash.OnEventLooted += ShowDisplay;
            AbilityDash.DashEventAction += ToggleCooldownBool;
        }

        private void OnDisable()
        {
            PickupDash.OnEventLooted -= ShowDisplay;
            AbilityDash.DashEventAction -= ToggleCooldownBool;
        }

        private void Start()
        {
            dashImage = dashDisplay.GetComponent<Image>();
            if ( dashImage == null )
            {
                Debug.LogError($"No component of {typeof(Component).Name} couldn't" +
                    $"be found on the dashDisplay for " + gameObject.name + "!");
            }

            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError(gameObject.name + " couldn't find an instance of" +
                    "PlayerBase!");
            }

            abilityDash = GetDependency<AbilityDash>(playerBase);

            if ( !playerBase.IsDashLooted )
            {
                dashDisplay.SetActive(false);
            }
        }

        // TODO: Add a numeral text display for cooldown as well.
        private void Update()
        {
            if ( isDashCooldownActive )
            {
                if ( abilityDash.RoundedRemainingCooldown > 0 )
                {
                    float fillAmount = Mathf.Clamp01(1 - abilityDash.RoundedRemainingCooldown / abilityDash.DashCooldown);
                    dashImage.fillAmount = fillAmount;

                    dashImage.color = Color.Lerp(onCooldown, offCooldown, fillAmount);
                }

                if ( abilityDash.RoundedRemainingCooldown <= 0 )
                {
                    dashImage.fillAmount = 1f;
                    dashImage.color = offCooldown;
                }
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

        public void ToggleCooldownBool()
        {
            isDashCooldownActive = !isDashCooldownActive;
        }

        public void ShowDisplay()
        {
            dashDisplay.SetActive(true);
        }
    }
}
