using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class SimpleObjectMover : MonoBehaviour
    {
        [SerializeField, Tooltip("How much this object moves (ping pong) from it's original position")]
        private Vector3 moveValue = new Vector3(0.0f, 0.25f, 0.0f);
        [SerializeField] private float duration = 2.0f;
        private Vector3 originalPosition;
        private Vector3 positionToMove;
        private float elapsedTime;

        private void Start()
        {
            originalPosition = transform.position;
            positionToMove = transform.position + moveValue;
        }

        private void Update()
        {
            UpdatePosition();
        }

        /// <summary>
        /// Calculates and updates object's position. Ping Ponging between starting position and 
        /// given value (starting position + moveValue). Smoothing step so it doesn't look so janky.
        /// </summary>
        private void UpdatePosition()
        {
            elapsedTime += Time.deltaTime;
            float step = elapsedTime / duration;
            float pingPongedStep = Mathf.PingPong(step, 1);
            float smoothedStep = Mathf.SmoothStep(0, 1, pingPongedStep);
            transform.position = Vector3.Lerp(originalPosition, positionToMove, smoothedStep);
        }
    }
}
