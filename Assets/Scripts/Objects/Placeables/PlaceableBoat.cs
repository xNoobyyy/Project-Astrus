using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Events;
using Player;
using UnityEngine;
using Utils;

namespace Objects.Placeables {
    public class PlaceableBoat : Interactable {
        private static readonly int InBoatHash = Animator.StringToHash("boat");

        public bool InBoat { get; private set; }

        private SpriteRenderer spriteRenderer;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable() {
            EventManager.Instance.Subscribe<PlayerMoveEvent>(OnPlayerMove);
        }

        private void OnDisable() {
            EventManager.Instance.Unsubscribe<PlayerMoveEvent>(OnPlayerMove);
        }

        private void Update() {
            if (!InBoat || !Input.GetKeyDown(KeyCode.Space)) return;

            var nearestPoint = ColliderManager.Instance.Land
                .Select(land => CustomClosestPoint(land, transform.position))
                .OrderBy(point => Vector2.Distance(point, transform.position))
                .FirstOrDefault();

            if (nearestPoint == default) return;
            if (Vector2.Distance(nearestPoint, transform.position) > 10f) return;

            ExitBoat(nearestPoint);
        }

        private void OnPlayerMove(PlayerMoveEvent e) {
            if (!InBoat) return;

            transform.position = e.To;
        }

        public override void OnInteract(Transform player) {
            if (InBoat) return;

            EnterBoat(player);
        }

        private void EnterBoat(Transform player) {
            InBoat = true;

            PlayerItem.Instance.animator.SetBool(InBoatHash, true);
            PlayerMovement.Instance.GetComponent<Rigidbody2D>().linearDamping = 1f;
            PlayerMovement.Instance.speed = 5f;

            spriteRenderer.enabled = false;

            player.position = transform.position;

            PlayerItem.Instance.InBoat = true;
        }

        private void ExitBoat(Vector2 point) {
            InBoat = false;

            PlayerItem.Instance.animator.SetBool(InBoatHash, false);
            PlayerMovement.Instance.GetComponent<Rigidbody2D>().linearDamping = 5f;
            PlayerMovement.Instance.speed = 3f;

            spriteRenderer.enabled = true;

            var v = (point - (Vector2)PlayerItem.Instance.transform.position).normalized * 0.5f;
            PlayerItem.Instance.transform.position = point + v;

            PlayerItem.Instance.InBoat = false;
        }

        public static bool IsPlaceable(BoxCollider2D box) {
            var boxPolygon = GetColliderPolygon(box);

            return !ColliderManager.Instance.Land.Select(GetColliderPolygon)
                .Any(poly => DoPolygonsOverlap(poly, boxPolygon));
        }

        // Converts any Collider2D to a polygon representation in world space.
        private static Vector2[] GetColliderPolygon(Collider2D collider) {
            if (collider is PolygonCollider2D poly) {
                // Use the first path directly.
                var points = poly.GetPath(0);
                for (var i = 0; i < points.Length; i++) {
                    points[i] = poly.transform.TransformPoint(points[i]);
                }

                return points;
            }

            switch (collider) {
                case BoxCollider2D box: {
                    var size = box.size;
                    var offset = box.offset;
                    var points = new Vector2[4];
                    points[0] = new Vector2(-size.x, -size.y) * 0.5f + offset;
                    points[1] = new Vector2(-size.x, size.y) * 0.5f + offset;
                    points[2] = new Vector2(size.x, size.y) * 0.5f + offset;
                    points[3] = new Vector2(size.x, -size.y) * 0.5f + offset;
                    for (var i = 0; i < points.Length; i++) {
                        points[i] = box.transform.TransformPoint(points[i]);
                    }

                    return points;
                }
                default: {
                    var b = collider.bounds;
                    var points = new Vector2[4];
                    points[0] = new Vector2(b.min.x, b.min.y);
                    points[1] = new Vector2(b.min.x, b.max.y);
                    points[2] = new Vector2(b.max.x, b.max.y);
                    points[3] = new Vector2(b.max.x, b.min.y);
                    return points;
                }
            }
        }

