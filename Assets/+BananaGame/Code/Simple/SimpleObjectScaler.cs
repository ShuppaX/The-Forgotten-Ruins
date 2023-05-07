using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class SimpleObjectScaler : MonoBehaviour
    {
        [SerializeField] private Vector3 endScales = new Vector3(1.2f, 1.2f, 1.2f);
        [SerializeField] private float duration = 5.0f;
        [SerializeField] private bool scaleOnStart = true;
        private bool allowScaling;
        private float elapsedTime;
        private Vector3 startScales;

        private void Start()
        {
            startScales = transform.localScale;

            if ( scaleOnStart )
            {
                allowScaling = true;
            }
        }

        private void FixedUpdate()
        {
            if ( allowScaling )
            {
                UpdateScale();
            }
        }

        private void UpdateScale()
        {
            elapsedTime += Time.deltaTime;
            float step = elapsedTime / duration;
            float smoothedStep = Mathf.SmoothStep(0, 1, step);
            transform.localScale = Vector3.Lerp(startScales, endScales, smoothedStep);

            if ( transform.localScale == endScales )
            {
                allowScaling = false;
            }
        }
    }
}
