using System.Linq;
using Player;
using UnityEngine;
using Utils;

namespace Objects.Placeables {
    public class PlaceableBoat : MonoBehaviour, IInteractable {
        private static readonly int InBoatHash = Animator.StringToHash("InBoat");

        [SerializeField] private GameObject hints;

        public bool InBoat { get; private set; }

        private void Update() {
            if (!InBoat) return;

            if (Input.GetKeyDown(KeyCode.Space)) {
                ExitBoat();
            }
        }

        public void OnInteract(Transform player) {
            if (InBoat) return;

            EnterBoat();
        }

        private void EnterBoat() {
            InBoat = true;
            hints.SetActive(true);

            PlayerItem.Instance.animator.SetBool(InBoatHash, true);
            PlayerMovement.Instance.GetComponent<Rigidbody2D>().linearDamping = 1f;
            PlayerMovement.Instance.speed = 5f;
        }

        private void ExitBoat() {
            InBoat = false;
            hints.SetActive(false);

            PlayerItem.Instance.animator.SetBool(InBoatHash, false);
            PlayerMovement.Instance.GetComponent<Rigidbody2D>().linearDamping = 5f;
            PlayerMovement.Instance.speed = 3f;
        }

        public static bool IsPlaceable(BoxCollider2D box) {
            var boxPolygon = GetColliderPolygon(box);

            foreach (var col in ColliderManager.Instance.Land) {
                var poly = GetColliderPolygon(col);
                if (DoPolygonsOverlap(poly, boxPolygon)) {
                    Debug.Log($"Not placeable: Overlap detected with {col.name}");
                    return false;
                }
            }

            Debug.Log("Placeable: No overlap detected.");
            return true;
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
                if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
                    (point.x <
                     (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) +
                     polygon[i].x)) {
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
    }
}