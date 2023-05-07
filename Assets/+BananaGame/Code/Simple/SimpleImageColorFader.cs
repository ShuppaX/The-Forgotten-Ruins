using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BananaSoup
{
    public class SimpleImageColorFader : MonoBehaviour
    {
        [SerializeField] private Color targetColor;
        [SerializeField] float duration = 2.0f;
        [SerializeField] bool setAlphaToZeroAtAwake = true;
        private Image image;
        private Color startingColor;
        private float elapsedTime;
        private bool allowColorChange = true;

        private void Awake()
        {
            image = GetComponent<Image>();
            if ( image == null )
            {
                Debug.LogError($"{name} is missing a reference to a Image!");
            }

            if ( setAlphaToZeroAtAwake )
            {
                var tempColor = image.color;
                tempColor.a = 0.0f;
                image.color = tempColor;
            }

            startingColor = image.color;
        }

        private void FixedUpdate()
        {
            UpdateColor();
        }

        private void UpdateColor()
        {

            if ( allowColorChange )
            {
                elapsedTime += Time.deltaTime;
                float step = elapsedTime / duration;
                float smoothedStep = Mathf.SmoothStep(0, 1, step);
                image.color = Color.Lerp(startingColor, targetColor, smoothedStep);

                if ( image.color == targetColor )
                {
                    allowColorChange = false;
                }
            }
        }
    }
}
