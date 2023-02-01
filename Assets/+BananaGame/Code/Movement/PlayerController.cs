using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 5.0f;

        private Rigidbody rb;
        private Vector2 move = Vector3.zero;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            rb = GetComponent<Rigidbody>();

            if (rb == null)
            {
                Debug.LogError("A Rigidbody2D couldn't be found on " + gameObject + "!");
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            move = context.ReadValue<Vector2>();
        }

        public void Move()
        {
            Vector3 movement = new Vector3(move.x, 0, move.y);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);

            transform.Translate(movement * movementSpeed * Time.fixedDeltaTime, Space.World);
        }
    }
}
