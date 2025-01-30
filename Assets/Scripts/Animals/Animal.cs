using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Utils.WhiteFlash;
using Random = UnityEngine.Random;

namespace Animals {
    [RequireComponent(typeof(Animator))]
    public class Animal : MonoBehaviour {
        // Animation parameter hashes
        private static readonly int Direction = Animator.StringToHash("direction");

        // Public config
        public float maxHealth;
        public float speed;
        public ParticleSystem damageParticles;
        public float loseInterestRange = 10f; // Distance at which we give up chasing
        public PolygonCollider2D area;
        public Transform player;

        // Public read-only property for health
        public float Health { get; private set; }

        // Home/wandering logic
        private Vector2Int homePosition;

        // References
        private Animator animator;
        private Rigidbody2D rb;

        // Internal coroutines
        private Coroutine idleCoroutine;
        private Coroutine moveCoroutine;

        // The transform (e.g. player) we might want to follow
        private Transform chaseTarget;
        private bool isChasing;

        // Current wander point for visualization
        private Vector2? currentWanderPoint;

        private Vector2Int[] wanderPoints;

        // LOS visualization
        private Vector2? lastLosStart;
        private Vector2? lastLosEnd;
        private bool lastLosClear;

        private void Awake() {
            Debug.Log("Animal Awake: Initializing...");
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            Health = maxHealth;
        }

