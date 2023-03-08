using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class AbilityBlindingSand : MonoBehaviour
    {
        [SerializeField] ParticleSystem sandParticles;
        //[SerializeField, Tooltip("Angle of the cone in degrees")]
        //private float coneAngle = 30.0f;
        //[SerializeField, Tooltip("The length / distance of the cone")]
        //private float coneDistance = 10.0f;

        // The duration of the particle effect, taken automatically from Particle System's Start Lifetime.
        private float duration;


        private Coroutine activeParticleEffect;

        private void Start()
        {
            // set the particle system to inactive at start
            sandParticles.gameObject.SetActive(false);

            duration = sandParticles.main.startLifetime.constant;
        }

        public void OnAbility(InputAction.CallbackContext context)
        {
            // Don't cast a sand if old currently playing.
            if ( activeParticleEffect != null )
            {
                return;
            }

            if ( context.performed )
            {
                sandParticles.gameObject.transform.parent = null;
                sandParticles.gameObject.transform.position = transform.position;

                // TODO: Fix rotation
                //Vector3 rotation = new Vector3(sandParticles.gameObject.transform.x, sandParticles.gameObject.transform.rotation.y);
                //Debug.Log("Rotation: " + rotation);
                //sandParticles.gameObject.transform.eulerAngles = rotation;
                //Debug.Log("Particle rotation: " + sandParticles.gameObject.transform.localRotation);
                sandParticles.gameObject.transform.rotation = transform.rotation;

                sandParticles.gameObject.SetActive(true);

                activeParticleEffect = StartCoroutine(DeactivateParticles());
            }
        }

        private IEnumerator DeactivateParticles()
        {
            sandParticles.Play();
            // wait for duration of the particle effect
            yield return new WaitForSeconds(duration);

            // deactivate the particle system
            sandParticles.Stop();
            sandParticles.gameObject.SetActive(false);
            activeParticleEffect = null;
        }

        // NOTE: This should be on Particle System?
        private void OnParticleCollision(GameObject other)
        {
            Debug.Log("Collision: " + other.name);
        }

        //private void OnDrawGizmos()
        //{
        //    // draw cone to show area of effect in editor
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawWireSphere(transform.position, 0.1f);
        //    Gizmos.DrawRay(transform.position, transform.forward * coneDistance);

        //    Vector3 leftRayDirection = Quaternion.Euler(0f, -coneAngle / 2f, 0f) * transform.forward;
        //    Vector3 rightRayDirection = Quaternion.Euler(0f, coneAngle / 2f, 0f) * transform.forward;

        //    Gizmos.DrawRay(transform.position, leftRayDirection * coneDistance);
        //    Gizmos.DrawRay(transform.position, rightRayDirection * coneDistance);

        //    Gizmos.DrawLine(transform.position + leftRayDirection * coneDistance, transform.position + rightRayDirection * coneDistance);
        //}
    }
}
