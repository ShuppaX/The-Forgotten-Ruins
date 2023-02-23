using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace BananaSoup
{
    public class MeleeBasic : MonoBehaviour
    {
        public float mRaycastRadius; // width of our line of sight (x-axis and y-axis)
        public float mTargetDetectionDistance; // depth of our line of sight (z-axis)

        private RaycastHit _mHitInfo; // allocating memory for the raycasthit

        // to avoid Garbage
        private bool _bHasDetectedEnnemy = false; // tracking whether the player
        // is detected to change color in gizmos

        private void Start()
        {
            throw new NotImplementedException();
        }

        private void Update()
        {
            CheckForTargetInLineOfSight();
        }


        public void CheckForTargetInLineOfSight()
        {
            //Spherecast only goes forward. Need to use physicsoverlapSphere for all directions.
            _bHasDetectedEnnemy = Physics.SphereCast(transform.position, mRaycastRadius, transform.forward,
                mTargetDetectionDistance);

            if (_bHasDetectedEnnemy)
            {
                if (_mHitInfo.transform.CompareTag("Player"))
                    Debug.Log("Detected Player");
                // insert fighting logic here
                else
                    Debug.Log("No Player detected");
                // no player detected, insert your own logic
            }
            else
            {
                // no player detected, insert your own logic
            }
        }

        private void OnDrawGizmos()
        {
            if (_bHasDetectedEnnemy)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;

            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawCube(new Vector3(0f, 0f, mTargetDetectionDistance / 2f),
                new Vector3(mRaycastRadius, mRaycastRadius, mTargetDetectionDistance));
        }
    }
}