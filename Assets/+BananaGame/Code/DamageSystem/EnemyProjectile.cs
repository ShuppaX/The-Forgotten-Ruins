using System;
using System.Collections;
using UnityEngine;

namespace BananaSoup.DamageSystem
{
    public class EnemyProjectile : Damager
    {
        [SerializeField] private float forwardForce = 10.0f; //projectiles forward force
        [SerializeField] private float upForce = 8.0f; //projectiles up force
        [SerializeField] private float aliveTime = 1.5f; //how long the projectile will live for

        private bool _isFired = false;

        private Rigidbody _rb;
        private Coroutine _aliveTimer = null;

        public event Action<EnemyProjectile> Expired;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();

            if ( _rb == null )
            {
                Debug.LogError("No Rigidbody on projectile");
            }
        }

        public void Setup(float speed = -1)
        {
            if ( speed > 0 )
            {
                forwardForce = speed;
            }

            _isFired = true;

            if ( _aliveTimer == null )
            {
                _aliveTimer = StartCoroutine(AliveTimer());
            }
        }

        private void OnDisable()
        {
            if ( _aliveTimer != null )
            {
                StopCoroutine(_aliveTimer);
                _aliveTimer = null;
            }
        }

        private void FixedUpdate()
        {
            
            //when fired the projectile gets a force impulse in the forward and up directions
            if ( _isFired )
            {
                var projectileTransform = transform;
                _rb.velocity = projectileTransform.forward * forwardForce;
                _rb.AddForce(projectileTransform.up * upForce, ForceMode.Impulse);
            }
        }

        public override void OnTriggerStay(Collider collision)
        {
            base.OnTriggerStay(collision);

            //Debug.Log($"{gameObject} Collided with {collision.gameObject}");
            Recycle();
        }

        private void Recycle()
        {
            if ( _aliveTimer != null )
            {
                StopCoroutine(_aliveTimer);
                _aliveTimer = null;
            }

            Expired?.Invoke(this);
        }

        private IEnumerator AliveTimer()
        {
            yield return new WaitForSeconds(aliveTime);
            Debug.Log("A projectile should call Recycle now!");

            Recycle();
        }
    }
}
