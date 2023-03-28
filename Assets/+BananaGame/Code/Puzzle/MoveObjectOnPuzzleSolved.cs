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
        private Vector3 startingPosition;
        private BoxCollider movementBlocker;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            startingPosition = transform.position;

            movementBlocker = gameObject.AddComponent<BoxCollider>();
            movementBlocker.size = new Vector3(movementBlocker.size.x, movementBlocker.size.y + 2, movementBlocker.size.z);
        }

        // TODO: If have time change this to for example, Coroutine to happen only once
        private void FixedUpdate()
        {
            // Puzzle solved, move object.
            if ( IsSolved && !hasMoved )
            {
                transform.position = Vector3.Lerp(transform.position, endPoint, Time.deltaTime * lerpModifier);
                float distance = (transform.position - endPoint).sqrMagnitude;
                if ( distance < distanceCompare )
                {
                    transform.position = endPoint;
                    hasMoved = true;
                    movementBlocker.enabled = false;
                }
            }
            // Puzzle is unsolved again, re-move it to the start position.
            // NOTE: Not the best way to make this, but it's working
            else if ( !IsSolved )
            {
                movementBlocker.enabled = true;
                hasMoved = false;
                transform.position = Vector3.Lerp(transform.position, startingPosition, Time.deltaTime * lerpModifier);
                float distance = (transform.position - startingPosition).sqrMagnitude;
                if ( distance < distanceCompare )
                {
                    transform.position = startingPosition;
                }
            }
        }
    }
}
