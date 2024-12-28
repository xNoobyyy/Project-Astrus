using UnityEngine;

namespace Player {
    public class PlayerMovement : MonoBehaviour {
        private CharacterController controller;

        [SerializeField] private float speed = 1.5f;

        private Vector3 movement;

        private void Start() {
            controller = GetComponent<CharacterController>();
            if (controller != null) return;
            Debug.LogError("CharacterController not found on Player");
            enabled = false;
        }

        private void Update() {
            var horizontalMove = Input.GetAxisRaw("Horizontal");
            var verticalMove = Input.GetAxisRaw("Vertical");

            movement = new Vector3(horizontalMove, verticalMove, 0);

            if (movement.magnitude > 1) {
                movement = movement.normalized;
            }

            movement *= speed;
            controller.Move(movement * Time.deltaTime);
        }
    }
}