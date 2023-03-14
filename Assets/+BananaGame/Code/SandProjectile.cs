using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace BananaSoup
{
    public class SandProjectile : MonoBehaviour
    {
        [SerializeField] private LayerMask collideWithLayers;
        [SerializeField][Layer] private string enemyLayer;
        // TODO: Add a layer to torches
        // [SerializeField][Layer] private string torchLayer;
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
            // Sand particles collided with an enemy.
            if ( !isCollisionDetected && other.gameObject.layer == LayerMask.NameToLayer(enemyLayer) )
            {
                Debug.Log(name + " collided with: " + other.name);
                isCollisionDetected = true;
            }

            // TODO: Set collision with an torch and put the fire out.
            //if ( !isCollisionDetected && other.gameObject.layer == LayerMask.NameToLayer(torchLayer) )
            //{
            //    Debug.Log(name + " collided with: " + other.name);
            //    isCollisionDetected = true;
            //}
        }
    }
}
