using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class AbilityBlindingSand : MonoBehaviour
    {
        [SerializeField] ParticleSystem sandParticles;
        [Tooltip("Transform where the sand particle effect spawns.")]
        [SerializeField] private Transform handTransform;
        [Tooltip("A delay to start Sand Particle Effect later to match it with the animation.")]
        [SerializeField] private const string isSanding = "sandThrow";

        private float duration;
        private Coroutine activeParticleCoroutine;
        private Animator animator;

        [Header("UnityActions to manage PlayerStates")]
        public UnityAction onSanding;
        public UnityAction onSandingFinished;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            // set the particle system to inactive at start
            sandParticles.gameObject.SetActive(false);

            // Set Coroutine duration to Particle Effect contant lenght
            duration = sandParticles.main.startLifetime.constant;

            animator = GetComponent<Animator>();
            if ( animator == null )
            {
                Debug.LogError(name + "'s Animator is null and it shouldn't be!");
            }
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
                onSanding.Invoke();
                animator.SetTrigger(isSanding);
            }
        }

        // ThrowSand is called from FennecCharacter@Throw animation by an event.
        private void ThrowSand()
        {
            // TODO: Allow throw only once
            // TODO: Disable movement when sanding from StateManager

            // Set Particle Effect transform.
            sandParticles.gameObject.transform.parent = null;
            sandParticles.gameObject.transform.position = handTransform.position;

            // TODO: Fix rotation
            //Vector3 rotation = new Vector3(sandParticles.gameObject.transform.rotation.x, sandParticles.gameObject.transform.rotation.y);
            //Debug.Log("Rotation: " + rotation);
            //sandParticles.gameObject.transform.eulerAngles = rotation;
            Vector3 rotation = new Vector3(sandParticles.gameObject.transform.rotation.x, PlayerBase.Instance.transform.rotation.y);
            //Debug.Log("Rotation: " + rotation);
            sandParticles.gameObject.transform.rotation.Set(rotation.x, PlayerBase.Instance.transform.rotation.y, PlayerBase.Instance.transform.rotation.z, PlayerBase.Instance.transform.rotation.w);
            //Debug.Log("Particle rotation: " + sandParticles.gameObject.transform.localRotation);

            //sandParticles.gameObject.transform.rotation = transform.rotation;

            sandParticles.gameObject.SetActive(true);

            onSandingFinished.Invoke();
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
