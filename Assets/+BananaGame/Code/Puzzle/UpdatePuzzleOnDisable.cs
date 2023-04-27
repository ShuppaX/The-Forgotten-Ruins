using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace BananaSoup.PuzzleSystem
{
    public class UpdatePuzzleOnDisable : PuzzleObjectBase
    {
        private void OnDisable()
        {
            if ( !IsSolutionReversed )
            {
                UpdateRemainingPuzzle(-1);
            }
            else
            {
                UpdateRemainingPuzzle(1);
            }
        }

        private void UpdateRemainingPuzzle(int value)
        {
            if ( GetPuzzleManager != null )
            {
                GetPuzzleManager.SetRemainingPuzzleObjectCount = value;
            }
        }
    }
}
