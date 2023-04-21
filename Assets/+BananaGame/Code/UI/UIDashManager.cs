using BananaSoup.Ability;
using BananaSoup.PickupSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BananaSoup.UI
{
    public class UIDashManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The parent gameObject (Dash_Display) with the" +
            "Image component.")]
        private GameObject dashDisplay;
        [SerializeField, Tooltip("The TMP text (Dash_CooldownTimer) to display" +
            "the remaining cooldown´time.")]
        private TMP_Text cooldownTimerText;

        [Space]

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
            AbilityDash.DashEventAction += ToggleCooldownDisplayAction;
        }

        private void OnDisable()
        {
            PickupDash.OnEventLooted -= ShowDisplay;
            AbilityDash.DashEventAction -= ToggleCooldownDisplayAction;
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

        private void Update()
        {
            if ( isDashCooldownActive )
            {
                // Local variable to store RoundedRemainingCooldown.
                float remainingCooldown = abilityDash.RoundedRemainingCooldown;

                if ( remainingCooldown > 0 )
                {
                    // Calculation where the fillAmount is clamped between 0 and 1
                    // It is 1 - the values to have the value go from 0 to 1.
                    float fillAmount = Mathf.Clamp01(1 - remainingCooldown / abilityDash.DashCooldown);
                    dashImage.fillAmount = fillAmount;

                    // Lerp the color of the image.
                    dashImage.color = Color.Lerp(onCooldown, offCooldown, fillAmount);

                    // Set the cooldownTimerText.
                    cooldownTimerText.text = remainingCooldown.ToString("0.00s");
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

        /// <summary>
        /// Method used to toggle the isDashCooldownActive bool to allow the corresponding
        /// actions for the Dash display happen in Update(). Also toggle cooldownTimerText's
        /// visibility according to the previously mentioned bool value.
        /// </summary>
        public void ToggleCooldownDisplayAction()
        {
            isDashCooldownActive = !isDashCooldownActive;
            cooldownTimerText.gameObject.SetActive(isDashCooldownActive);

            // Make sure the fillAmount is 1, color is offCooldown color
            // and that the cooldownTimerText is empty.
            // Only allow this to happen when the cooldown is inactive.
            if ( !isDashCooldownActive )
            {
                Debug.Log("Resetting Image and TMP_Text values!");
                dashImage.fillAmount = 1f;
                dashImage.color = offCooldown;
                cooldownTimerText.text = string.Empty;
            }
        }

        /// <summary>
        /// Method used to set dashDisplay active and at the same time make sure that the
        /// cooldownTimerText is empty and not shown because it shouldn't be shown.
        /// </summary>
        public void ShowDisplay()
        {
            dashDisplay.SetActive(true);
            cooldownTimerText.gameObject.SetActive(false);
            cooldownTimerText.text = string.Empty;
        }
    }
}
