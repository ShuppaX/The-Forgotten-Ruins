using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class MoveObjectOnPuzzleSolved : MonoBehaviour, IPuzzle
    {
        [SerializeField] private Vector3 endPoint;
        [SerializeField] private float lerpModifier = 3.0f;
        private float distanceCompare = 0.0001f;
        private bool isSolved;
        private bool hasMoved;

        public void OnPuzzleSolved()
        {
            isSolved = true;
        }

        private void FixedUpdate()
        {
            if ( isSolved && !hasMoved )
            {
                transform.position = Vector3.Lerp(transform.position, endPoint, Time.deltaTime * lerpModifier);
                float distance = (transform.position - endPoint).sqrMagnitude;
                if( distance < distanceCompare )
                {
                    hasMoved = true;
                }
            }
        }
    }
}
