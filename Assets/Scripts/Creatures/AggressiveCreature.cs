using System.Linq;
using Logic.Events;
using UnityEngine;

namespace Creatures {
    public class AggressiveCreature : CreatureBase {
        private Transform chaseTarget;
        private bool isChasing;

        [SerializeField] private float attackRange = 1f;
        [SerializeField] private int attackDamage = 5;
        [SerializeField] private float attackCooldown = 1f;
        private float timeSinceLastAttack;

        private void OnEnable() {
            EventManager.Instance.Subscribe<PlayerMoveEvent>(HandlePlayerMove);
        }

        private void OnDisable() {
            EventManager.Instance.Unsubscribe<PlayerMoveEvent>(HandlePlayerMove);
        }

        private void HandlePlayerMove(PlayerMoveEvent e) {
            if (Vector2Int.RoundToInt(e.From) == Vector2Int.RoundToInt(e.To)) return;

            if (isChasing) {
                if (Vector3.Distance(transform.position, chaseTarget.position) < 20f) return;

                isChasing = false;
                chaseTarget = null;
                animator.SetBool(Running, false);
                agent.ResetPath();
                StartIdle();
            } else {
                if (!IsLos(e.Transform.position)) return;

                chaseTarget = e.Transform;
                isChasing = true;
                StopExistingCoroutines();
                animator.SetBool(Running, true);
            }
        }

        public override void OnAttack(Transform attacker, float damage) {
            if (!HandleDamage(attacker, damage))
                return;
            chaseTarget = attacker;
            isChasing = true;
            StopExistingCoroutines();
        }

        private void Update() {
            if (!isChasing || chaseTarget == null) return;

            agent.SetDestination(chaseTarget.position);

            if (agent.velocity.sqrMagnitude > 0.01f) SetAnimationDirection(agent.velocity.normalized);

            if (Vector3.Distance(transform.position, chaseTarget.position) < attackRange) {
                if (timeSinceLastAttack > attackCooldown) {
                    timeSinceLastAttack = 0;
                    EventManager.Instance.Trigger(new PlayerDamageEvent(attackDamage, transform));
                } else {
                    timeSinceLastAttack += Time.deltaTime;
                }
            }
        }

        private bool IsLos(Vector2 v) => Vector2.Distance(v, transform.position) < 20f && Physics2D.RaycastAll(
            transform.position, v - (Vector2)transform.position,
            Vector2.Distance(transform.position, v)).All(c => !c.transform.CompareTag("Obstacle"));
    }
}