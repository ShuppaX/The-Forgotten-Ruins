using BananaSoup.InteractSystem;
using BananaSoup.Puzzle;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class PressurePlateAction : PuzzleObjectBase
    {
        private bool isActivated;

        private PressureplateFlash flasher;

        private void Start()
        {
            flasher = GetComponentInChildren<PressureplateFlash>();
            if ( flasher == null )
            {
                Debug.Log($"{name} couldn't find a component of type {typeof(PressureplateFlash)} from it's children!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ( other.gameObject.GetComponent<LiftableRockAction>() != null )
            {
                if ( !isActivated )
                {
                    isActivated = true;
                    GetPuzzleManager.SetRemainingPuzzleObjectCount = -1;
                    flasher.CallFlash();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ( other.gameObject.GetComponent<LiftableRockAction>() != null )
            {
                if ( isActivated )
                {
                    isActivated = false;
                    GetPuzzleManager.SetRemainingPuzzleObjectCount = 1;
                    flasher.ResetColorAndEmission();
                }
            }
        }
    }
}
