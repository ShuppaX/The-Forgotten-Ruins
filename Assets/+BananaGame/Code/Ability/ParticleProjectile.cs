using System.Collections;
using System.Collections.Generic;
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

        [HideInInspector] public Type projectileType;

        private bool isCollisionDetected;
        private ParticleSystem particleEffect;

        public bool IsCollisionDetected
        {
            get { return isCollisionDetected; }
            set { isCollisionDetected = value; }
        }

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
            particleEffect = GetComponent<ParticleSystem>();
            if ( particleEffect == null )
            {
                Debug.LogError(name + " is missing a reference to a ParticleSystem!");
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
            }
        }
    }
}
