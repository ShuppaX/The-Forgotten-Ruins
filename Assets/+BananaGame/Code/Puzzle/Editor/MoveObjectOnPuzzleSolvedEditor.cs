using UnityEditor;
using UnityEngine;

namespace BananaSoup.PuzzleSystem.CustomInspector
{
    [CustomEditor(typeof(MoveObjectOnPuzzleSolved), true)]
    public class MoveObjectOnPuzzleSolvedEditor : Editor
    {
        private MoveObjectOnPuzzleSolved moveObjectOnPuzzleSolved;

        private void OnEnable()
        {
            moveObjectOnPuzzleSolved = (MoveObjectOnPuzzleSolved)target;
        }

        public override void OnInspectorGUI()
        {
            if ( GUILayout.Button("Set current location") )
            {
                moveObjectOnPuzzleSolved.SetCurrentLocation();
            }

            base.OnInspectorGUI();
        }
    }
}