        // Checks if two polygons (represented by arrays of Vector2 points) overlap.
        private static bool DoPolygonsOverlap(Vector2[] polyA, Vector2[] polyB) {
            // Check if any vertex of polyA is inside polyB.
            if (polyA.Any(point => IsPointInPolygon(point, polyB))) return true;

            // Check if any vertex of polyB is inside polyA.
            if (polyB.Any(point => IsPointInPolygon(point, polyA))) return true;

            // Check for edge intersections.
            var countA = polyA.Length;
            var countB = polyB.Length;
            for (var i = 0; i < countA; i++) {
                var a1 = polyA[i];
                var a2 = polyA[(i + 1) % countA];
                for (var j = 0; j < countB; j++) {
                    var b1 = polyB[j];
                    var b2 = polyB[(j + 1) % countB];
                    if (LineSegmentsIntersect(a1, a2, b1, b2))
                        return true;
                }
            }

            return false;
        }

        // Implements a ray-casting algorithm to check if a point is inside a polygon.
        private static bool IsPointInPolygon(Vector2 point, Vector2[] polygon) {
            var isInside = false;
            var count = polygon.Length;
            for (int i = 0, j = count - 1; i < count; j = i++) {
                if (polygon[i].y > point.y != polygon[j].y > point.y &&
                    point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) +
                    polygon[i].x) {
                    isInside = !isInside;
                }
            }

            return isInside;
        }

        // Checks if two line segments (p->p2 and q->q2) intersect.
        private static bool LineSegmentsIntersect(Vector2 p, Vector2 p2, Vector2 q, Vector2 q2) {
            var r = p2 - p;
            var s = q2 - q;
            var denominator = r.x * s.y - r.y * s.x;
            if (Mathf.Approximately(denominator, 0))
                return false; // Lines are parallel.
            var qp = q - p;
            var t = (qp.x * s.y - qp.y * s.x) / denominator;
            var u = (qp.x * r.y - qp.y * r.x) / denominator;
            return (t is >= 0 and <= 1 && u is >= 0 and <= 1);
        }

        private static Vector2 CustomClosestPoint(Vector2[] polygon, Vector2 point) {
            var closest = Vector2.zero;
            var minDistance = float.MaxValue;
            for (var i = 0; i < polygon.Length; i++) {
                var a = polygon[i];
                var b = polygon[(i + 1) % polygon.Length];
                var candidate = ClosestPointOnLineSegment(a, b, point);
                var dist = Vector2.Distance(candidate, point);

                if (!(dist < minDistance)) continue;

                minDistance = dist;
                closest = candidate;
            }

            return closest;
        }

        private static Vector2 ClosestPointOnLineSegment(Vector2 a, Vector2 b, Vector2 point) {
            var ab = b - a;
            var t = Vector2.Dot(point - a, ab) / Vector2.Dot(ab, ab);
            t = Mathf.Clamp01(t);
            return a + t * ab;
        }

        public static Vector2 CustomClosestPoint(Collider2D collider, Vector2 point) {
            var polygon = GetColliderPolygon(collider);
            return CustomClosestPoint(polygon, point);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            if (!InBoat) return;
            if (ColliderManager.Instance == null || ColliderManager.Instance.Land == null) return;

            var nearestPoint = ColliderManager.Instance.Land
                .Select(land => CustomClosestPoint(land, transform.position))
                .OrderBy(point => Vector2.Distance(point, transform.position))
                .FirstOrDefault();

            if (nearestPoint == default) return;
            if (Vector2.Distance(nearestPoint, transform.position) > 10f) return;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, nearestPoint);
            Gizmos.DrawSphere(nearestPoint, 0.3f);

            Gizmos.color = Color.red;
            var v = (nearestPoint - (Vector2)PlayerItem.Instance.transform.position).normalized * 0.5f;
            Gizmos.DrawSphere(nearestPoint + v, 0.3f);
        }
#endif
    }
}