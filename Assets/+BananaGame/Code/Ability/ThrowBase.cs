using UnityEngine;
using BananaSoup.Managers;
using BananaSoup.Utilities;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

namespace BananaSoup.Ability
{
    public class ThrowBase : MonoBehaviour
    {
        [SerializeField] ParticleProjectile abilityParticles;
        [SerializeField] private int poolSize = 3;
        [SerializeField] private Image uIDisplay;
        private Transform spawnPoint;
        private ComponentPool<ParticleProjectile> projectiles;
        private Coroutine activeParticleCoroutine = null;
        private PlayerStateManager psm = null;
        private PlayerStateManager.PlayerState abilityState;
        private Coroutine throwRoutine;
        private bool isStartingToThrow;
        protected AbilityThrow abilityThrow;

        [Tooltip("Time in seconds when to enable the throwable projectile. " +
            "If 0, projectile will spawn immediatelly when throw animation starts.")]
        public float timingWhenEnablingProjectile = 0.0f;

        [Tooltip("Time in seconds how long the player's throwing state is active from the particle " +
                   "projectile spawning (aka timingWhenEnablingProjectile). If 0, player state is reseted " +
                "at the same time when animation starts. Can't be smaller than timingWhenEnablingProjectile!")]
        public float timeAfterSetThrowDone = 2.0f;

        public PlayerStateManager.PlayerState SetAbilityStateName
        {
            get { return abilityState; }
            set { abilityState = value; }
        }

        public Image UIDisplay
        {
            get => uIDisplay;
        }

        private void Awake()
        {
            projectiles = new ComponentPool<ParticleProjectile>(abilityParticles, poolSize);
        }

        public virtual void Start()
        {
            Setup();
        }

        private void OnDisable()
        {
            TryStopAndNullRoutine();
        }

        private void Setup()
        {
            // set the particle system to inactive at start
            abilityParticles.gameObject.SetActive(false);

            psm = PlayerStateManager.Instance;
            if ( psm == null )
            {
                Debug.LogError(gameObject.name + " couldn't find an instance of PlayerStateManager!");
            }

            abilityThrow = GetComponent<AbilityThrow>();
            if ( abilityThrow == null )
            {
                Debug.LogError(name + " is missing a reference to a AbilityThrow!");
            }
        }

        public void OnStartingToThrow(Transform parent)
        {
            // Don't throw ability if old currently playing.
            if ( activeParticleCoroutine != null )
            {
                return;
            }

            if ( !projectiles.DoesPoolHaveInactiveItem() )
            {
                return;
            }

            spawnPoint = parent;

            psm.SetPlayerState(abilityState);
            PlayerBase.Instance.IsMovable = false;
            PlayerBase.Instance.IsTurnable = false;
            PlayerBase.Instance.IsInteractingEnabled = false;
            PlayerBase.Instance.AreAbilitiesEnabled = false;
            PlayerBase.Instance.CanDash = false;

            isStartingToThrow = true;
            throwRoutine = StartCoroutine(ThrowRoutine(timingWhenEnablingProjectile));
        }

        private IEnumerator ThrowRoutine(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            if ( isStartingToThrow )
            {
                OnThrow();
            }
            else
            {
                OnThrowDone();
            }

            throwRoutine = null;
        }

        private void OnThrow()
        {
            ParticleProjectile projectile = projectiles.Get();
            if ( projectile != null )
            {
                projectile.Expired += OnExpired;

                // Set Particle Effect position.
                projectile.transform.position = spawnPoint.position;

                // Set Particle Effect rotation.
                var playerRotation = transform.eulerAngles;
                var particleRotationX = abilityParticles.gameObject.transform.eulerAngles.x;
                projectile.transform.rotation = Quaternion.Euler(particleRotationX, playerRotation.y, playerRotation.z);

                projectile.gameObject.SetActive(true);
                projectile.Setup();

                isStartingToThrow = false;
                throwRoutine = StartCoroutine(ThrowRoutine(timeAfterSetThrowDone));
            }
        }

        private void OnThrowDone()
        {
            PlayerBase.Instance.IsMovable = true;
            PlayerBase.Instance.IsTurnable = true;
            PlayerBase.Instance.IsInteractingEnabled = true;
            PlayerBase.Instance.AreAbilitiesEnabled = true;
            PlayerBase.Instance.CanDash = true;
            psm.ResetPlayerState();
        }

        private void OnExpired(ParticleProjectile projectile)
        {
            projectile.Expired -= OnExpired;

            if ( !projectiles.Recycle(projectile) )
            {
                Debug.LogError("Couldn't recycle the projectile back to the pool!");
            }
        }

        private void TryStopAndNullRoutine()
        {
            if ( throwRoutine != null )
            {
                StopCoroutine(throwRoutine);
                throwRoutine = null;
            }
        }
    }
}
