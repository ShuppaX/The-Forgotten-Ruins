using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class CheckNearInteractables : MonoBehaviour
    {
        [SerializeField] private float distance = 0.5f;
        private void FixedUpdate()
        {
            RaycastHit hit;
            if ( Physics.Raycast(transform.position, transform.forward, out hit, distance) )
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
                Debug.Log("Hit");
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * distance, Color.yellow);
                Debug.Log("No hit");
            }
        }
    }
}
