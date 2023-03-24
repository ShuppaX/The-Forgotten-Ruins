using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class MoveObjectOnPuzzleSolved : PuzzleSolutionGameObject
    {
        [SerializeField] private Vector3 endPoint;
        [SerializeField] private float lerpModifier = 3.0f;
        private float distanceCompare = 0.001f;
        private bool hasMoved;

        // TODO: If have time change this to for example, Coroutine to happen only once
        private void FixedUpdate()
        {
            if ( IsSolved && !hasMoved )
            {
                transform.position = Vector3.Lerp(transform.position, endPoint, Time.deltaTime * lerpModifier);
                float distance = (transform.position - endPoint).sqrMagnitude;
                if( distance < distanceCompare )
                {
                    transform.position = endPoint;
                    hasMoved = true;
                }
            }
        }
    }
}
