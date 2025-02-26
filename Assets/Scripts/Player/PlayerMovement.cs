using System;
using Logic;
using Logic.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerMovement : MonoBehaviour {
        public static PlayerMovement Instance { get; private set; }

        private static readonly int HorizontalMove = Animator.StringToHash("horizontal_move");
        private static readonly int VerticalMove = Animator.StringToHash("vertical_move");
        private Animator animator;
        private PlayerItem playerItem;

        [SerializeField] private float speed = 1.5f;

        private Vector2 movement;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
                return;
            }
        }

        private void Start() {  
            animator = GetComponent<Animator>();
            playerItem = GetComponent<PlayerItem>();

            EventManager.Instance.Trigger(new PlayerMoveEvent(transform.position, transform.position, transform));
        }

        private void Update() {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            movement = movement.normalized;

            if (playerItem.IsAttacking) movement = Vector2.zero;

            animator.SetFloat(HorizontalMove, movement.x);
            animator.SetFloat(VerticalMove, movement.y);
        }

        private void FixedUpdate() {
            if (movement == Vector2.zero) return;

            var from = transform.position;
            var add = new Vector2(movement.x, movement.y) * (speed * Time.fixedDeltaTime);
            transform.position += new Vector3(add.x, add.y, 0f);
            var to = transform.position;
            EventManager.Instance.Trigger(new PlayerMoveEvent(from, to, transform));
        }
    }
}