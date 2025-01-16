using System;
using Items.Items;
using UnityEngine;

namespace Items.DroppedItem {
    /// <summary>
    /// use ItemManager#CreateDroppedItem to create a dropped item
    /// </summary>
    public class DroppedItem : MonoBehaviour {
        public Item Item { get; private set; }

        public bool IsTargeting { get; private set; }
        private Vector3 target;

        private void FixedUpdate() {
            if (!IsTargeting) return;

            var distance = Vector2.Distance(transform.position, target);

            if (distance < 0.1f) {
                IsTargeting = false;
                target = Vector3.zero;
                return;
            }

            // f(x) = 5 * x^2
            var speed = 5 * Mathf.Pow(distance, 2);

            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                speed * Time.fixedDeltaTime
            );
        }

        public void Initialize(Item item) {
            Item = item;
            GetComponent<SpriteRenderer>().sprite = Item.Icon;
        }

        public void MoveTo(Vector3 to) {
            target = to;
            IsTargeting = true;
        }
    }
}