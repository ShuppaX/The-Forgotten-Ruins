using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class AbilityBlindingSand : MonoBehaviour
    {
        [SerializeField] ParticleSystem sandParticles;
        [Tooltip("Transform where the sand particle effect spawns.")]
        [SerializeField] private Transform handTransform;
        [Tooltip("A delay to start Sand Particle Effect later to match it with the animation.")]
        [SerializeField] private const string isSanding = "isSanding";

        private float duration;
        private Coroutine activeParticleCoroutine;
        private Animator animator;

        private void Start()
        {
            // set the particle system to inactive at start
            sandParticles.gameObject.SetActive(false);

            // Set Coroutine duration to Particle Effect contant lenght
            duration = sandParticles.main.startLifetime.constant;

            animator = GetComponent<Animator>();
        }

        public void OnAbility(InputAction.CallbackContext context)
        {
            // Don't cast a sand if old currently playing.
            if ( activeParticleCoroutine != null )
            {
                return;
            }

            if ( context.performed )
            {
                animator.SetTrigger(isSanding);
            }
        }

        // ThrowSand is called from FennecCharacter@Throw animation by an event.
        private void ThrowSand()
        {
            // TODO: Disable movement when sanding from StateManager

            // Set Particle Effect transform.
            sandParticles.gameObject.transform.parent = null;
            sandParticles.gameObject.transform.position = handTransform.position;

            // TODO: Fix rotation
            //Vector3 rotation = new Vector3(sandParticles.gameObject.transform.x, sandParticles.gameObject.transform.rotation.y);
            //Debug.Log("Rotation: " + rotation);
            //sandParticles.gameObject.transform.eulerAngles = rotation;
            //Debug.Log("Particle rotation: " + sandParticles.gameObject.transform.localRotation);
            sandParticles.gameObject.transform.rotation = transform.rotation;

            sandParticles.gameObject.SetActive(true);

            activeParticleCoroutine = StartCoroutine(DeactivateParticles());
        }

        private IEnumerator DeactivateParticles()
        {
            sandParticles.Play();
            // wait for duration of the particle effect
            yield return new WaitForSeconds(duration);

            // deactivate the particle system
            sandParticles.Stop();
            sandParticles.gameObject.SetActive(false);
            activeParticleCoroutine = null;
        }
    }
}
