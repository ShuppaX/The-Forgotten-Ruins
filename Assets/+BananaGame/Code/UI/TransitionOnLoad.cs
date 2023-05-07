using System.Collections;
using UnityEngine;

namespace BananaSoup.UI
{
    public class TransitionOnLoad : MonoBehaviour
    {
        [SerializeField, Tooltip("The time the transition should take.")]
        private float transitionTime = 5.0f;

        // Bool to check if fading is over.
        private bool fadingOver = false;

        // Coroutine to make sure only one of them is active at a time
        private Coroutine faderRoutine = null;

        // Reference to the CanvasGroup and Canvas on the fader GameObject.
        private CanvasGroup transitionFade;
        private Canvas transitionCanvas;

        public float TransitionTime => transitionTime;

        private void Start()
        {
            transitionFade = GetComponent<CanvasGroup>();
            if ( transitionFade == null )
            {
                Debug.LogError($"A component of type {typeof(CanvasGroup)} couldn't be found on {name}!");
            }

            transitionCanvas = GetComponent<Canvas>();
            if ( transitionCanvas == null )
            {
                Debug.LogError($"A component of type {typeof(Canvas)} couldn't be found on {name}!");
            }

            fadingOver = false;

            ToggleFaderCanvas();

            if ( faderRoutine == null )
            {
                faderRoutine = StartCoroutine(Fader(1f, 0f, true));
            }
        }

        public void FadeOut()
        {
            fadingOver = false;

            ToggleFaderCanvas();

            if ( faderRoutine == null )
            {
                faderRoutine = StartCoroutine(Fader(0f, 1f, false));
            }
        }

        private IEnumerator Fader(float startAlpha, float endAlpha, bool toggleCanvasAfter)
        {
            fadingOver = false;
            float elapsedTime = 0f;

            while ( elapsedTime < transitionTime )
            {
                elapsedTime += Time.deltaTime;

                float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, (elapsedTime / transitionTime));

                transitionFade.alpha = currentAlpha;

                yield return null;
            }

            fadingOver = true;
            
            if ( toggleCanvasAfter )
            {
                ToggleFaderCanvas();
            }

            faderRoutine = null;
        }

        private void ToggleFaderCanvas()
        {
            if ( fadingOver )
            {
                transitionCanvas.enabled = false;
            }
            else
            {
                transitionCanvas.enabled = true;
            }
        }
    }
}