        private void Start() {
            Debug.Log("Animal Start: Setting home position and starting idle mode.");
            homePosition = Vector2Int.RoundToInt(transform.position);
            wanderPoints = GetRandomLosPointsFromHome();
            damageParticles.Stop();
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

            var idleTime = Random.Range(2f, 10f);
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

            var randomPoint = wanderPoints[Random.Range(0, wanderPoints.Length)];
            currentWanderPoint = randomPoint; // For visualization
            Debug.Log($"Wandering to random point: {randomPoint}");

            moveCoroutine = StartCoroutine(WanderTo(randomPoint, () => {
                Debug.Log("Wander complete.");
                currentWanderPoint = null; // Clear visualization
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
            var force = direction * 15f;
            rb.AddForce(force, ForceMode2D.Impulse);

            Debug.Log($"Knocked back with force {force}");

            // Flash effect
            GetComponent<SpriteFlashEffect>().StartWhiteFlash();

            // Damage particles
            var rotation = Quaternion.FromToRotation(Vector2.right, direction);
            damageParticles.transform.rotation = rotation;
            damageParticles.Play();

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

                var v = chaseTarget.position - transform.position;

                var angle = -Vector2.SignedAngle(Vector2.up, v);
                if (angle < 0) angle += 360f;

                var direction = angle switch { >= 315f or < 45f => 1, < 135f => 2, < 225f => 3, _ => 4 };

                animator.SetInteger(Direction, direction);

                var newPosition = Vector2.MoveTowards(transform.position,
                    chaseTarget.position,
                    speed * Time.fixedDeltaTime);
                transform.position = newPosition;

                Debug.Log($"Chasing target. Position: {transform.position}");
                yield return new WaitForFixedUpdate();
            }

            StopChasing();
        }

        private IEnumerator WanderTo(Vector2 wanderPoint, Action onComplete = null) {
            Debug.Log($"Wandering to: {wanderPoint}");
            wanderPoint = ClosestPointInArea(wanderPoint);
            currentWanderPoint = wanderPoint; // For visualization

            while (Vector2.Distance(transform.position, wanderPoint) > 0.01f) {
                yield return new WaitForFixedUpdate();

                var v = (wanderPoint - (Vector2)transform.position).normalized;

                var angle = -Vector2.SignedAngle(Vector2.up, v);
                if (angle < 0) angle += 360f;

                var direction = angle switch { >= 315f or < 45f => 1, < 135f => 2, < 225f => 3, _ => 4 };

                animator.SetInteger(Direction, direction);

                var newPosition = Vector2.MoveTowards(transform.position,
                    wanderPoint,
                    speed * Time.fixedDeltaTime);
                transform.position = newPosition;

                if (!isChasing) continue;
                Debug.Log("Interrupted by chase. Stopping wander.");
                onComplete?.Invoke();
                yield break;
            }

            animator.SetInteger(Direction, 0);

            Debug.Log("Reached wander point.");
            currentWanderPoint = null; // Clear visualization
            onComplete?.Invoke();
        }

        private Vector2Int[] GetRandomLosPointsFromHome() {
            Debug.Log("Finding random LOS point from home.");

            var points = new List<Vector2Int>(10);

            for (var i = 0; i < 10; i++) {
                var n = 0;
                while (true) {
                    n++;
                    if (n > 1000) {
                        Debug.LogError("Failed to find LOS point. Aborting.");
                        break;
                    }

                    var rx = Random.Range(-5, 5);
                    var ry = Random.Range(-5, 5);
                    if (rx == 0 && ry == 0) continue;

                    var candidate = homePosition + new Vector2Int(rx, ry);

                    Debug.Log("Checking candidate: " + candidate);
                    if (!IsPointInArea(candidate)) continue;
                    Debug.Log("Candidate is in area.");
                    if (!points.All(point => HasLineOfSight(candidate, point))) continue;
                    Debug.Log("Candidate has LOS to all other points.");
                    if (!HasLineOfSight(homePosition, candidate)) continue;
                    Debug.Log("Candidate has LOS to home.");

                    points.Add(candidate);
                    break;
                }
            }

            return points.ToArray();
        }

        private bool HasLineOfSight(Vector2 start, Vector2 end) {
            Debug.Log($"Checking LOS from {start} to {end}");
            var direction = (end - start).normalized;

            const float step = 0.5f;
            var current = start;
            var blocked = false;

            while (Vector2.Distance(current, end) > step) {
                current += direction * step;

                if (!IsPointInArea(current)) {
                    Debug.Log("LOS blocked by area.");
                    blocked = true;
                    break;
                }

                if (Physics2D.OverlapCircleAll(current, 0.5f).All(c => !c.CompareTag("Obstacle"))) continue;
                Debug.Log("LOS blocked by obstacle.");
                blocked = true;
                break;
            }

            // Store LOS data for visualization
            lastLosStart = start;
            lastLosEnd = end;
            lastLosClear = !blocked;

            if (blocked) return false;

            Debug.Log("LOS clear.");
            return true;
        }

        private Vector2 ClosestPointInArea(Vector2 position) {
            Debug.Log($"Finding closest point in area to {position}");

            if (IsPointInArea(position)) {
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

                if (!(distance < closestDistance)) continue;
                closest = projected;
                closestDistance = distance;
            }

            Debug.Log($"Closest point found: {closest}");
            return closest;
        }

        private bool IsPointInArea(Vector2 point) {
            return area.OverlapPoint(point);
        }

        private static Vector2 ProjectPointOnLineSegment(Vector2 a, Vector2 b, Vector2 p) {
            var ab = b - a;
            var t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / ab.sqrMagnitude);
            return a + t * ab;
        }

        // =============================================
        // GIZMOS FOR DEBUGGING
        // =============================================
        private void OnDrawGizmos() {
            // Only draw in editor
            if (!Application.isPlaying) return;

            // Draw Home Position
            Gizmos.color = Color.green;
            Gizmos.DrawSphere((Vector2)homePosition, 0.2f);

            // Draw Lose Interest Range
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            Gizmos.DrawWireSphere((Vector2)homePosition, loseInterestRange);

            // Draw Current Chase Target
            if (chaseTarget != null) {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(chaseTarget.position, 0.3f);
                // Draw LOS line
                if (lastLosStart.HasValue && lastLosEnd.HasValue) {
                    Gizmos.color = lastLosClear ? Color.green : Color.red;
                    Gizmos.DrawLine(lastLosStart.Value, lastLosEnd.Value);
                }
            }

            // Draw Current Wander Point
            if (currentWanderPoint.HasValue) {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(currentWanderPoint.Value, 0.2f);
            }

            // Draw Area Polygon
            if (area != null) {
                Gizmos.color = Color.yellow;
                var points = new Vector3[area.points.Length];
                for (var i = 0; i < area.points.Length; i++) {
                    points[i] = area.transform.TransformPoint(area.points[i]);
                }

                for (var i = 0; i < points.Length; i++) {
                    var start = points[i];
                    var end = points[(i + 1) % points.Length];
                    Gizmos.DrawLine(start, end);
                }
            }

            // Optionally, draw the current movement direction
            if (isChasing && chaseTarget != null) {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, chaseTarget.position);
            } else if (currentWanderPoint.HasValue) {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, currentWanderPoint.Value);
            }
        }
    }
}