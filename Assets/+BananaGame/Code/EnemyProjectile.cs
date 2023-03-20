using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        [SerializeField] private float duration = 5;

        private float _durationTimer;

        private Rigidbody _rb;
        private bool _isFired = true;
        [SerializeField] private float forwardForce = 10.0f;
        [SerializeField] private float upForce = 8.0f;
        private Vector3 _playerDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            
            
            if (_rb == null)
            {
                Debug.LogError("No Rigidbody on projectile");
                
            }
        }

        private void FixedUpdate()
        {
            //_playerDirection = PlayerBase.transform.position;
            var whereisPlayer = _playerDirection;
            if (_isFired)
            {
                _rb.AddForce(transform.forward * forwardForce, ForceMode.Impulse);
                _rb.AddForce(transform.up * upForce, ForceMode.Impulse);
            }
        }

        
        

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Collided with {other.gameObject}");
            Destroy(gameObject);
        }
        
        private IEnumerator AliveTimer(float duration)
        {
            _durationTimer = duration;
            while(_durationTimer > 0)
            {
                _durationTimer -= Time.deltaTime;
                yield return null;
            }
            
            Destroy(gameObject);
            
        }
    }
}
