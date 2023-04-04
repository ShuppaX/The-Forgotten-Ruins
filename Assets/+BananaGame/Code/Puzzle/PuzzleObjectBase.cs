using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class PuzzleObjectBase : MonoBehaviour
    {
        [SerializeField, Tooltip("If true, this puzzle object is marked solved when for example, fire is extinguished or box is on platform. " +
                              "If false, this puzzle object is marked solved when reverse requirements are met.")]
        private bool isSolutionReversed = false;

        private PuzzleHandler thisPuzzleManager;

        public PuzzleHandler GetPuzzleManager => thisPuzzleManager;
        public bool IsSolutionReversed
        {
            get => isSolutionReversed;
            set => isSolutionReversed = value;
        }

        public void SetHandler(PuzzleHandler manager)
        {
            thisPuzzleManager = manager;
        }
    }
}
