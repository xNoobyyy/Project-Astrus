using System;
using System.Linq;
using Animals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player {
    [RequireComponent(typeof(Animator))]
    public class PlayerCombat : MonoBehaviour {
        private static readonly int Attacking = Animator.StringToHash("attacking");

        public float range;

        private Animator animator;

        private bool isAttacking;

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        private void Update() {
            if (!Input.GetMouseButtonDown(0) || isAttacking) return;

            var targets = Physics2D.OverlapCircleAll(transform.position, range)
                .Where(target => target.CompareTag("Enemy") || target.CompareTag("Animal"));

            var closestTarget = targets
                .OrderBy(target => Vector2.Distance(transform.position, target.transform.position))
                .FirstOrDefault();

            if (ReferenceEquals(closestTarget, null)) return;

            var target = closestTarget.transform;
            var animal = target.GetComponent<Animal>();

            animal.TakeDamage(1, transform.position);

            animator.SetBool(Attacking, true);
            isAttacking = true;
        }

        private void AttackFinished() {
            animator.SetBool(Attacking, false);
            isAttacking = false;
        }
    }
}