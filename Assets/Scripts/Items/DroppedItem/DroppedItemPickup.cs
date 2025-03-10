using Player.Inventory;
using UnityEngine;

namespace Items.DroppedItem {
    public class DroppedItemPickup : MonoBehaviour {
        public DroppedItem droppedItem;

        private bool waitingForPickup;

        public void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            if (!droppedItem.IsDroppingMotion) {
                var pickedUp = PlayerInventory.Instance.PickupItem(droppedItem.Item);
                if (pickedUp) {
                    Destroy(droppedItem.gameObject);
                    return;
                }
            }

            waitingForPickup = true;
        }

        public void OnTriggerStay2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;
            if (!waitingForPickup) return;
            if (droppedItem.IsDroppingMotion) return;

            if (!PlayerInventory.Instance.CanPickUpItem(droppedItem.Item)) return;
            if (!PlayerInventory.Instance.PickupItem(droppedItem.Item)) return;

            Destroy(droppedItem.gameObject);
            waitingForPickup = false;
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            waitingForPickup = false;
        }
    }
}