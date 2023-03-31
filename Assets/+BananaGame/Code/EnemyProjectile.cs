using System.Collections;
using UnityEngine;

namespace BananaSoup
{
    public class EnemyProjectile : Damager
    {
        [SerializeField] private float forwardForce = 10.0f;
        [SerializeField] private float upForce = 8.0f;
        [SerializeField] private float aliveTime = 5.0f;

        private bool _isFired = true;

        private Rigidbody _rb;
        private Coroutine _aliveTimer = null;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();

            if ( _rb == null )
            {
                Debug.LogError("No Rigidbody on projectile");
            }
        }

        private void Start()
        {
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
            if ( _isFired )
            {
                _rb.AddForce(transform.forward * forwardForce, ForceMode.Impulse);
                _rb.AddForce(transform.up * upForce, ForceMode.Impulse);
            }
        }

        public override void OnTriggerEnter(Collider collision)
        {
            base.OnTriggerEnter(collision);

            Debug.Log($"Collided with {collision.gameObject}");
            Destroy(gameObject);
        }

        private IEnumerator AliveTimer()
        {
            yield return new WaitForSeconds(aliveTime);

            Destroy(gameObject);
            _aliveTimer = null;
        }
    }
}
