using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class SimpleObjectRotator : MonoBehaviour
    {
        [SerializeField, Tooltip("How much this object rotates per FixedUpdate (50 times per second)")]
        private Vector3 rotationAngles = new Vector3(0.0f, 2.0f, 0.0f);

        private void FixedUpdate()
        {
            transform.Rotate(rotationAngles);
        }
    }
}
