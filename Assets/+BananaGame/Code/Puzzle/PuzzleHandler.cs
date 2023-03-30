using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BananaSoup.PuzzleSystem
{
    public class PuzzleHandler : MonoBehaviour
    {
        [Tooltip("An array of PuzzleObjects which should be marked as done that the puzzle is set completed.")]
        [SerializeField] private PuzzleObjectBase[] puzzleGameObjects;

        [Tooltip("A GameObject that is affected after the puzzle is solved.")]
        [SerializeField] private PuzzleSolutionGameObject puzzleSolutionGameObject;

        public UnityAction onPuzzleObjectCheck;
        private int remainingPuzzleObjects;

        public int SetRemainingPuzzleObjectCount
        {
            set
            {
                remainingPuzzleObjects += value;
                onPuzzleObjectCheck.Invoke();
            }
        }

        private void OnEnable()
        {
            onPuzzleObjectCheck += CheckIsPuzzleSolved;
        }

        private void OnDisable()
        {
            onPuzzleObjectCheck -= CheckIsPuzzleSolved;
        }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            if ( puzzleGameObjects.Length <= 0 )
            {
                Debug.LogError(this + "'s puzzleGameObjects.Lenght is zero (" + puzzleGameObjects.Length + ") and it can't be!");
            }
            remainingPuzzleObjects = puzzleGameObjects.Length;

            if ( puzzleSolutionGameObject == null )
            {
                Debug.LogError(this + " is missing a reference to the puzzleSolutionGameObject component and it is required!");
            }

            for ( int i = 0; i < puzzleGameObjects.Length; i++ )
            {
                puzzleGameObjects[i].SetManager(this);
            }
        }

        private void CheckIsPuzzleSolved()
        {
            if ( remainingPuzzleObjects == 0 )
            {
                puzzleSolutionGameObject.IsSolved = true;
            }
            else if ( puzzleSolutionGameObject.IsSolved && remainingPuzzleObjects > 0 )
            {
                puzzleSolutionGameObject.IsSolved = false;
            }
        }
    }
}
