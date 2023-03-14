using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class SimpleObjectFollower : MonoBehaviour
    {
        [SerializeField] private Vector3 offsets;
        private Transform target;

        private void Start()
        {
            target = PlayerBase.Instance.transform;
        }

        private void LateUpdate()
        {
            transform.position = target.position + offsets;
        }
    }
}
