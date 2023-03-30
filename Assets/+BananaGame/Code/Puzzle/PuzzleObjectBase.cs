using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class PuzzleObjectBase : MonoBehaviour
    {
        private PuzzleHandler thisPuzzleManager;

        public PuzzleHandler GetPuzzleManager
        {
            get { return thisPuzzleManager; }
        }

        public void SetManager(PuzzleHandler manager)
        {
            thisPuzzleManager = manager;
        }
    }
}
