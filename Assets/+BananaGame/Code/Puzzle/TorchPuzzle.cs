using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BananaSoup
{
    public class TorchPuzzle : MonoBehaviour
    {
        [Tooltip("An array of torches which should be extinguished that the puzzle is set completed.")]
        [SerializeField] private TorchInteraction[] torches;
        [Tooltip("A GameObject that is affected after the puzzle is solved.")]
        [SerializeField] private GameObject puzzleSolutionGameObject;
        public UnityAction onTorchExtinguished;
        private int remainingTorches;
        private IPuzzle puzzleComponent;

        public int SetTorchExtinguished
        {
            set
            {
                remainingTorches -= value;
                onTorchExtinguished.Invoke();
            }
        }

        private void OnEnable()
        {
            onTorchExtinguished += OnTorchExtinguished;
        }

        private void OnDisable()
        {
            onTorchExtinguished -= OnTorchExtinguished;
        }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            remainingTorches = torches.Length;
            puzzleComponent = puzzleSolutionGameObject.GetComponent<IPuzzle>();
            if ( puzzleComponent == null )
            {
                Debug.LogError(this + " is missing an IPuzzle component and it is required!");
            }
        }

        private void OnTorchExtinguished()
        {
            if ( remainingTorches == 0 )
            {
                puzzleComponent.OnPuzzleSolved();
            }
        }
    }
}
