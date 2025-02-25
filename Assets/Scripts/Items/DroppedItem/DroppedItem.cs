using System;
using System.Collections;
using Items.Items;
using UnityEngine;

namespace Items.DroppedItem {
    /// <summary>
    /// use ItemManager#CreateDroppedItem to create a dropped item
    /// </summary>
    public class DroppedItem : MonoBehaviour {
        public Item Item { get; private set; }

        private Vector3 target;
        private Coroutine moveCoroutine;

        public void Initialize(Item item) {
            Item = item;
            GetComponent<SpriteRenderer>().sprite = Item.Icon;
        }

        public void MoveTo(Vector3 to) {
            target = to;
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveCoroutine());
        }

        private IEnumerator MoveCoroutine() {
            var distance = Vector2.Distance(transform.position, target);
            while (distance > 0.1f) {
                distance = Vector2.Distance(transform.position, target);

                transform.position = Vector2.MoveTowards(
                    transform.position,
                    target,
                    5f * Time.fixedDeltaTime
                );
                yield return new WaitForFixedUpdate();
            }

            target = Vector3.zero;
        }
    }
}