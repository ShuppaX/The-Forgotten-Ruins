using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace BananaSoup
{
    public class SandProjectile : MonoBehaviour
    {
        private bool isCollisionDetected;
        private ParticleSystem sandEffect;

        private void OnEnable()
        {
            isCollisionDetected = false;
        }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            sandEffect = GetComponent<ParticleSystem>();
            if ( sandEffect == null )
            {
                Debug.LogError(name + " is missing a ParticleSystem!");
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            if ( !isCollisionDetected )
            {
                return;
            }

            if ( other.TryGetComponent(out ISandable sandable) )
            {
                isCollisionDetected = true;
                sandable.OnSandAttack();
            }
        }
    }
}
