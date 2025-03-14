using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Logic.Events;
using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Utils;
using Utils.WhiteFlash;
// for Pathfinding etc.
using Random = UnityEngine.Random;

namespace Creatures {
    public interface IAttackable {
        void OnAttack(Transform attacker, float damage);
    }

    public enum CreatureType {
        Dodo,
        Quokka,
        Zombie,
        ZombieBoss,
        Golem,
        Capybara,
    }

    public abstract class CreatureBase : MonoBehaviour, IAttackable {
        // Animation parameter hash
        private static readonly int Direction = Animator.StringToHash("direction");
        protected static readonly int Running = Animator.StringToHash("running");

        // Common configuration fields
        [SerializeField] public CreatureType type;
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float speed;
        [SerializeField] protected ParticleSystem damageParticles;
        [SerializeField] protected ParticleSystem deathParticles;
        [SerializeField] protected ParticleSystem fireParticles;
        [SerializeField] private GameObject[] destroyOnDeath;

        // Health and state
        public float Health { get; protected set; }
        public bool Dead { get; protected set; }

        // Knockback constant
        private const float KnockbackForce = 15f;

        // References
        protected Animator Animator;
        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;
        private new Collider2D collider;

        // Coroutines
        private Coroutine idleCoroutine;
        protected Coroutine MoveCoroutine;

        // Home/wandering logic
        private Vector2Int homePosition;
        private Vector2[] wanderPoints;

        // Idle time settings
        private const float IdleMin = 2f;
        private const float IdleMax = 10f;

        // Navigation
        protected NavMeshAgent Agent;

        private float fireTicks;
        private Coroutine fireCoroutine;

        protected virtual void Awake() {
            Animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();
            Health = maxHealth;

            damageParticles.Stop();
            deathParticles.Stop();
        }

        protected virtual void Start() {
            Agent = GetComponent<NavMeshAgent>();
            Agent.updateRotation = false;
            Agent.updateUpAxis = false;

            homePosition = Vector2Int.RoundToInt(transform.position);
            wanderPoints = GetRandomWanderPointsFromArea();

            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        protected void OnEnable() {
            StartIdle();
        }

        protected void OnDisable() {
            StopExistingCoroutines();
        }

        // Starts idle/wander behavior
        protected void StartIdle() {
            if (idleCoroutine != null) return;
            StopExistingCoroutines();
            Animator.SetBool(Running, false);
            var idleTime = Random.Range(IdleMin, IdleMax);
            idleCoroutine = StartCoroutine(IdleThenWander(idleTime));
        }

        protected void StopExistingCoroutines() {
            if (idleCoroutine != null) {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }

            if (MoveCoroutine == null) return;

            StopCoroutine(MoveCoroutine);
            MoveCoroutine = null;
        }

        private IEnumerator IdleThenWander(float time) {
            yield return new WaitForSeconds(time);
            idleCoroutine = null;
            var randomPoint = wanderPoints[Random.Range(0, wanderPoints.Length)];
            MoveCoroutine = StartCoroutine(WanderTo(randomPoint, () => {
                MoveCoroutine = null;
                StartIdle();
            }));
        }

        protected IEnumerator WanderTo(Vector2 destination, Action onComplete = null) {
            Animator.SetBool(Running, true);
            Agent.SetDestination(destination);
            while (Agent.pathPending || Agent.remainingDistance > Agent.stoppingDistance) {
                if (Agent.velocity.sqrMagnitude > 0.01f)
                    SetAnimationDirection(Agent.velocity.normalized);
                yield return null;
            }

            Animator.SetBool(Running, false);
            onComplete?.Invoke();
        }

        private IEnumerator DestroyGameObject() {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        private Vector2[] GetRandomWanderPointsFromArea() {
            var points = new List<Vector2>();
            var attempts = 0;

            while (points.Count < 10 && attempts < 1000) {
                var randomPoint = homePosition + new Vector2(
                    Random.Range(-50, 50),
                    Random.Range(-50, 50)
                );
                if (NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas)) {
                    points.Add(hit.position);
                }

                attempts++;
            }

            if (points.Count == 0) Debug.LogError($"Failed to find any valid wander points for {name}");

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
            Animator.SetInteger(Direction, GetAnimDirection(moveDirection));
        }

        // Processes common damage logic (subtract health, apply knockback, flash, damage particles).
        // Returns false if the creature dies.
        protected bool HandleDamage(Transform attacker, float damage) {
            Health -= damage;
            if (attacker != null) {
                var knockbackDir = ((Vector2)transform.position - (Vector2)attacker.position).normalized;
                rb.AddForce(knockbackDir * KnockbackForce, ForceMode2D.Impulse);
                var rotation = Quaternion.FromToRotation(Vector2.right, knockbackDir);
                damageParticles.transform.rotation = rotation;
                damageParticles.Play();
            }

            if (type == CreatureType.Zombie) {
                SoundManager.Instance.PlaySound(SoundEffect.ZombieHit);
            } else if (type == CreatureType.Golem) {
                SoundManager.Instance.PlaySound(SoundEffect.Help);
            } else if (type == CreatureType.Quokka) {
                SoundManager.Instance.PlaySound(SoundEffect.Quokka);
            }

            GetComponent<SpriteFlashEffect>().StartWhiteFlash();
            EventManager.Instance.Trigger(new CreatureInteractEvent(this, InteractionType.Attack));
            if (!(Health <= 0)) return true;

            Kill();
            return false;
        }

        public void SetOnFire(float duration = 5f) {
            if (fireCoroutine != null) {
                StopCoroutine(fireCoroutine);
            }

            fireCoroutine = StartCoroutine(FireDamageTicks(duration));
        }

        private IEnumerator FireDamageTicks(float duration) {
            fireParticles.Play();
            while (fireTicks < duration) {
                fireTicks++;
                HandleDamage(null, 1f);
                yield return new WaitForSeconds(1f);
            }

            fireParticles.Stop();
            fireTicks = 0f;
            fireCoroutine = null;
        }

        public abstract void OnAttack(Transform attacker, float damage);

        public virtual void Kill() {
            if (type == CreatureType.Zombie) {
                SoundManager.Instance.PlaySound(SoundEffect.ZombieDeath);
            }

            deathParticles.Play();
            spriteRenderer.enabled = false;
            Health = 0;
            Dead = true;
            EventManager.Instance.Trigger(new CreatureDeathEvent(this));
            foreach (var obj in destroyOnDeath) {
                Destroy(obj);
            }

            collider.enabled = false;

            StartCoroutine(DestroyGameObject());
        }

        public abstract void OnTouch();

        protected bool IsLos(Vector2 v) => Vector2.Distance(v, transform.position) < 20f && Physics2D.RaycastAll(
            transform.position, v - (Vector2)transform.position,
            Vector2.Distance(transform.position, v)).All(c => !c.transform.CompareTag("Obstacle"));
    }
}