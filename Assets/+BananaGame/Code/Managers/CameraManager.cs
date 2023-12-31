using BananaSoup.Effects;
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

        [SerializeField, Tooltip("Transition time from TitleScreen view to GamePlay view. (The transition is actually about 3/4 of the value)")]
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
        private Camera mainCamera = null;
        private GameStateManager gameStateManager = null;
        [SerializeField] private DepthOfFieldApplier depthOfFieldApplier = null;

        private const GameStateManager.GameState inGame = GameStateManager.GameState.Playing;

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

            gameStateManager = GameStateManager.Instance;
            if ( gameStateManager == null )
            {
                Debug.LogError($"{name} couldn't find an instance of GameStateManager!");
            }

            if ( depthOfFieldApplier == null )
            {
                Debug.LogError($"No component of {typeof(DepthOfFieldApplier)} can be found on the {name}!");
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

        public void ActivateGamePlayView()
        {
            mainCamera.focalLength = gamePlayFocalLength;
            transform.position = gamePlayStartPosition;
            transform.rotation = gamePlayRotation;

            playerFollower.enabled = true;
            gameStateManager.SetGameState(inGame);
        }

        private IEnumerator CameraTransitionCoroutine()
        {
            float remainingTransitionTime = 0f;
            float startTime = Time.time;

            while ( remainingTransitionTime < 1.0f )
            {
                if ( CheckIfCameraIsInPosition() )
                {
                    break;
                }

                float smoothStepT = Mathf.SmoothStep(0, 1, remainingTransitionTime);
                LerpMainCamera(smoothStepT);

                remainingTransitionTime = (Time.time - startTime) / transitionTime;

                yield return null;
            }

            ActivateGamePlayView();
            transitionCoroutine = null;
        }

        private void LerpMainCamera(float remainingTime)
        {
            float t = remainingTime / transitionTime;

            mainCamera.focalLength = Mathf.Lerp(mainCamera.focalLength, gamePlayFocalLength, t);
            transform.position = Vector3.Lerp(transform.position, gamePlayStartPosition, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, gamePlayRotation, t * transitionRotationMultiplier);

            // Lerping blur value also
            depthOfFieldApplier.SetBlur = Mathf.Lerp(depthOfFieldApplier.SetBlur, depthOfFieldApplier.GetGameplayBlur, t);
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
