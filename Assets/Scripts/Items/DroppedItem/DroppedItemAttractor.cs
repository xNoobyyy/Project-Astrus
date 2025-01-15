using UnityEngine;

namespace Items.DroppedItem {
    [RequireComponent(typeof(CircleCollider2D))]
    public class DroppedItemAttractor : MonoBehaviour {
        public DroppedItem droppedItem;

        private Transform player;
        private float radius;

        private void Awake() {
            radius = GetComponent<CircleCollider2D>().radius;
        }

        // called whilst player is inside the attraction radius
        private void OnTriggerStay2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            if (player == null) {
                player = other.transform;
            }

            var distance = Vector2.Distance(player.position, droppedItem.transform.position);

            if (distance > radius * 2 || distance < 0.1f) return;

            // f(x) = (x-radius*2)^2 / (radius*2)^2 -> 0 at radius and 1 at 0
            var attraction = Mathf.Pow((distance - radius * 2) / (radius * 2), 2);

            Debug.Log($"Distance: {distance}, Attraction: {attraction}");

            droppedItem.transform.position = Vector2.MoveTowards(
                droppedItem.transform.position,
                player.position,
                attraction * Time.fixedDeltaTime
            );
        }
    }
}