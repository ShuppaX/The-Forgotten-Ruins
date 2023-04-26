using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class DamageFlashManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The color the object is to change to when taking damage.")]
        private Color damageColor = Color.red;
        [SerializeField, Tooltip("The time it takes to complete the damage flash effetc.")]
        private float flashTime = 0.25f;

        [Space]

        [SerializeField, Tooltip("The amount of materials on the Mesh Renderer.")]
        private int materialCount = 0;

        private Coroutine damageFlashRoutine = null;

        private MeshRenderer meshRenderer;
        private Material[] materials;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            if (damageFlashRoutine != null)
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
            materials = new Material[materialCount];

            for (int i = 0; i < materialCount; i++)
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

        }
    }
}
