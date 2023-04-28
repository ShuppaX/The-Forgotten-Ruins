using System.Collections;
using UnityEngine;

namespace BananaSoup.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField, Tooltip("MainMenuCamera, used to get position, rotation and focal length.")]
        private Camera titleScreenCamera;
        [SerializeField, Tooltip("GamePlayCamera, used as the actual camera in the game and main menu.")]
        private Camera gamePlayCamera;

        [Space]

        [SerializeField, Tooltip("Transition time from TitleScreen view to GamePlay view.")]
        private float transitionTime = 5.0f;
        [SerializeField, Tooltip("Transition rotation multiplier.")]
        private float transitionRotationMultiplier = 3.0f;

        private Coroutine transitionCoroutine = null;

        private float titleScreenFocalLength = 0.0f;
        private Vector3 titleScreenPosition = Vector3.zero;
        private Quaternion titleScreenRotation = Quaternion.identity;

        private float gamePlayFocalLength = 0.0f;
        private Vector3 gamePlayStartPosition = Vector3.zero;
        private Quaternion gamePlayRotation = Quaternion.identity;

        // References
        private SimplePlayerFollower playerFollower = null;
        private Camera mainCamera;

        private void OnDisable()
        {
            if ( transitionCoroutine != null )
            {
                StopCoroutine(transitionCoroutine);
                transitionCoroutine = null;
            }
        }

        private void Awake()
        {
            playerFollower = GetComponent<SimplePlayerFollower>();
            if ( playerFollower == null )
            {
                Debug.LogError($"No component of {typeof(SimplePlayerFollower)} can be found on the {name}!");
            }

            mainCamera = GetComponent<Camera>();
            if ( mainCamera == null )
            {
                Debug.LogError($"No component of {typeof(Camera)} can be found on the {name}!");
            }

            titleScreenFocalLength = titleScreenCamera.focalLength;
            titleScreenPosition = titleScreenCamera.gameObject.transform.position;
            titleScreenRotation = titleScreenCamera.gameObject.transform.rotation;

            gamePlayFocalLength = gamePlayCamera.focalLength;
            gamePlayStartPosition = gamePlayCamera.gameObject.transform.position;
            gamePlayRotation = gamePlayCamera.gameObject.transform.rotation;
        }

        private void Start()
        {
            mainCamera.focalLength = titleScreenFocalLength;
            transform.position = titleScreenPosition;
            transform.rotation = titleScreenRotation;
        }

        public void TransitionCamera()
        {
            if ( transitionCoroutine == null )
            {
                transitionCoroutine = StartCoroutine(CameraTransitionCoroutine());
            }
        }

        private void ActivateGamePlayView()
        {
            mainCamera.focalLength = gamePlayFocalLength;
            transform.position = gamePlayStartPosition;
            transform.rotation = gamePlayRotation;

            playerFollower.enabled = true;
        }

        private IEnumerator CameraTransitionCoroutine()
        {
            float remainingTransitionTime = 0f;
            float startTime = Time.time;

            while ( remainingTransitionTime < 1.0f )
            {
                Debug.Log("Transition while loop is running!");

                if ( CheckIfCameraIsInPosition() )
                {
                    break;
                }

                float smoothStepT = Mathf.SmoothStep(0, 1, remainingTransitionTime);
                LerpMainCamera(smoothStepT);

                remainingTransitionTime = (Time.time - startTime) / transitionTime;

                Debug.Log(remainingTransitionTime);
                yield return null;
            }

            Debug.Log("Transition is finished!");
            ActivateGamePlayView();
            transitionCoroutine = null;
        }

        private void LerpMainCamera(float remainingTime)
        {
            float t = remainingTime / transitionTime;

            mainCamera.focalLength = Mathf.Lerp(mainCamera.focalLength, gamePlayFocalLength, t);
            transform.position = Vector3.Lerp(transform.position, gamePlayStartPosition, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, gamePlayRotation, t * transitionRotationMultiplier);
        }

        private bool CheckIfCameraIsInPosition()
        {
            if ( Mathf.Abs(mainCamera.focalLength - gamePlayFocalLength) < 0.05f
                && Vector3.Distance(gamePlayStartPosition, transform.position) < 0.05f)
            {
            return true;
            }
            else
            {
                return false;
            }
        }
    }
}
