using BananaSoup.HealthSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BananaSoup.UI
{
    public class UIHealthManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The panel used as the parent of the health" +
            "icons.")]
        private GameObject healthDisplay;

        [Space]

        [SerializeField, Tooltip("The prefab which is used to display the " +
            "HP image(s).")]
        private GameObject healthIcon;
        [SerializeField, Tooltip("Full HP Sprite")]
        private Sprite fullHPImage;
        [SerializeField, Tooltip("Empty HP Sprite")]
        private Sprite emptyHPImage;

        private float lateStartDelay = 0.01f;

        private GameObject[] healthIcons;

        private int currentHealth = 0;

        private Coroutine lateStartRoutine = null;

        // References
        private PlayerBase playerBase;
        private PlayerHealth playerHealth;

        private void OnEnable()
        {
            PlayerHealth.PlayerHealthChanged += UpdateHealthDisplay;
        }

        private void OnDisable()
        {
            PlayerHealth.PlayerHealthChanged -= UpdateHealthDisplay;
        }

        private void Start()
        {
            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError(gameObject.name + " couldn't access an instance of PlayerBase!");
            }

            playerHealth = GetDependency<PlayerHealth>(playerBase);

            if ( currentHealth == 0 && lateStartRoutine == null )
            {
                lateStartRoutine = StartCoroutine(nameof(LateStart));
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

        private void InitializeArray()
        {
            currentHealth = playerHealth.CurrentHealth;

            healthIcons = new GameObject[playerHealth.MaxHealth];

            for ( int i = 0; i < healthIcons.Length; i++ )
            {
                healthIcons[i] = Instantiate(healthIcon, healthDisplay.transform);
            }

            for ( int j = 0; j < currentHealth; j++ )
            {
                if ( healthIcons[j].GetComponent<Image>() == null )
                {
                    Debug.LogWarning(healthIcons[j].name + " has no Image component!");
                }

                healthIcons[j].GetComponent<Image>().sprite = fullHPImage;
            }
        }

        private IEnumerator LateStart()
        {
            yield return new WaitForSeconds(lateStartDelay);
            InitializeArray();
            lateStartRoutine = null;
        }

        public void UpdateHealthDisplay()
        {
            var previousHealth = currentHealth;
            currentHealth = playerHealth.CurrentHealth;

            if ( currentHealth < previousHealth )
            {
                healthIcons[previousHealth - 1].GetComponent<Image>().sprite = emptyHPImage;
            }
            else if ( currentHealth > previousHealth )
            {
                healthIcons[currentHealth - 1].GetComponent<Image>().sprite = fullHPImage;
            }
        }
    }
}
