using System;
using System.Collections;
using UnityEngine;

namespace BananaSoup
{
    public class ParticleProjectile : MonoBehaviour
    {
        public enum Type
        {
            Sand = 0,
            Spark = 1
        }

        [SerializeField] private float aliveTime = 3.0f;
        [HideInInspector] public Type projectileType;
        private Coroutine aliveTimer = null;
        private bool isCollisionDetected;
        private ParticleSystem particleEffect;
        public event Action<ParticleProjectile> Expired;

        public bool IsCollisionDetected
        {
            get { return isCollisionDetected; }
            set { isCollisionDetected = value; }
        }

        private void OnEnable()
        {
            isCollisionDetected = false;
        }

        private void Awake()
        {
            GetReference();
        }

        // NOTE: This seems to be obsolete because script execution order
        private void GetReference()
        {
            particleEffect = GetComponent<ParticleSystem>();
            if ( particleEffect == null )
            {
                Debug.LogError(name + " is missing a reference to a ParticleSystem!");
            }
        }

        public void Setup()
        {
            // HACK: Getting reference to the particle effect, because Awake doesn't fire up fast enough
            if ( particleEffect == null )
            {
                particleEffect = GetComponent<ParticleSystem>();

                if ( particleEffect == null )
                {
                    Debug.LogError(name + " is missing a reference to a ParticleSystem!");
                }
            }

            if ( aliveTimer == null )
            {
                particleEffect.Play();
                aliveTimer = StartCoroutine(AliveTimer());
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            if ( IsCollisionDetected )
            {
                return;
            }

            if ( other.TryGetComponent(out IThrowReactable target) )
            {
                IsCollisionDetected = true;
                target.OnThrowAbility(projectileType);

                Recycle();
            }
        }

        private IEnumerator AliveTimer()
        {
            yield return new WaitForSeconds(aliveTime);
            Debug.Log("A projectile should call Recycle now!");

            Recycle();
        }

        private void Recycle()
        {
            if ( aliveTimer != null )
            {
                StopCoroutine(aliveTimer);
                particleEffect.Stop();
                aliveTimer = null;
            }

            if ( Expired != null )
            {
                Expired(this);
            }
        }
    }
}
