using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaSoup
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Adjustable variables")]
        [SerializeField] private float movementSpeed = 5.0f;
        [SerializeField] private float turnSpeed = 360.0f;

        private PlayerInput playerInput;
        private InputAction moveInput;

        private Rigidbody rb;
        private Vector3 movementInput = Vector3.zero;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            rb = GetComponent<Rigidbody>();
            playerInput = new PlayerInput();

            if (rb == null)
            {
                Debug.LogError("A Rigidbody couldn't be found on the " + gameObject + "!");
            }
        }

        private void OnEnable()
        {
            moveInput = playerInput.Player.Move;
            playerInput.Player.Enable();
        }

        private void OnDisable()
        {
            playerInput.Player.Disable();
        }

        private void FixedUpdate()
        {
            Move(Time.fixedDeltaTime);
            Look(Time.fixedDeltaTime);
        }

        // Store the moveInput values to the movementInput Vector3
        private void Update()
        {
            movementInput.Set(moveInput.ReadValue<Vector2>().x, rb.velocity.y, moveInput.ReadValue<Vector2>().y);
        }

        private void Move(float deltaTime)
        {
            Vector3 movement = (transform.forward * movementInput.magnitude) * movementSpeed * deltaTime;
            rb.MovePosition(transform.position + movement);
        }

        private void Look(float deltaTime)
        {
            if (movementInput != Vector3.zero)
            {
                var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

                var skewedInput = matrix.MultiplyPoint3x4(movementInput);

                var relative = (transform.position + skewedInput) - transform.position;
                var rot = Quaternion.LookRotation(relative, Vector3.up);

                // Use this for smooth turning
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * deltaTime);

                // Use this for instant turning
                //transform.rotation = rot;
            }
        }
    }
}
