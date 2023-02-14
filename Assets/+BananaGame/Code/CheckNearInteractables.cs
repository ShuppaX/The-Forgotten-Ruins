using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class CheckNearInteractables : MonoBehaviour
    {
        [SerializeField] private float distance = 0.5f;
        //private void FixedUpdate()
        //{
        //    RaycastHit hit;
        //    if ( Physics.Raycast(transform.position, transform.forward, out hit, distance) )
        //    {
        //        Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
        //        Debug.Log("Hit");
        //    }
        //}

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("OnCollisionEnter");
            if ( other.transform.GetComponent<Interactable>() != null )
            {
                Debug.Log("Player collided with: " + other.gameObject);
            }


            if ( TryGetComponent(out Interactable interactable) )
            {
                Debug.Log("Interactable [" + interactable + "] on range");
            }

        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    //Debug.Log("OnCollisionEnter");
        //    if ( collision.transform.GetComponent<Interactable>() != null )
        //    {
        //        Debug.Log("Player collided with: " + collision.gameObject);
        //    }


        //    if ( TryGetComponent(out Interactable interactable) )
        //    {
        //        Debug.Log("Interactable [" + interactable + "] on range");
        //    }
        //}
    }
}
