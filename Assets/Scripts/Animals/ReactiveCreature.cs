using System.Collections;
using Logic.Events;
using UnityEngine;

namespace Animals {
    public class ReactiveCreature : CreatureBase {
        [SerializeField] protected GameObject angryTag;
        [SerializeField] private float chaseDuration = 10f;
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private int attackDamage = 5;
        [SerializeField] private float attackCooldown = 1f;
        private float timeSinceLastAttack;

        private Transform chaseTarget;
        private bool isChasing;
        private float elapsedChaseTime;
        private Vector2Int lastKnownTile;

        public override void OnAttack(Transform attacker, float damage) {
            if (!HandleDamage(attacker, damage))
                return;
            chaseTarget = attacker;
            isChasing = true;
            elapsedChaseTime = 0f;
            StopExistingCoroutines();
            angryTag.SetActive(true);
            moveCoroutine = StartCoroutine(ChaseLoop());
        }

        protected override void Kill() {
            base.Kill();
            angryTag.SetActive(false);
        }

        private IEnumerator ChaseLoop() {
            animator.SetBool(Running, true);
            while (isChasing && chaseTarget != null && elapsedChaseTime < chaseDuration) {
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

                yield return null;
                elapsedChaseTime += Time.deltaTime;
            }

            animator.SetBool(Running, false);
            isChasing = false;
            chaseTarget = null;
            angryTag.SetActive(false);
            StartIdle();
        }
    }
}