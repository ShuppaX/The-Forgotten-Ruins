using System.Collections;
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
        [SerializeField] private const string isSanding = "sandThrow";

        private float duration;

        private Coroutine activeParticleCoroutine = null;
        private Animator animator = null;
        private PlayerStateManager psm = null;
        private DebugManager debug = null;

        [Header("Constant strings used for PlayerState handling")]
        public const string sanding = "Sanding";
        public const string sandingOver = "Idle";

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

            psm = PlayerStateManager.Instance;
            debug = DebugManager.Instance;

            animator = GetComponent<Animator>();
            if ( animator == null )
            {
                Debug.LogError(name + "'s Animator is null and it shouldn't be!");
            }
        }

        public void OnAbility(InputAction.CallbackContext context)
        {
            if ( !PlayerBase.Instance.AreAbilitiesEnabled )
            {
                return;
            }

            // Don't cast a sand if old currently playing.
            if ( activeParticleCoroutine != null )
            {
                return;
            }

            if ( context.performed )
            {
                psm.SetPlayerState(sanding);
                debug.UpdatePlayerStateText();
                PlayerBase.Instance.IsMovable = false;
                PlayerBase.Instance.IsTurnable = false;
                PlayerBase.Instance.IsInteractingEnabled = false;
                PlayerBase.Instance.AreAbilitiesEnabled = false;
                animator.SetTrigger(isSanding);
            }
        }

        // ThrowSand() is called from Fennec@SandThrow animation by an event.
        private void ThrowSand()
        {
            // TODO: Allow throw only once
            // TODO: Disable movement when sanding from StateManager

            // Set Particle Effect parent to null, so the player's movements won't affect it.
            sandParticles.gameObject.transform.parent = null;

            // Set Particle Effect position.
            sandParticles.gameObject.transform.position = handTransform.position;

            // Set Particle Effect rotation.
            var playerRotation = transform.eulerAngles;
            var sandRotationX = sandParticles.gameObject.transform.eulerAngles.x;
            sandParticles.gameObject.transform.rotation = Quaternion.Euler(sandRotationX, playerRotation.y, playerRotation.z);

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

        // SetSandingDone() is called from Fennec@SandThrow animation by an event.
        private void SetSandingDone()
        {
            PlayerBase.Instance.IsMovable = true;
            PlayerBase.Instance.IsTurnable = true;
            PlayerBase.Instance.IsInteractingEnabled = true;
            PlayerBase.Instance.AreAbilitiesEnabled = true;
            psm.SetPlayerState(sandingOver);
            debug.UpdatePlayerStateText();
        }
    }
}
