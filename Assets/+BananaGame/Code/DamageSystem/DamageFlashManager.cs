using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class DamageFlashManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The color the object is to change to when taking damage.")]
        private Color flashColor = Color.red;
        [SerializeField, Tooltip("The time it takes to complete the damage flash effetc.")]
        private float flashTime = 0.25f;

        private Coroutine damageFlashRoutine = null;

        private MeshRenderer meshRenderer;
        private Material[] materials;

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            if ( damageFlashRoutine != null )
            {
                StopCoroutine(damageFlashRoutine);
                damageFlashRoutine = null;
            }
        }

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if ( meshRenderer == null )
            {
                Debug.LogError(gameObject.name + " doesn't have a MeshRenderer component!");
            }

            Initialize();
        }

        // Start is called before the first frame update
        private void Start()
        {

        }

        private void Initialize()
        {
            materials = new Material[meshRenderer.materials.Length];

            for ( int i = 0; i < materials.Length; i++ )
            {
                materials[i] = meshRenderer.materials[i];
            }
        }

        // Update is called once per frame
        private void Update()
        {

        }

        private IEnumerator DamageFlash()
        {
            // Set flash color
            SetFlashColor();

            // lerp the flash amount
            float currentFlashAmount = 0.0f;
            float elapsedTime = 0f;

            while ( elapsedTime < flashTime )
            {
                // calculate elapsedTime
                elapsedTime += Time.deltaTime;

                currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / flashTime));
                SetFlashAmount(currentFlashAmount);

                yield return null;
            }
        }

        private void SetFlashColor()
        {
            for ( int i = 0; i < materials.Length; i++ )
            {
                materials[i].SetColor("_FlashColor", flashColor);
            }
        }

        private void SetFlashAmount(float amount)
        {
            for ( int i = 0; i < materials.Length; i++ )
            {
                materials[i].SetFloat("_FlashStrength", amount);
            }
        }
    }
}
