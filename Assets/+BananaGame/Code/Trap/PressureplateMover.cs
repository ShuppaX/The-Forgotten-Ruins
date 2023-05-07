using UnityEngine;

namespace BananaSoup.Traps
{
    public class PressureplateMover : MonoBehaviour
    {
        private Vector3 plateStartPosition = Vector3.zero;
        private Vector3 plateEndPositionOffset = new Vector3(0f, 0.05f, 0f);
        private Vector3 plateEndPosition = Vector3.zero;

        private Vector3 plateCurrentPosition = Vector3.zero;

        private MovablePressureplate pressureplate;

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
            plateCurrentPosition = Vector3.Lerp(plateCurrentPosition, plateEndPosition, 2.0f);
            pressureplate.transform.position = plateCurrentPosition;
        }

        private void OnTriggerExit(Collider other)
        {
            plateCurrentPosition = Vector3.Lerp(plateCurrentPosition, plateStartPosition, 2.0f);
            pressureplate.transform.position = plateCurrentPosition;
        }
    }
}
