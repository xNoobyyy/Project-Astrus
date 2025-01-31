using System;
using System.Linq;
using Animals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Player {
    [RequireComponent(typeof(Animator))]
    public class PlayerCombat : MonoBehaviour {
        private static readonly int AttackDirection = Animator.StringToHash("attack_direction");

        public float range;

        private Rigidbody2D rb;
        private Animator animator;

        private Animal attacking;
        public bool IsAttacking { get; private set; }

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Update() {
            if (!Input.GetMouseButtonDown(0) || IsAttacking) return;

            var targets = Physics2D.OverlapCircleAll(transform.position, range)
                .Where(target => target.CompareTag("Enemy") || target.CompareTag("Animal"));

            var closestTarget = targets
                .OrderBy(target => Vector2.Distance(transform.position, target.transform.position))
                .FirstOrDefault();

            if (ReferenceEquals(closestTarget, null)) return;

            var target = closestTarget.transform;
            var animal = target.GetComponent<Animal>();

            var v = target.position - transform.position;
            var angle = -Vector2.SignedAngle(Vector2.up, v);
            if (angle < 0) angle += 360f;

            var direction = angle switch { >= 315f or < 45f => 1, < 135f => 2, < 225f => 3, _ => 4 };
            animator.SetInteger(AttackDirection, direction);
            IsAttacking = true;
            attacking = animal;
        }

        private void AttackFinished() {
            animator.SetInteger(AttackDirection, 0);
            IsAttacking = false;
            attacking = null;
        }

        private void Attack() {
            if (attacking == null) return;
            attacking.TakeDamage(1, transform.position, transform);
            var v = attacking.transform.position - transform.position;
            rb.AddForce(-v.normalized * 3f, ForceMode2D.Impulse);
        }
    }
}