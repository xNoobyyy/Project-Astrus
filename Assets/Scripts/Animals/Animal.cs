using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animals {
    [RequireComponent(typeof(Animator))]
    public class Animal : MonoBehaviour {
        // Animation parameter hashes
        private static readonly int Direction = Animator.StringToHash("direction");

        // Public config
        public float maxHealth;
        public float speed;
        public float loseInterestRange = 10f; // Distance at which we give up chasing
        public PolygonCollider2D area;
        public Transform player;

        // Public read-only property for health
        public float Health { get; private set; }

        // Home/wandering logic
        private Vector2 homePosition;

        // References
        private Animator animator;
        private Rigidbody2D rb;

        // Internal coroutines
        private Coroutine idleCoroutine;
        private Coroutine moveCoroutine;

        // The transform (e.g. player) we might want to follow
        private Transform chaseTarget;
        private bool isChasing;

        private void Awake() {
            Debug.Log("Animal Awake: Initializing...");
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            Health = maxHealth;
        }

        private void Start() {
            Debug.Log("Animal Start: Setting home position and starting idle mode.");
            homePosition = transform.position;
            StartIdle();
        }

        /// <summary>
        /// Called to make the animal try to chase a certain target (e.g. player).
        /// </summary>
        /// <param name="target">The transform to chase.</param>
        public void SetTarget(Transform target) {
            Debug.Log($"SetTarget called with target: {target}");
            chaseTarget = target;
            isChasing = true;

            StopExistingCoroutines();

            moveCoroutine = StartCoroutine(ChaseLoop());
        }

        /// <summary>
        /// Explicitly stop any chasing behavior.
        /// </summary>
        public void StopChasing() {
            Debug.Log("StopChasing called.");
            isChasing = false;
            chaseTarget = null;

            if (moveCoroutine != null) {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            StartIdle();
        }

        /// <summary>
        /// Idle for a few seconds, then pick a random wander point.
        /// </summary>
        public void StartIdle() {
            Debug.Log("StartIdle called.");
            if (idleCoroutine != null) return;

            StopExistingCoroutines();

            animator.SetInteger(Direction, 0);

            var idleTime = Random.Range(3f, 10f);
            Debug.Log($"Idling for {idleTime} seconds.");
            idleCoroutine = StartCoroutine(IdleThenWander(idleTime));
        }

        private void StopExistingCoroutines() {
            if (idleCoroutine != null) {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }

            if (moveCoroutine != null) {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }

        // =============================================
        // INTERNAL COROUTINES & LOGIC
        // =============================================

        private IEnumerator IdleThenWander(float time) {
            Debug.Log("IdleThenWander started.");
            yield return new WaitForSeconds(time);

            idleCoroutine = null;

            var randomPoint = GetRandomLosPointFromHome();
            Debug.Log($"Wandering to random point: {randomPoint}");

            moveCoroutine = StartCoroutine(WanderTo(randomPoint, () => {
                Debug.Log("Wander complete.");
                moveCoroutine = null;
                StartIdle();
            }));
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.F)) {
                SetTarget(player);
            }
        }

        public void TakeDamage(float damage, Vector2 from) {
            //Health -= damage;
            Debug.Log($"Took {damage} damage. Health: {Health}");


            // Knockback
            var direction = (transform.position - (Vector3)from).normalized;
            rb.AddForce(direction * 5f, ForceMode2D.Impulse);

            /*if (Health <= 0) {
                Die();
            }*/
        }

        private IEnumerator ChaseLoop() {
            Debug.Log("ChaseLoop started.");
            while (isChasing && chaseTarget != null) {
                var dist = Vector2.Distance(transform.position, chaseTarget.position);

                if (dist > loseInterestRange || !HasLineOfSight(transform.position, chaseTarget.position)) {
                    Debug.Log("Lost line-of-sight or target out of range. Stopping chase.");
                    StopChasing();
                    yield break;
                }

                yield return new WaitForFixedUpdate();

                transform.position = Vector2.MoveTowards(transform.position,
                    chaseTarget.position,
                    speed * Time.fixedDeltaTime);

                var moved = chaseTarget.position - transform.position;

                var angle = -Vector2.SignedAngle(Vector2.up, moved);
                if (angle < 0) angle += 360f;

                int direction;
                if (angle is >= 315f or < 45f) direction = 1; // North
                else if (angle < 135f) direction = 2; // East
                else if (angle < 225f) direction = 3; // South
                else direction = 4; // West

                animator.SetInteger(Direction, direction);

                Debug.Log($"Chasing target. Position: {transform.position}");
            }

            StopChasing();
        }

        private IEnumerator WanderTo(Vector2 wanderPoint, Action onComplete = null) {
            Debug.Log($"Wandering to: {wanderPoint}");
            wanderPoint = ClosestPointInArea(wanderPoint);

            var v = (wanderPoint - (Vector2)transform.position).normalized;

            var angle = -Vector2.SignedAngle(Vector2.up, v);
            if (angle < 0) angle += 360f;

            int direction;
            if (angle is >= 315f or < 45f) direction = 1; // North
            else if (angle < 135f) direction = 2; // East
            else if (angle < 225f) direction = 3; // South
            else direction = 4; // West

            animator.SetInteger(Direction, direction);

            Debug.Log(
                $"Wandering... Current Position: {transform.position} with angle {angle} and movement vector {v}");

            while (Vector2.Distance(transform.position, wanderPoint) > 0.01f) {
                yield return new WaitForFixedUpdate();

                transform.position = Vector2.MoveTowards(transform.position,
                    wanderPoint,
                    speed * Time.fixedDeltaTime);

                if (!isChasing) continue;
                Debug.Log("Interrupted by chase. Stopping wander.");
                onComplete?.Invoke();
                yield break;
            }

            animator.SetInteger(Direction, 0);

            Debug.Log("Reached wander point.");
            onComplete?.Invoke();
        }

        private Vector2 GetRandomLosPointFromHome() {
            Debug.Log("Finding random LOS point from home.");
            var randomPoint = homePosition;

            for (var i = 0; i < 10; i++) {
                var rx = Random.Range(-5f, 5f);
                var ry = Random.Range(-5f, 5f);
                var candidate = homePosition + new Vector2(rx, ry);

                candidate = ClosestPointInArea(candidate);

                if (HasLineOfSight(homePosition, candidate)) {
                    randomPoint = candidate;
                    Debug.Log($"Found LOS point: {randomPoint}");
                    break;
                }
            }

            return randomPoint;
        }

        private bool HasLineOfSight(Vector2 start, Vector2 end) {
            Debug.Log($"Checking LOS from {start} to {end}");
            var hit = Physics2D.Raycast(start, (end - start).normalized, Vector2.Distance(start, end));

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Obstacle")) {
                Debug.Log("LOS blocked by obstacle.");
                return false;
            }

            Debug.Log("LOS clear.");
            return true;
        }

        private Vector2 ClosestPointInArea(Vector2 position) {
            Debug.Log($"Finding closest point in area to {position}");

            if (area.OverlapPoint(position)) {
                Debug.Log("Point already in area.");
                return position;
            }

            var closest = position;
            var closestDistance = float.MaxValue;

            for (var i = 0; i < area.points.Length; i++) {
                Vector2 pointA = area.transform.TransformPoint(area.points[i]);
                Vector2 pointB = area.transform.TransformPoint(area.points[(i + 1) % area.points.Length]);
                var projected = ProjectPointOnLineSegment(pointA, pointB, position);
                var distance = Vector2.Distance(position, projected);

                if (distance < closestDistance) {
                    closest = projected;
                    closestDistance = distance;
                }
            }

            Debug.Log($"Closest point found: {closest}");
            return closest;
        }

        private static Vector2 ProjectPointOnLineSegment(Vector2 a, Vector2 b, Vector2 p) {
            var ab = b - a;
            var t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / ab.sqrMagnitude);
            return a + t * ab;
        }
    }
}