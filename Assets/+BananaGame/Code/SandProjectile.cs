using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class SandProjectile : MonoBehaviour
    {
        private ParticleSystem sandEffect;

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

        // TODO: Add shadow to particle effect.

        private void OnParticleCollision(GameObject other)
        {
            // TODO: Set collision with an enemy.
            // TODO: Set collision with an torch (put the fire out).
            Debug.Log("Collision with: " + other.name);
        }
    }
}
