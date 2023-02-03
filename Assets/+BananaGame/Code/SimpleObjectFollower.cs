using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class SimpleObjectFollower : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offsets;

        private void LateUpdate()
        {
            transform.position = target.position + offsets;
        }
    }
}
