using UnityEngine;

namespace Items.DroppedItem {
    [RequireComponent(typeof(CircleCollider2D))]
    public class DroppedItemAttractor : MonoBehaviour {
        public DroppedItem droppedItem;

        private Transform player;

        public const float AttractionRadius = 2.5f;

        private void Awake() {
            GetComponent<CircleCollider2D>().radius = AttractionRadius / 2;
        }

        // called whilst player is inside the attraction radius
        private void OnTriggerStay2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            if (player == null) {
                player = other.transform;
            }

            var distance = Vector2.Distance(player.position, droppedItem.transform.position);

            if (distance is > AttractionRadius or < 0.1f) return;


            droppedItem.transform.position = Vector2.MoveTowards(
                droppedItem.transform.position,
                player.position,
                5f * Time.deltaTime
            );
        }
    }
}