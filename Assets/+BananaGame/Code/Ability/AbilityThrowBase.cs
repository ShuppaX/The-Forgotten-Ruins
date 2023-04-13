using UnityEngine;
using BananaSoup.Managers;
using BananaSoup.Utilities;

namespace BananaSoup.Ability
{
    public class AbilityThrowBase : MonoBehaviour
    {
        [SerializeField] ParticleProjectile abilityParticles;
        [SerializeField] private int poolSize = 3;

        private Transform spawnPoint;
        private ComponentPool<ParticleProjectile> projectiles;
        private Coroutine activeParticleCoroutine = null;
        private PlayerStateManager psm = null;
        private PlayerStateManager.PlayerState abilityState;

        public PlayerStateManager.PlayerState SetAbilityStateName
        {
            get { return abilityState; }
            set { abilityState = value; }
        }

        private void Awake()
        {
            projectiles = new ComponentPool<ParticleProjectile>(abilityParticles, poolSize);
        }

        public virtual void Start()
        {
            Setup();
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
        }

        public void OnStartingToThrow(Transform parent)
        {
            // Don't throw ability if old currently playing.
            if ( activeParticleCoroutine != null )
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
        }

        public void OnThrow()
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
            }
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

        private void OnExpired(ParticleProjectile projectile)
        {
            projectile.Expired -= OnExpired;

            if ( !projectiles.Recycle(projectile) )
            {
                Debug.LogError("Couldn't recycle the projectile back to the pool!");
            }
        }
    }
}
