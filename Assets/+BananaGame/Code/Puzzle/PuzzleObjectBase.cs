using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class PuzzleObjectBase : MonoBehaviour
    {
        private PuzzleManager thisPuzzleManager;

        public PuzzleManager GetPuzzleManager
        {
            get { return thisPuzzleManager; }
        }

        public void SetManager(PuzzleManager manager)
        {
            thisPuzzleManager = manager;
        }
    }
}
