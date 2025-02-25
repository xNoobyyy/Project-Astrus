using UnityEngine;
using UnityEngine.AI;

namespace Creatures {
    public class FriendlyCreature : CreatureBase {
        [SerializeField] private float runAwayDistance = 5f;

        public override void OnAttack(Transform attacker, float damage) {
            if (!HandleDamage(attacker, damage))
                return;
            StopExistingCoroutines();
            var destination = GetSafeRunAwayDestination(attacker.position);
            moveCoroutine = StartCoroutine(WanderTo(destination, () => {
                moveCoroutine = null;
                StartIdle();
            }));
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