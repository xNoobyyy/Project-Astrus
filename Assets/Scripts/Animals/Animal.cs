using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animals {
    [RequireComponent(typeof(Animator))]
    public class Animal : MonoBehaviour {
        private static readonly int HorizontalMove = Animator.StringToHash("horizontal_move");
        private static readonly int VerticalMove = Animator.StringToHash("vertical_move");

        public float Health { get; private set; }

        private AStar aStar;

        public float maxHealth;
        public float speed;
        public PolygonCollider2D area;

        private Vector2 movingTo;
        private Coroutine moveCoroutine;

        private Coroutine idleCoroutine;

        private Animator animator;

        private void Awake() {
            aStar = new AStar(this);

            animator = GetComponent<Animator>();
        }

        private void Start() {
            StartIdle();
        }

        public void StartIdle() {
            if (idleCoroutine != null) return;

            if (moveCoroutine != null) {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            var idleTime = Random.Range(3f, 10f);

            animator.SetFloat(HorizontalMove, 0);
            animator.SetFloat(VerticalMove, 0);

            idleCoroutine = StartCoroutine(Idle(idleTime, () => {
                idleCoroutine = null;
                SetTarget(new Vector2(transform.position.x, transform.position.y) +
                          new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)));
            }));
        }

        public void SetTarget(Vector2 target) {
            if (idleCoroutine != null) {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }

            if (moveCoroutine != null) {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            moveCoroutine = StartCoroutine(MoveTo(target, () => {
                moveCoroutine = null;

                StartIdle();
            }));
        }

        public void StopMoving() {
            if (moveCoroutine != null) {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            StartIdle();
        }

        private IEnumerator MoveTo(Vector2 target, Action onComplete = null) {
            target = ClosestPointInArea(target);
            movingTo = target;

            var path = aStar.FindPath(transform.position, target);

            if (path == null) {
                onComplete?.Invoke();
                yield break;
            }

            foreach (var node in path) {
                while (Vector2.Distance(transform.position, node) > 0.01f) {
                    var originalPosition = transform.position;

                    transform.position = Vector2.MoveTowards(transform.position, node,
                        speed * Time.fixedDeltaTime);

                    var moved = new Vector2(transform.position.x - originalPosition.x,
                        transform.position.y - originalPosition.y);

                    moved.Normalize();

                    animator.SetFloat(HorizontalMove, moved.x);
                    animator.SetFloat(VerticalMove, moved.y);

                    yield return new WaitForFixedUpdate();
                }
            }

            animator.SetFloat(HorizontalMove, 0);
            animator.SetFloat(VerticalMove, 0);

            onComplete?.Invoke();
        }

        private static IEnumerator Idle(float time, Action onComplete = null) {
            yield return new WaitForSeconds(time);
            onComplete?.Invoke();
        }

        private static Vector2 ProjectPointOnLineSegment(Vector2 a, Vector2 b, Vector2 p) {
            var ab = b - a;
            var t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / ab.sqrMagnitude);
            return a + t * ab;
        }

        private Vector2 ClosestPointInArea(Vector2 position) {
            if (area.OverlapPoint(position)) return position;

            var closest = position;
            var closestDistance = float.MaxValue;

            // Check each segment of the polygon
            for (var i = 0; i < area.points.Length; i++) {
                Vector2 pointA = area.transform.TransformPoint(area.points[i]);
                Vector2 pointB = area.transform.TransformPoint(
                    area.points[(i + 1) % area.points.Length]
                );

                var projected = ProjectPointOnLineSegment(pointA, pointB, position);
                var distance = Vector2.Distance(position, projected);

                if (!(distance < closestDistance)) continue;
                closest = projected;
                closestDistance = distance;
            }

            return closest;
        }
    }
}