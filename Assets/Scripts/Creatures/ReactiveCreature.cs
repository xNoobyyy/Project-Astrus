using System.Collections;
using Logic.Events;
using TextDisplay;
using UnityEngine;

namespace Creatures {
    public class ReactiveCreature : CreatureBase {
        [SerializeField] protected GameObject angryTag;
        [SerializeField] private float chaseDuration = 10f;
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private int attackDamage = 5;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private float followDistance = 20f;
        [SerializeField] private ParticleSystem hearts;

        private Coroutine heartsCoroutine;
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
            MoveCoroutine = StartCoroutine(ChaseLoop());
        }

        protected override void Kill() {
            if (angryTag) angryTag.SetActive(false);
            base.Kill();
        }

        private IEnumerator ChaseLoop() {
            Animator.SetBool(Running, true);
            while (isChasing && chaseTarget != null && elapsedChaseTime < chaseDuration &&
                   Vector2.Distance(transform.position, chaseTarget.position) < followDistance) {
                Agent.SetDestination(chaseTarget.position);

                if (Agent.velocity.sqrMagnitude > 0.01f) SetAnimationDirection(Agent.velocity.normalized);

                if (Vector3.Distance(transform.position, chaseTarget.position) < attackRange) {
                    if (timeSinceLastAttack > attackCooldown &&
                        !TextDisplayManager.Instance.textDisplay.isDialogueActive) {
                        timeSinceLastAttack = 0;
                        EventManager.Instance.Trigger(new PlayerDamageEvent(attackDamage, transform));
                    } else {
                        timeSinceLastAttack += Time.deltaTime;
                    }
                }

                yield return null;
                elapsedChaseTime += Time.deltaTime;
            }

            Animator.SetBool(Running, false);
            isChasing = false;
            chaseTarget = null;
            angryTag.SetActive(false);
            Agent.ResetPath();
            StartIdle();
        }

        public override void OnTouch() {
            if (heartsCoroutine != null) StopCoroutine(heartsCoroutine);

            hearts.Play();
            EventManager.Instance.Trigger(new CreatureInteractEvent(this, InteractionType.Pet));

            heartsCoroutine = StartCoroutine(StopHearts());
        }

        private IEnumerator StopHearts() {
            yield return new WaitForSeconds(5f);
            hearts.Stop();
        }
    }
}