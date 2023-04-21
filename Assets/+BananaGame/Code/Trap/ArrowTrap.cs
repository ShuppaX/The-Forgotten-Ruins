using BananaSoup.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.Traps
{
    public class ArrowTrap : Trap
    {
        [SerializeField] private EnemyProjectile arrowProjectile;
        [SerializeField] private Transform[] arrowShooter;
        [SerializeField] private int poolSize = 10;
        private Coroutine trapRoutine;
        private bool arrowsFired;
        private ComponentPool<EnemyProjectile> projectiles;

        private void Awake()
        {
            projectiles = new ComponentPool<EnemyProjectile>(arrowProjectile, poolSize);
        }

        private void OnDisable()
        {
            if ( trapRoutine != null )
            {
                StopCoroutine(Cooldown());
                trapRoutine = null;
            }
        }

        public override void ActivateTrap()
        {
            if ( !arrowsFired )
            {
                arrowsFired = true;
                ShootArrow();
            }
        }

        private void ShootArrow()
        {
            foreach ( var shooter in arrowShooter )
            {
                EnemyProjectile projectile = projectiles.Get();
                Vector3 arrowSpawnOffset = shooter.transform.forward * 0.2f;
                projectile.transform.position = shooter.position - arrowSpawnOffset;
                projectile.transform.rotation = shooter.transform.rotation;
                projectile.Expired += OnExpired;
                projectile.Setup();
            }

            if ( IsRepeatable )
            {
                trapRoutine = StartCoroutine(Cooldown());
            }
        }

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(GetCooldown);
            arrowsFired = false;
            trapRoutine = null;
            IsTrapActivated = false;
        }

        private void OnExpired(EnemyProjectile projectile)
        {
            projectile.Expired -= OnExpired;

            if ( !projectiles.Recycle(projectile) )
            {
                Debug.LogError("Couldn't recycle the projectile back to the pool!");
            }
        }
    }
}
