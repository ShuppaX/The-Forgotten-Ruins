using System;
using System.Collections;
using UnityEngine;

namespace BananaSoup
{
    public class EnemyProjectile : Damager
    {
        [SerializeField] private float forwardForce = 10.0f; //projectiles forward force
        [SerializeField] private float upForce = 8.0f; //projectiles up force
        [SerializeField] private float aliveTime = 5.0f; //how long the projectile will live for

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
                _rb.velocity = transform.forward * forwardForce;
                _rb.AddForce(transform.up * upForce, ForceMode.Impulse);
            }
        }

        public override void OnTriggerEnter(Collider collision)
        {
            base.OnTriggerEnter(collision);

            Debug.Log($"Collided with {collision.gameObject}");
            Recycle();
        }

        private void Recycle()
        {
            if ( _aliveTimer != null )
            {
                StopCoroutine(_aliveTimer);
                _aliveTimer = null;
            }

            if ( Expired != null )
            {
                Expired(this);
            }
        }

        private IEnumerator AliveTimer()
        {
            yield return new WaitForSeconds(aliveTime);
            Debug.Log("A projectile should call Recycle now!");

            Recycle();
        }
    }
}
