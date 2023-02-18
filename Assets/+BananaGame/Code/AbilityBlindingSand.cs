using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class AbilityBlindingSand : MonoBehaviour
    {
        [SerializeField, Tooltip("Angle of the cone in degrees")]
        private float coneAngle = 30.0f;
        [SerializeField, Tooltip("The length / distance of the cone")]
        private float coneDistance = 10.0f;
        [SerializeField, Tooltip("The duration of the particle effect")]
        private float duration = 3.0f;

        [SerializeField] ParticleSystem sandParticles;

        private void Start()
        {
            // set the particle system to inactive at start
            sandParticles.gameObject.SetActive(false);
        }

        private void Update()
        {
            
        }
        
        public void OnAbility(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                sandParticles.gameObject.SetActive(true);

                sandParticles.Play();
                StartCoroutine(DeactivateParticles());
            }
        }

        private IEnumerator DeactivateParticles()
        {
            // wait for duration of the particle effect
            yield return new WaitForSeconds(duration);

            // deactivate the particle system
            sandParticles.Stop();
            sandParticles.gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            // draw cone to show area of effect in editor
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
            Gizmos.DrawRay(transform.position, transform.forward * coneDistance);

            Vector3 leftRayDirection = Quaternion.Euler(0f, -coneAngle / 2f, 0f) * transform.forward;
            Vector3 rightRayDirection = Quaternion.Euler(0f, coneAngle / 2f, 0f) * transform.forward;

            Gizmos.DrawRay(transform.position, leftRayDirection * coneDistance);
            Gizmos.DrawRay(transform.position, rightRayDirection * coneDistance);

            Gizmos.DrawLine(transform.position + leftRayDirection * coneDistance, transform.position + rightRayDirection * coneDistance);
        }
    }
}
