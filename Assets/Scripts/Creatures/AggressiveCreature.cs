using Logic.Events;
using Player;
using TextDisplay;
using UnityEngine;
using Utils;

namespace Creatures {
    public class AggressiveCreature : CreatureBase {
        private static readonly int Spawned = Animator.StringToHash("spawned");
        private Transform chaseTarget;
        private bool isChasing;

        [SerializeField] private float attackRange = 1f;
        [SerializeField] private int attackDamage = 5;
        [SerializeField] private float attackCooldown = 1f;
        private float timeSinceLastAttack;

        private new void OnEnable() {
            EventManager.Instance.Subscribe<PlayerMoveEvent>(HandlePlayerMove);
        }

        private new void OnDisable() {
            base.OnDisable();
            EventManager.Instance.Unsubscribe<PlayerMoveEvent>(HandlePlayerMove);
        }

        public void Init() {
            base.OnEnable();
            Animator.SetBool(Spawned, true);
        }

        private void HandlePlayerMove(PlayerMoveEvent e) {
            if (Vector2Int.RoundToInt(e.From) == Vector2Int.RoundToInt(e.To)) return;

            if (isChasing) {
                Agent.SetDestination(e.To);
                if (Vector3.Distance(transform.position, chaseTarget.position) < 15f &&
                    !PlayerItem.Instance.Invisible) return;

                isChasing = false;
                chaseTarget = null;
                Animator.SetBool(Running, false);
                Agent.ResetPath();
                StartIdle();
            } else {
                if (!IsLos(e.Transform.position) || PlayerItem.Instance.Invisible ||
                    Vector3.Distance(transform.position, chaseTarget.position) >= 15f) return;

                chaseTarget = e.Transform;
                isChasing = true;
                StopExistingCoroutines();
                Animator.SetBool(Running, true);
            }
        }

        public override void OnAttack(Transform attacker, float damage) {
            if (!HandleDamage(attacker, damage))
                return;

            chaseTarget = attacker;
            isChasing = true;
            StopExistingCoroutines();
        }

        public override void OnTouch() { }

        private void Update() {
            if (!isChasing || chaseTarget == null) return;
            if (PlayerItem.Instance.Invisible) {
                isChasing = false;
                chaseTarget = null;
                Animator.SetBool(Running, false);
                Agent.ResetPath();
                StartIdle();
                return;
            }

            Agent.SetDestination(chaseTarget.position);

            if (Agent.velocity.sqrMagnitude > 0.01f) SetAnimationDirection(Agent.velocity.normalized);

            if (Vector3.Distance(transform.position, chaseTarget.position) < attackRange) {
                if (timeSinceLastAttack > attackCooldown && !TextDisplayManager.Instance.textDisplay.isDialogueActive) {
                    timeSinceLastAttack = 0;
                    EventManager.Instance.Trigger(new PlayerDamageEvent(attackDamage, transform));
                } else {
                    timeSinceLastAttack += Time.deltaTime;
                }
            }
        }
    }
}