using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BananaSoup.PuzzleSystem
{
    public class PuzzleObject : MonoBehaviour
    {
        [Tooltip("An array of PuzzleObjects which should be marked as done that the puzzle is set completed.")]
        [SerializeField] private PuzzleObjectBase[] puzzleGameObjects;

        [Tooltip("A GameObject that is affected after the puzzle is solved.")]
        [SerializeField] private PuzzleSolutionGameObject puzzleSolutionGameObject;

        public UnityAction onPuzzleObjectSolved;
        private int remainingPuzzleObjects;

        public int SetRemainingPuzzleObjectCount
        {
            set
            {
                remainingPuzzleObjects += value;
                onPuzzleObjectSolved.Invoke();
            }
        }

        private void OnEnable()
        {
            onPuzzleObjectSolved += OnTorchExtinguished;
        }

        private void OnDisable()
        {
            onPuzzleObjectSolved -= OnTorchExtinguished;
        }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            if ( puzzleGameObjects.Length <= 0 )
            {
                Debug.LogError(this + " puzzleGameObjects.Lenght is zero (" + puzzleGameObjects.Length + ") and it can't be!");
            }
            remainingPuzzleObjects = puzzleGameObjects.Length;

            if ( puzzleSolutionGameObject == null )
            {
                Debug.LogError(this + " is missing a reference to the puzzleSolutionGameObject component and it is required!");
            }
        }

        private void OnTorchExtinguished()
        {
            if ( remainingPuzzleObjects == 0 )
            {
                puzzleSolutionGameObject.IsSolved = true;
            }
        }
    }
}
