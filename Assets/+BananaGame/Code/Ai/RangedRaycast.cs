using System.Collections;
using UnityEngine;
using BananaSoup.Utilities;

namespace BananaSoup
{
    public class RangedRaycast : MeleeRaycast
    {
        [SerializeField]
        private EnemyProjectile projectilePrefab;
        [SerializeField]
        private float timeBetweenShots = 1.5f;
        [SerializeField]
        private float projectileSpeed = 15.0f;
        [SerializeField]
        private Transform firingPoint;
        [SerializeField]
        private int poolSize = 5;

        private bool onCooldown = false;

        private ComponentPool<EnemyProjectile> projectiles;
        private Coroutine cooldownRoutine = null;


        public override void Awake()
        {
            base.Awake();

            projectiles = new ComponentPool<EnemyProjectile>(projectilePrefab, poolSize);
        }

        private void OnDisable()
        {
            if ( cooldownRoutine != null )
            {
                StopCoroutine(cooldownRoutine);
                cooldownRoutine = null;
            }
        }

        public override void Attack()
        {
            //Stop enemy movement
            enemy.SetDestination(transform.position);

            if ( alreadyAttacked )
            {
                return;
            }

            if ( onCooldown )
            {
                return;
            }

            EnemyProjectile projectile = projectiles.Get();

            if ( projectile != null )
            {
                projectile.Expired += OnExpired;
                projectile.transform.position = firingPoint.position;
                projectile.transform.rotation = firingPoint.rotation;

                projectile.Setup(projectileSpeed);

                Debug.Log("pew");

                if ( cooldownRoutine != null )
                {
                    cooldownRoutine = StartCoroutine(OnCooldown());
                }
            }

            _lastDidSomething = Time.time;
        }

        private void OnExpired(EnemyProjectile projectile)
        {
            projectile.Expired -= OnExpired;

            if ( !projectiles.Recycle(projectile) )
            {
                Debug.LogError("Couldn't recycle the projectile back to the pool!");
            }
        }

        private IEnumerator OnCooldown()
        {
            onCooldown = true;
            yield return new WaitForSeconds(timeBetweenShots);
            onCooldown = false;

            cooldownRoutine = null;
        }
    }
}