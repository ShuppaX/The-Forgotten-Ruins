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
        private bool _isFired;

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
            if (_isFired)
            {
                _rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                _rb.AddForce(transform.up * 8f, ForceMode.Impulse);
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
