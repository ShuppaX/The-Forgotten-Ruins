using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class PuzzleSolutionGameObject : MonoBehaviour
    {
        private bool isSolved;

        public bool IsSolved
        {
            get { return isSolved; }
            set { isSolved = value; }
        }
    }
}
