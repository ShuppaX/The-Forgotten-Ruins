using BananaSoup.PuzzleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class PressurePlateInteraction : PuzzleObjectBase
    {
        private bool isActivated;
        private float distanceCompare = 0.3f;
        private Transform activationObject;

        private void OnTriggerStay(Collider other)
        {
            Vector3 othersBottomPoint = other.bounds.center;
            othersBottomPoint.y -= other.bounds.extents.y;

            float distance = 0.0f;
            distance = (transform.position - othersBottomPoint).sqrMagnitude;

            if ( !isActivated && distance < distanceCompare )
            {
                isActivated = true;
                activationObject = other.transform;
                GetPuzzleManager.SetRemainingPuzzleObjectCount = -1;
            }
            else if ( isActivated && (distance > distanceCompare) && (activationObject == other.transform) )
            {
                isActivated = false;
                GetPuzzleManager.SetRemainingPuzzleObjectCount = 1;
            }
        }
    }
}
