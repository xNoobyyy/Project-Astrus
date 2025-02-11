using UnityEngine;
using System.Collections.Generic;
using Animals;

namespace Player {
    [RequireComponent(typeof(Animator))]
    public class PlayerCombat : MonoBehaviour {
        private static readonly int AttackDirection = Animator.StringToHash("attack_direction");

        [SerializeField] private Collider2D attackCollider;

        private Camera mainCamera;
        private Animator animator;
        private Rigidbody2D rb;

        public bool IsAttacking { get; private set; }
        private Vector2 attackDirection;

        private void Awake() {
            mainCamera = Camera.main;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        // TODO: Only when sword is equipped
        private void Update() {
            if (!Input.GetMouseButtonDown(0) || IsAttacking) return;

            var zCoord = mainCamera.WorldToScreenPoint(transform.position).z;
            var mousePosition = Input.mousePosition;
            mousePosition.z = zCoord;

            var pos = mainCamera.ScreenToWorldPoint(mousePosition);

            var v = (pos - transform.position).normalized;
            attackDirection = v;
            var angle = -Vector2.SignedAngle(Vector2.up, v);
            if (angle < 0) angle += 360f;

            // Set the attack direction for the animator
            var direction = angle switch { >= 315f or < 45f => 1, < 135f => 2, < 225f => 3, _ => 4 };
            animator.SetInteger(AttackDirection, direction);
            IsAttacking = true;

            // Rotate the attack collider
            var colliderRotation = -((angle + 180f) % 360f);
            attackCollider.transform.localRotation = Quaternion.Euler(0f, 0f, colliderRotation);
        }

        private void AttackFinished() {
            animator.SetInteger(AttackDirection, 0);
            IsAttacking = false;
            attackDirection = Vector2.zero;
        }

        private void Attack() {
            rb.AddForce(-attackDirection.normalized * 3f, ForceMode2D.Impulse);

            var colliders = new List<Collider2D>();
            Physics2D.OverlapCollider(attackCollider, new ContactFilter2D().NoFilter(), colliders);
            colliders.ForEach(c => c.GetComponent<IAttackable>()?.OnAttack(transform, 1));
        }
    }
}