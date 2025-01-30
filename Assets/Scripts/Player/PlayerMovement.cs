using System;
using Animals;
using Logic;
using UnityEngine;
using UnityEngine.Events;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerMovement : MonoBehaviour {
        private static readonly int HorizontalMove = Animator.StringToHash("horizontal_move");
        private static readonly int VerticalMove = Animator.StringToHash("vertical_move");
        private Rigidbody2D rb;
        private Animator animator;
        private PlayerCombat playerCombat;
        private EventManager eventManager;

        [SerializeField] private float speed = 1.5f;

        private Vector2 movement;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            playerCombat = GetComponent<PlayerCombat>();
            eventManager = FindFirstObjectByType<EventManager>();
        }

        private void Update() {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            movement = movement.normalized;

            if (playerCombat.IsAttacking) movement = Vector2.zero;

            animator.SetFloat(HorizontalMove, movement.x);
            animator.SetFloat(VerticalMove, movement.y);
        }

        private void FixedUpdate() {
            if (movement == Vector2.zero) return;

            var from = transform.position;
            var add = new Vector2(movement.x, movement.y) * (speed * Time.fixedDeltaTime);
            transform.position += new Vector3(add.x, add.y, 0f);
            var to = transform.position;
            eventManager?.InvokePlayerMove(from, to);
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