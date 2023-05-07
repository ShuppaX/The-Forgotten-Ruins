using System.Collections;
using UnityEngine;

namespace BananaSoup.Puzzle
{
    public class PressurePlateFlash : MonoBehaviour
    {
        [ColorUsage(true, true)]
        [SerializeField, Tooltip("The color the object is to changed to while activated.")]
        private Color flashColor = Color.red;
        [SerializeField, Tooltip("The time it takes to complete the revert the flash effect.")]
        private float flashTime = 1.0f;

        private float minFlash = 0f;
        private float maxFlash = 1f;
        private float minEmission = 12f;
        private float maxEmission = 20.5f;
        private float currentFlash = 0f;
        private float currentEmission = 0f;

        private Material material;
        private int materialIndexInRenderer = 1;

        private Coroutine flashRoutine = null;

        // References
        private MeshRenderer meshRenderer;

        private void OnDisable()
        {
            // StopCoroutine and null it if it isn't null on disable.
            if ( flashRoutine != null )
            {
                StopCoroutine(flashRoutine);
                flashRoutine = null;
            }
        }

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if ( meshRenderer == null )
            {
                Debug.LogError(name + " doesn't have a MeshRenderer component!");
            }

            material = meshRenderer.materials[materialIndexInRenderer];

            currentFlash = minFlash;
            currentEmission = minEmission;
        }

        /// <summary>
        /// Method used to call the flashing effect.
        /// </summary>
        public void CallFlash()
        {
            if ( flashRoutine != null )
            {
                StopCoroutine(flashRoutine);
                flashRoutine = null;
            }

            if ( flashRoutine == null )
            {
                flashRoutine = StartCoroutine(nameof(Flasher));
            }
        }

        /// <summary>
        /// Method used to reset color and emission.
        /// </summary>
        public void ResetColorAndEmission()
        {
            if ( flashRoutine != null )
            {
                StopCoroutine(flashRoutine);
                flashRoutine = null;
            }

            if ( flashRoutine == null )
            {
                flashRoutine = StartCoroutine(nameof(RevertFlash));
            }
        }

        /// <summary>
        /// Coroutine in which the flash color is set to the serialized flash color,
        /// then a local float is initialized, which is used to track the elapsed time.
        /// Call SetFlashAmount with currentFlashAmount which is calculated with Mathf.Lerp
        /// and then null the Coroutine.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Flasher()
        {
            // Set flash color
            SetFlashColor();

            float elapsedTime = 0f;

            while ( elapsedTime < flashTime)
            {
                elapsedTime += Time.deltaTime;

                float currentFlashAmount = Mathf.Lerp(currentFlash, maxFlash, (elapsedTime / flashTime));
                float emissionIntensity = Mathf.Lerp(currentEmission, maxEmission, (elapsedTime / flashTime));

                currentFlash = currentFlashAmount;
                currentEmission = emissionIntensity;

                SetFlashAmount(currentFlashAmount);
                SetEmissionIntensity(emissionIntensity);

                yield return null;
            }

            flashRoutine = null;
        }

        private IEnumerator RevertFlash()
        {
            float elapsedTime = 0f;

            while ( elapsedTime < flashTime )
            {
                elapsedTime += Time.deltaTime;

                float currentFlashAmount = Mathf.Lerp(currentFlash, minFlash, (elapsedTime / flashTime));
                float emissionIntensity = Mathf.Lerp(currentEmission, minEmission, (elapsedTime / flashTime));

                currentFlash = currentFlashAmount;
                currentEmission = emissionIntensity;

                SetFlashAmount(currentFlashAmount);
                SetEmissionIntensity(emissionIntensity);

                yield return null;
            }

            flashRoutine = null;
        }

        /// <summary>
        /// Set all of the materials in materials color to flashColor.
        /// </summary>
        private void SetFlashColor()
        {
            material.SetColor("_FlashColor", flashColor);
        }

        /// <summary>
        /// Set all of the materials in materials _FlashStrength to parameter amount.
        /// </summary>
        /// <param name="amount">Floating point between 0 and 1 to set the strength of the flash.</param>
        private void SetFlashAmount(float amount)
        {
            material.SetFloat("_FlashStrength", amount);
        }

        /// <summary>
        /// Set all of the materials in materials _EmissionIntensity to parameter amount.
        /// </summary>
        /// <param name="amount">Float value, check desired value with material.</param>
        private void SetEmissionIntensity(float amount)
        {
            material.SetFloat("_EmissionIntensity", amount);
        }
    }
}
