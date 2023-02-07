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

        private PlayerInput playerInput;
        private InputAction move;

        private Rigidbody rb;
        private Vector3 movement = Vector3.zero;

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
            move = playerInput.Player.Move;
            playerInput.Player.Enable();
        }

        private void OnDisable()
        {
            playerInput.Player.Disable();
        }

        private void FixedUpdate()
        {
            Move(Time.fixedDeltaTime);
        }

        // Used mainly for debugging at the moment
        //private void Update()
        //{
        //    Debug.Log("The value of 'move' is: " + move.ReadValue<Vector2>());
        //    Debug.Log("The value of 'movement' is: " + movement);
        //}

        private void Move(float deltaTime)
        {
            movement = new Vector3((move.ReadValue<Vector2>().x * movementSpeed * deltaTime), rb.velocity.y, (move.ReadValue<Vector2>().y * movementSpeed * deltaTime));
            rb.MovePosition(transform.position + movement);
        }
    }
}
