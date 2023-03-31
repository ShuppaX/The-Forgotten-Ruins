using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class AbilityThrowBase : MonoBehaviour
    {
        [SerializeField] ParticleSystem abilityParticles;

        // The transform where the ability's particle effect spawns.
        private Transform handTransform;

        // A delay to start Ability's Particle Effect later to match it with the animation.
        private float duration;
        private Coroutine activeParticleCoroutine = null;
        private Animator animator = null;
        private PlayerStateManager psm = null;
        private PlayerStateManager.PlayerState abilityState;

        public PlayerStateManager.PlayerState SetAbilityStateName
        {
            get { return abilityState; }
            set { abilityState = value; }
        }

        public virtual void Start()
        {
            Setup();
        }

        private void Setup()
        {
            // set the particle system to inactive at start
            abilityParticles.gameObject.SetActive(false);

            // Set Coroutine duration to Particle Effect contant lenght
            duration = abilityParticles.main.startLifetime.constant;

            psm = PlayerStateManager.Instance;
            if ( psm == null )
            {
                Debug.LogError(gameObject.name + " couldn't find an instance of PlayerStateManager!");
            }

            animator = GetComponent<Animator>();
            if ( animator == null )
            {
                Debug.LogError(name + "'s Animator is null and it shouldn't be!");
            }
        }

        public void OnStartingToThrow(Transform parent)
        {
            // Don't cast a sand if old currently playing.
            if ( activeParticleCoroutine != null )
            {
                return;
            }

            handTransform = parent;

            psm.SetPlayerState(abilityState);
            PlayerBase.Instance.IsMovable = false;
            PlayerBase.Instance.IsTurnable = false;
            PlayerBase.Instance.IsInteractingEnabled = false;
            PlayerBase.Instance.AreAbilitiesEnabled = false;
            PlayerBase.Instance.CanDash = false;
        }

        private IEnumerator DeactivateParticles()
        {
            abilityParticles.Play();
            // wait for duration of the particle effect
            yield return new WaitForSeconds(duration);

            // deactivate the particle system
            abilityParticles.Stop();
            abilityParticles.gameObject.SetActive(false);
            activeParticleCoroutine = null;
        }

        public void OnThrow()
        {
            // Set Particle Effect position.
            abilityParticles.gameObject.transform.position = handTransform.position;

            // Set Particle Effect rotation.
            var playerRotation = transform.eulerAngles;
            var particleRotationX = abilityParticles.gameObject.transform.eulerAngles.x;
            abilityParticles.gameObject.transform.rotation = Quaternion.Euler(particleRotationX, playerRotation.y, playerRotation.z);

            abilityParticles.gameObject.SetActive(true);

            activeParticleCoroutine = StartCoroutine(DeactivateParticles());
        }

        public void OnThrowDone()
        {
            PlayerBase.Instance.IsMovable = true;
            PlayerBase.Instance.IsTurnable = true;
            PlayerBase.Instance.IsInteractingEnabled = true;
            PlayerBase.Instance.AreAbilitiesEnabled = true;
            PlayerBase.Instance.CanDash = true;
            psm.ResetPlayerState();
        }
    }
}
