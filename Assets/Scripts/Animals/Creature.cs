using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logic;
using UnityEngine.AI; // for Pathfinding etc.
using Utils.WhiteFlash;
using Random = UnityEngine.Random;

namespace Animals {
    public interface IAttackable {
        void OnAttack(Transform attacker, float damage);
    }

    public abstract class CreatureBase : MonoBehaviour, IAttackable {
        // Animation parameter hash
        private static readonly int Direction = Animator.StringToHash("direction");

        // Common configuration fields
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float speed;
        [SerializeField] protected ParticleSystem damageParticles;
        [SerializeField] protected ParticleSystem deathParticles;
        [SerializeField] protected PolygonCollider2D area;

        // Health and state
        public float Health { get; protected set; }
        public bool Dead { get; protected set; }

        // Knockback constant
        private const float KNOCKBACK_FORCE = 15f;

        // References
        private Animator animator;
        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;

        // Coroutines
        private Coroutine idleCoroutine;
        protected Coroutine moveCoroutine;

        // Home/wandering logic
        private Vector2Int homePosition;
        private Vector2[] wanderPoints;

        // Idle time settings
        private const float IdleMin = 2f;
        private const float IdleMax = 10f;

        // Navigation
        protected NavMeshAgent agent;

        protected virtual void Awake() {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            Health = maxHealth;

            damageParticles.Stop();
            deathParticles.Stop();
        }

        protected virtual void Start() {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            homePosition = Vector2Int.RoundToInt(transform.position);
            wanderPoints = GetRandomWanderPointsFromArea();
            StartIdle();
        }

        // Starts idle/wander behavior
        protected void StartIdle() {
            if (idleCoroutine != null) return;
            StopExistingCoroutines();
            animator.SetInteger(Direction, 0);
            var idleTime = Random.Range(IdleMin, IdleMax);
            idleCoroutine = StartCoroutine(IdleThenWander(idleTime));
        }

        protected void StopExistingCoroutines() {
            if (idleCoroutine != null) {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }

            if (moveCoroutine != null) {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }

        private IEnumerator IdleThenWander(float time) {
            yield return new WaitForSeconds(time);
            idleCoroutine = null;
            var randomPoint = wanderPoints[Random.Range(0, wanderPoints.Length)];
            moveCoroutine = StartCoroutine(WanderTo(randomPoint, () => {
                moveCoroutine = null;
                StartIdle();
            }));
        }

        protected IEnumerator WanderTo(Vector2 destination, Action onComplete = null) {
            agent.SetDestination(destination);
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance) {
                if (agent.velocity.sqrMagnitude > 0.01f)
                    SetAnimationDirection(agent.velocity.normalized);
                yield return null;
            }

            animator.SetInteger(Direction, 0);
            onComplete?.Invoke();
        }

        private IEnumerator DestroyGameObject() {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        private Vector2[] GetRandomWanderPointsFromArea() {
            var points = new List<Vector2>();
            var bounds = area.bounds;
            var attempts = 0;
            while (points.Count < 10 && attempts < 100) {
                var randomPoint = new Vector2(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y)
                );
                if (area.OverlapPoint(randomPoint)) {
                    if (NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas)) {
                        points.Add(hit.position);
                    }
                }

                attempts++;
            }

            return points.ToArray();
        }

        // Helper to compute the animation direction from a movement vector
        private static int GetAnimDirection(Vector2 moveDirection) {
            var angle = -Vector2.SignedAngle(Vector2.up, moveDirection);
            if (angle < 0) angle += 360f;
            return angle switch { < 45f or >= 315f => 1, < 135f => 2, < 225f => 3, _ => 4 };
        }

        // Updates the animator parameter based on a movement direction
        protected void SetAnimationDirection(Vector2 moveDirection) {
            animator.SetInteger(Direction, GetAnimDirection(moveDirection));
        }

        // Processes common damage logic (subtract health, apply knockback, flash, damage particles).
        // Returns false if the creature dies.
        protected bool HandleDamage(Transform attacker, float damage) {
            Health -= damage;
            var knockbackDir = ((Vector2)transform.position - (Vector2)attacker.position).normalized;
            rb.AddForce(knockbackDir * KNOCKBACK_FORCE, ForceMode2D.Impulse);
            GetComponent<SpriteFlashEffect>().StartWhiteFlash();
            var rotation = Quaternion.FromToRotation(Vector2.right, knockbackDir);
            damageParticles.transform.rotation = rotation;
            damageParticles.Play();
            if (!(Health <= 0)) return true;

            Kill();
            return false;
        }

        public abstract void OnAttack(Transform attacker, float damage);

        protected virtual void Kill() {
            deathParticles.Play();
            spriteRenderer.enabled = false;
            Health = 0;
            Dead = true;
            StartCoroutine(DestroyGameObject());
        }
    }
}