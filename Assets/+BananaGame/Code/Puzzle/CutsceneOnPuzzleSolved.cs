using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace BananaSoup.PuzzleSystem
{
    public class CutsceneOnPuzzleSolved : PuzzleSolutionGameObject
    {
        [SerializeField] private PlayableDirector director;
        private bool isCutsceneActivated;

        private void Update()
        {
            if ( IsSolved && !isCutsceneActivated)
            {
                Debug.Log("Puzzle solved, playing cutscene");
                isCutsceneActivated = true;
                director.Play();
            }
        }
    }
}
