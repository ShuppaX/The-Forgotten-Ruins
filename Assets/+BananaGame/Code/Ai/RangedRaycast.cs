using System.Collections;
using UnityEngine;
using BananaSoup.Utilities;
using BananaSoup.DamageSystem;
using Unity.VisualScripting;
using UnityEngine.AI;

namespace BananaSoup
{
    public class RangedRaycast : MeleeRaycast
    {
        [Header("Projectile variables")] [SerializeField]
        private EnemyProjectile projectilePrefab;

        [SerializeField] private Transform firingPoint;
        [SerializeField] private float timeBetweenShots = 0.8f;
        [SerializeField] private float projectileSpeed = 15.0f;
        [SerializeField] private float firingDelay = 0.7f;

        [SerializeField] [Tooltip("Size of the projectile pool")]
        private int poolSize = 10;

        private ComponentPool<EnemyProjectile> _projectiles;
        private EnemyProjectile _projectile;
        private Coroutine _cooldownRoutine = null;
        private bool _onCooldown = false;
        private bool _alreadyAttacked;
        private Coroutine _firingStall;


        public override void Awake()
        {
            enemy = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            _projectiles = new ComponentPool<EnemyProjectile>(projectilePrefab, poolSize);
        }


        protected override void Attack()
        {
            if (_stunned) return;
            //Stop enemy movement
            enemy.SetDestination(transform.position);

            if (_alreadyAttacked) return;

            if (_onCooldown) return;
            _projectile = _projectiles.Get();


            ClearTrigger();
            SetTrigger(attack);

            if (_projectile != null)
            {
                Invoke(nameof(Fire), firingDelay);

                if (_cooldownRoutine != null) _cooldownRoutine = StartCoroutine(OnCooldown());
            }

            lastDidSomething = Time.time;
        }

        private void OnExpired(EnemyProjectile projectile)
        {
            projectile.Expired -= OnExpired;

            if (!_projectiles.Recycle(projectile)) Debug.LogError("Couldn't recycle the projectile back to the pool!");
        }

        private IEnumerator OnCooldown()
        {
            _onCooldown = true;
            yield return new WaitForSeconds(timeBetweenShots);
            _onCooldown = false;

            _cooldownRoutine = null;
        }


        protected override void OnDisable()
        {
            base.OnDisable();
            if (_cooldownRoutine != null)
            {
                StopCoroutine(_cooldownRoutine);
                _cooldownRoutine = null;
            }
        }

        private void Fire()
        {
            var projTra = _projectile.transform;

            _projectile.Expired += OnExpired;
            projTra.position = firingPoint.position;
            projTra.rotation = firingPoint.rotation;
            
            _projectile.Setup(projectileSpeed);
        }


        public override void DeathSequence()
        {
            ClearTrigger();
            SetTrigger(animDeath);
        }
    }
}