using System.Collections;
using UnityEngine;

namespace Animals {
    public class ReactiveCreature : CreatureBase {
        [SerializeField] private float chaseDuration = 10f;
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

        private IEnumerator ChaseLoop() {
            while (isChasing && chaseTarget != null && elapsedChaseTime < chaseDuration) {
                agent.SetDestination(chaseTarget.position);
                if (agent.velocity.sqrMagnitude > 0.01f)
                    SetAnimationDirection(agent.velocity.normalized);
                yield return null;
                elapsedChaseTime += Time.deltaTime;
            }

            isChasing = false;
            chaseTarget = null;
            angryTag.SetActive(false);
            StartIdle();
        }
    }
}