using System;
using Logic;
using Logic.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Controls;
using Utils;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerMovement : MonoBehaviour {
        public static PlayerMovement Instance { get; private set; }

        private static readonly int HorizontalMove = Animator.StringToHash("horizontal_move");
        private static readonly int VerticalMove = Animator.StringToHash("vertical_move");
        private Animator animator;
        private PlayerItem playerItem;

        [SerializeField] public float speed = 3f;

        private Vector2 movement;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
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

            if (PlayerItem.Instance.InBoat || (Mathf.RoundToInt(from.x) == Mathf.RoundToInt(to.x) &&
                                               Mathf.RoundToInt(from.y) == Mathf.RoundToInt(to.y))) return;

            SoundEffect soundEffect;
            switch (AreaManager.Instance.LastOrCurrentArea.type) {
                case AreaType.Cave:
                    soundEffect = SoundEffect.WalkingCave;
                    break;
                case AreaType.Beach:
                    soundEffect = SoundEffect.WalkingSand;
                    break;
                case AreaType.City:
                    soundEffect = SoundEffect.WalkingCity;
                    break;
                default:
                    soundEffect = SoundEffect.WalkingGrass;
                    break;
            }

            SoundManager.Instance.PlaySound(soundEffect, 0.05f);
        }
    }
}