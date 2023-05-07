using System.Collections;
using UnityEngine;

namespace BananaSoup.DamageSystem
{
    public class DamageFlash : MonoBehaviour
    {
        [SerializeField, Tooltip("The GameObject which has the SkinnedMeshRenderer which you want to manipulate.")]
        private GameObject targetObject = null;

        [Space]

        [ColorUsage(true, true)]
        [SerializeField, Tooltip("The color the object is to changed to when taking damage.")]
        private Color flashColor = Color.red;
        [SerializeField, Tooltip("The time it takes to complete the damage flash effect.")]
        private float flashTime = 0.25f;

        private Coroutine damageFlashRoutine = null;

        // References
        private SkinnedMeshRenderer skinnedMeshRenderer;
        private Material[] materials;

        private void OnDisable()
        {
            // StopCoroutine and null it if it isn't null on disable.
            if ( damageFlashRoutine != null )
            {
                StopCoroutine(damageFlashRoutine);
                damageFlashRoutine = null;
            }
        }

        private void Awake()
        {
            if ( targetObject == null )
            {
                Debug.LogError("The targetObject is not set for the " + gameObject.name + "!");
            }

            skinnedMeshRenderer = targetObject.GetComponent<SkinnedMeshRenderer>();
            if ( skinnedMeshRenderer == null )
            {
                Debug.LogError(targetObject.gameObject.name + " doesn't have a MeshRenderer component!");
            }

            Initialize();
        }

        /// <summary>
        /// Assign materials from meshRenderer to materials array.
        /// </summary>
        private void Initialize()
        {
            materials = new Material[skinnedMeshRenderer.materials.Length];

            for ( int i = 0; i < materials.Length; i++ )
            {
                materials[i] = skinnedMeshRenderer.materials[i];
            }
        }

        /// <summary>
        /// Method used to call the damage flash.
        /// </summary>
        public void CallDamageFlash()
        {
            if ( damageFlashRoutine == null )
            {
                damageFlashRoutine = StartCoroutine(nameof(DamageFlasher));
            }
        }

        /// <summary>
        /// Coroutine in which the flash color is set to the serialized flash color,
        /// then a local float is initialized, which is used to track the elapsed time.
        /// Call SetFlashAmount with currentFlashAmount which is calculated with Mathf.Lerp
        /// and then null the Coroutine.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DamageFlasher()
        {
            // Set flash color
            SetFlashColor();

            // lerp the flash amount
            float elapsedTime = 0f;

            while ( elapsedTime < flashTime )
            {
                // calculate elapsedTime
                elapsedTime += Time.deltaTime;

                float currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / flashTime));
                float emissionIntensity = Mathf.Lerp(0.5f, 0f, (elapsedTime / flashTime));
                SetFlashAmount(currentFlashAmount);
                SetEmissionIntensity(emissionIntensity);

                yield return null;
            }

            damageFlashRoutine = null;
        }

        /// <summary>
        /// Set all of the materials in materials color to flashColor.
        /// </summary>
        private void SetFlashColor()
        {
            for ( int i = 0; i < materials.Length; i++ )
            {
                materials[i].SetColor("_FlashColor", flashColor);
            }
        }

        /// <summary>
        /// Set all of the materials in materials _FlashStrength to parameter amount.
        /// </summary>
        /// <param name="amount">Floating point between 0 and 1 to set the strength of the flash.</param>
        private void SetFlashAmount(float amount)
        {
            for ( int i = 0; i < materials.Length; i++ )
            {
                materials[i].SetFloat("_FlashStrength", amount);
            }
        }

        /// <summary>
        /// Set all of the materials in materials _EmissionIntensity to parameter amount.
        /// </summary>
        /// <param name="amount">Floating point between 0 and 1 to set the intensity of emission.</param>
        private void SetEmissionIntensity(float amount)
        {
            for ( int i = 0; i < materials.Length; i++ )
            {
                materials[i].SetFloat("_EmissionIntensity", amount);
            }
        }
    }
}
