using System;
using System.Collections;
using Animals;
using UnityEngine;

namespace Pathfinding {
    public class Animal : MonoBehaviour {
        public float Health { get; private set; }

        public float Health { get; private se
        private AStar aStar;

        public float maxHealth;
        public float speed;

        private Vector2 movingTo;
        private Coroutine moveCoroutine;

        private Coroutine idleCoroutine;

        private void Awake() {
            Debug.Log($"[Animal] Awake: Initializing AStar for {gameObject.name}");
            aStar = new AStar(this);
        }

        private void Start() {
            Debug.Log($"[Animal] Start: Initiating idle state for {gameObject.name}");
            StartIdle();
        }

        public void StartIdle() {
            Debug.Log($"[Animal] StartIdle: Attempting to start idle for {gameObject.name}");

            if (idleCoroutine != null) {
                Debug.Log($"[Animal] StartIdle: Idle coroutine already running. Skipping.");
                return;
            }

            if (moveCoroutine != null) {
                Debug.Log($"[Animal] StartIdle: Stopping existing move coroutine");
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            float idleTime = UnityEngine.Random.Range(1f, 10f);
            Debug.Log($"[Animal] StartIdle: Idle time set to {idleTime} seconds");

            idleCoroutine = StartCoroutine(Idle(idleTime, () => {
                Debug.Log($"[Animal] Idle completed. Setting new random target for {gameObject.name}");
                idleCoroutine = null;
                SetTarget(new Vector2(transform.position.x, transform.position.y) +
                          new Vector2(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10)));
            }));
        }

        public void SetTarget(Vector2 target) {
            Debug.Log($"[Animal] SetTarget: Setting new target {target} for {gameObject.name}");

            if (idleCoroutine != null) {
                Debug.Log("[Animal] SetTarget: Stopping idle coroutine");
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }

            if (moveCoroutine != null) {
                Debug.Log("[Animal] SetTarget: Stopping existing move coroutine");
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            moveCoroutine = StartCoroutine(MoveTo(target, () => {
                Debug.Log($"[Animal] Movement to {target} completed for {gameObject.name}");
                moveCoroutine = null;

                StartIdle();
            }));
        }

        public void StopMoving() {
            Debug.Log($"[Animal] StopMoving: Stopping movement for {gameObject.name}");

            if (moveCoroutine != null) {
                Debug.Log("[Animal] StopMoving: Stopping move coroutine");
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            StartIdle();
        }

        private IEnumerator MoveTo(Vector2 target, Action onComplete = null) {
            Debug.Log($"[Animal] MoveTo: Starting path to {target} for {gameObject.name}");

            movingTo = target;

            var path = aStar.FindPath(transform.position, target);

            if (path == null) {
                Debug.LogWarning($"[Animal] MoveTo: No path found to {target} for {gameObject.name}");
                onComplete?.Invoke();
                yield break;
            }

            Debug.Log($"[Animal] MoveTo: Path found with {path.Count} nodes");

            foreach (var node in path) {
                Debug.Log($"[Animal] MoveTo: Moving towards node {node}");

                while (Vector2.Distance(transform.position, node) > 0.1f) {
                    Debug.Log(
                        $"[Animal] MoveTo: Moving towards node {node} from {transform.position} with distance {Vector2.Distance(transform.position, node)} and maxDistanceDelta {speed * Time.fixedDeltaTime}");

                    transform.position = Vector2.MoveTowards(transform.position, node,
                        speed * Time.fixedDeltaTime);

                    Debug.Log(
                        $"[Animal] MoveTo: Current position {transform.position}, Distance to node: {Vector2.Distance(transform.position, node)}");

                    yield return new WaitForFixedUpdate();
                }
            }

            Debug.Log($"[Animal] MoveTo: Reached final destination {target} for {gameObject.name}");
            onComplete?.Invoke();
        }

        private static IEnumerator Idle(float time, Action onComplete = null) {
            Debug.Log($"[Animal] Idle: Starting idle for {time} seconds");

            yield return new WaitForSeconds(time);

            Debug.Log("[Animal] Idle: Idle time completed");
            onComplete?.Invoke();
        }
    }
}