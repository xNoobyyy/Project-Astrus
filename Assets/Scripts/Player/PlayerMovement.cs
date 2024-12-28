using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour {
        private Rigidbody2D rb;

        [SerializeField] private float speed = 1.5f;

        private Vector2 movement;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            movement = movement.normalized;
        }

        private void FixedUpdate() {
            rb.linearVelocity = movement * speed;
        }
    }
}