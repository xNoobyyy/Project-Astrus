using UnityEngine;

namespace Animals {
    public class AggressiveCreature : CreatureBase {
        private Transform chaseTarget;
        private bool isChasing;
        
        public override void OnAttack(Transform attacker, float damage) {
            if (!HandleDamage(attacker, damage))
                return;
            chaseTarget = attacker;
            isChasing = true;
            StopExistingCoroutines();
            angryTag.SetActive(true);
        }

        private void Update() {
            if (!isChasing || chaseTarget == null)
                return;
            agent.SetDestination(chaseTarget.position);
            if (agent.velocity.sqrMagnitude > 0.01f)
                SetAnimationDirection(agent.velocity.normalized);
        }
    }
}