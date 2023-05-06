using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine;

namespace BananaSoup
{
    public class DetectNearbyIThrowReactables : MonoBehaviour
    {
        [SerializeField, Tooltip("The maximum distance to detect IThrowables near the player.")]
        private float maxCastDistance = 3.0f;
        [SerializeField, Tooltip("The radius of the SphereCast.")]
        private float sphereRadius = 1.0f;
        [SerializeField, Tooltip("Select the layers you want the SphereCast to collide with.")]
        private LayerMask collidableLayer;

        [Space]

        [SerializeField, Tooltip("The maximum angle that the throwable can be rotated towards.")]
        private float maximumRotationAngle = 50.0f;

        [Space]

        [SerializeField, Tooltip("Turn this on to draw sphere gizmo!")]
        private bool isDrawingSphere = false;

        private Quaternion calculatedRotation = Quaternion.identity;
        public Quaternion CalculatedRotation
        {
            get => calculatedRotation;
        }

        public void OnThrow()
        {
            Vector3 spheresCastingLocation = (transform.position + (transform.forward * -sphereRadius));
            RaycastHit hit;
            if ( isDrawingSphere )
            {
                if ( RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(spheresCastingLocation, sphereRadius, transform.forward, out hit, maxCastDistance, collidableLayer, PreviewCondition.Editor, 0f, Color.green, Color.red) )
                {
                    if ( hit.transform.gameObject.GetComponent<IThrowReactable>() != null )
                    {
                        Debug.Log("The sphere hit a IThrowReactable!");

                        Vector3 directionToHit = hit.point - transform.position;

                        directionToHit.y = 0;

                        Quaternion targetRotation = Quaternion.LookRotation(directionToHit);

                        //Vector3 euler = targetRotation.eulerAngles;
                        //euler.x = transform.rotation.eulerAngles.x;
                        //euler.z = transform.rotation.eulerAngles.z;
                        //targetRotation = Quaternion.Euler(euler);

                        float angleOfRotation = Quaternion.Angle(transform.rotation, targetRotation);

                        if (angleOfRotation <= maximumRotationAngle )
                        {
                            calculatedRotation = targetRotation;
                            Debug.Log("The rotation was within the allowed range!");
                        }
                        else
                        {
                            calculatedRotation = Quaternion.identity;
                        }
                    }
                }
                else
                {
                    calculatedRotation = Quaternion.identity;
                }
            }
            else
            {
                if ( UnityEngine.Physics.SphereCast(spheresCastingLocation, sphereRadius, transform.forward, out hit, maxCastDistance, collidableLayer) )
                {
                    if ( hit.transform.gameObject.GetComponent<IThrowReactable>() != null )
                    {
                        Vector3 directionToHit = hit.point - transform.position;

                        directionToHit.y = 0;

                        Quaternion targetRotation = Quaternion.LookRotation(directionToHit);

                        //Vector3 euler = targetRotation.eulerAngles;
                        //euler.x = transform.rotation.eulerAngles.x;
                        //euler.z = transform.rotation.eulerAngles.z;
                        //targetRotation = Quaternion.Euler(euler);

                        float angleOfRotation = Quaternion.Angle(transform.rotation, targetRotation);

                        if ( angleOfRotation <= maximumRotationAngle )
                        {
                            calculatedRotation = targetRotation;
                        }
                        else
                        {
                            calculatedRotation = Quaternion.identity;
                        }
                    }
                }
                else
                {
                    calculatedRotation = Quaternion.identity;
                }
            }
        }
    }
}
