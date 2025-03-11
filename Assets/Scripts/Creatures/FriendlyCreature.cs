using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Creatures {
    public class FriendlyCreature : CreatureBase {
        [SerializeField] private float runAwayDistance = 10f;
        [SerializeField] private ParticleSystem hearts;

        private Coroutine heartsCoroutine;

        public override void OnAttack(Transform attacker, float damage) {
            if (!HandleDamage(attacker, damage))
                return;

            if (heartsCoroutine != null) StopCoroutine(heartsCoroutine);

            StopExistingCoroutines();
            var destination = GetSafeRunAwayDestination(attacker.position);
            moveCoroutine = StartCoroutine(WanderTo(destination, () => {
                moveCoroutine = null;
                StartIdle();
            }));
        }

        public override void OnTouch() {
            if (heartsCoroutine != null) StopCoroutine(heartsCoroutine);
            hearts.Play();
            heartsCoroutine = StartCoroutine(StopHearts());
        }

        private IEnumerator StopHearts() {
            yield return new WaitForSeconds(5f);
            hearts.Stop();
            heartsCoroutine = null;
        }

        // Computes a safe destination away from the attacker while checking for obstacles.
        private Vector2 GetSafeRunAwayDestination(Vector2 attackerPosition) {
            var awayDir = ((Vector2)transform.position - attackerPosition).normalized;
            var candidate = (Vector2)transform.position + awayDir * runAwayDistance;
            if (NavMesh.SamplePosition(candidate, out var hit, 1.0f, NavMesh.AllAreas))
                candidate = hit.position;
            return candidate;
        }
    }
}