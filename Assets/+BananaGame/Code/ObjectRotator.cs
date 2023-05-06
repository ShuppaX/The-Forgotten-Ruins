using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class ObjectRotator : MonoBehaviour
    {
        [SerializeField, Tooltip("How much this object rotates per FixedUpdate (50 times per second)")]
        private float rotationAngle = 2.0f;

        private void FixedUpdate()
        {
            transform.Rotate(0, rotationAngle, 0);
        }
    }
}
