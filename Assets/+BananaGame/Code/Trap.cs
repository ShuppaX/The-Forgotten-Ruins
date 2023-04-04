using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private GameObject trapDamageObject;
        [SerializeField] private bool isRepeatable = true;

        private void OnTriggerEnter(Collider other)
        {
            
        }
    }
}
