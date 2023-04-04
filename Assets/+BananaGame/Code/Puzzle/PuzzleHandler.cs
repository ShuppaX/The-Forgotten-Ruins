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

        [Tooltip("An array of GameObjects that is affected after the puzzle is solved.")]
        [SerializeField] private PuzzleSolutionGameObject[] puzzleSolutionGameObjects;

        public UnityAction onPuzzleObjectCheck;
        private int remainingPuzzleObjects;
        private bool isPuzzleSolved;

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

            if ( puzzleSolutionGameObjects == null )
            {
                Debug.LogError(this + " is missing a reference to the puzzleSolutionGameObject component and it is required!");
            }

            if ( puzzleSolutionGameObjects.Length <= 0 )
            {
                Debug.LogError(this + "'s puzzleSolutionGameObjects is empty!");
            }
            else
            {
                try
                {
                    for ( int i = 0; i < puzzleSolutionGameObjects.Length; i++ )
                    {
                        puzzleSolutionGameObjects[i].GetComponent<PuzzleSolutionGameObject>();
                    }
                }
                catch ( Exception )
                {
                    Debug.LogError(this + "'s puzzleSolutionGameObjects has null index in the array!");
                }
            }

            for ( int i = 0; i < puzzleGameObjects.Length; i++ )
            {
                // Set this Handler for the puzzleGameObjects array.
                puzzleGameObjects[i].SetHandler(this);

                try
                {
                    // Torches
                    if ( puzzleGameObjects[i].GetComponent<TorchAction>().IsTorchAlreadyBurning )
                    {
                        remainingPuzzleObjects += puzzleGameObjects[i].GetComponent<TorchAction>().GetCompletitionValueAtStart();
                    }
                    else
                    {
                        remainingPuzzleObjects += puzzleGameObjects[i].GetComponent<TorchAction>().GetCompletitionValueAtStart();
                    }
                }
                catch ( Exception )
                {
                    // Other cases
                    remainingPuzzleObjects = puzzleGameObjects.Length;
                }
            }

            CheckIsPuzzleSolved();
        }

        private void CheckIsPuzzleSolved()
        {
            if ( remainingPuzzleObjects == 0 )
            {
                isPuzzleSolved = true;
                for ( int i = 0; i < puzzleSolutionGameObjects.Length; i++ )
                {
                    puzzleSolutionGameObjects[i].IsSolved = true;
                }
            }
            else if ( isPuzzleSolved && remainingPuzzleObjects > 0 )
            {
                isPuzzleSolved = false;
                for ( int i = 0; i < puzzleSolutionGameObjects.Length; i++ )
                {
                    puzzleSolutionGameObjects[i].IsSolved = false;
                }
            }
        }
    }
}
