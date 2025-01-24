using System;
using Animals;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerMovement : MonoBehaviour {
        private static readonly int HorizontalMove = Animator.StringToHash("horizontal_move");
        private static readonly int VerticalMove = Animator.StringToHash("vertical_move");
        private Rigidbody2D rb;
        private Animator animator;

        [SerializeField] private float speed = 1.5f;

        private Vector2 movement;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Update() {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            movement = movement.normalized;

            animator.SetFloat(HorizontalMove, movement.x);
            animator.SetFloat(VerticalMove, movement.y);
        }

        private void FixedUpdate() {
            rb.linearVelocity = movement * speed;
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Enemy")) return;

            var animal = other.GetComponent<Animal>();

            if (animal != null) {
                animal.SetTarget(transform);
            }
        }
    }
}