using System.Collections;
using UnityEngine;

namespace BananaSoup.Utilities
{
    public class PressurePlateMoverer : MonoBehaviour
    {
        private Vector3 plateStartPosition = Vector3.zero;
        private Vector3 plateEndPositionOffset = new Vector3(0f, 0.05f, 0f);
        private Vector3 plateEndPosition = Vector3.zero;

        private Vector3 plateCurrentPosition = Vector3.zero;

        private float checkDelay = 0.1f;

        private int objectsOnPlate = 0;

        private Coroutine checkForObjectRoutine = null;

        private MovablePressureplate pressureplate;

        private void OnDisable()
        {
            TryStopAndNullCoroutine();
        }

        private void Start()
        {
            pressureplate = GetComponentInChildren<MovablePressureplate>();
            if ( pressureplate == null )
            {
                Debug.LogError($"{name} couldn't find a component of type {typeof(MovablePressureplate)} on it's children!");
            }

            plateCurrentPosition = pressureplate.Position;
            plateStartPosition = pressureplate.Position;
            plateEndPosition = pressureplate.Position - plateEndPositionOffset;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ( objectsOnPlate == 0 )
            {
                ActivatePressurePlate();

                if ( checkForObjectRoutine == null )
                {
                    checkForObjectRoutine = StartCoroutine(CheckIfObjectOnPlate(other.gameObject));
                }
            }

            objectsOnPlate++;
        }

        private void OnTriggerExit(Collider other)
        {
            objectsOnPlate--;

            if ( objectsOnPlate == 0 )
            {
                DeactivatePressurePlate(); 
            }
        }

        private IEnumerator CheckIfObjectOnPlate(GameObject otherObject)
        {
            while ( true )
            {
                if ( !otherObject.activeSelf )
                {
                    DeactivatePressurePlate();
                    yield break;
                }

                yield return new WaitForSeconds(checkDelay);
            }
        }

        private void ActivatePressurePlate()
        {
            plateCurrentPosition = Vector3.Lerp(plateCurrentPosition, plateEndPosition, 2.0f);
            pressureplate.transform.position = plateCurrentPosition;
        }

        private void DeactivatePressurePlate()
        {
            plateCurrentPosition = Vector3.Lerp(plateCurrentPosition, plateStartPosition, 2.0f);
            pressureplate.transform.position = plateCurrentPosition;
            TryStopAndNullCoroutine();
        }

        private void TryStopAndNullCoroutine()
        {
            if ( checkForObjectRoutine != null )
            {
                StopCoroutine(checkForObjectRoutine);
                checkForObjectRoutine = null;
            }
        }
    }
}
