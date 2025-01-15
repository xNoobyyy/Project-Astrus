using UnityEngine;

namespace Items.DroppedItem {
    public class DroppedItemPickup : MonoBehaviour {
        public DroppedItem droppedItem;

        public void OnTriggerEnter2D(Collider2D other) { }

        public void OnTriggerStay2D(Collider2D other) { }

        private void OnTriggerExit2D(Collider2D other) { }
    }
}