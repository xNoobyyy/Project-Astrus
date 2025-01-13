using UnityEngine;

namespace Player.Inventory {
    public class InventoryScreen : MonoBehaviour {
        public ItemSlot itemSlotPrefab;
        public GameObject itemSlotContainer;
        
        private void Start() {
            var playerInventory = PlayerInventory.Instance;
            for (var i = 0; i < 8; i++) {
                for (var j = 0; j < 4; j++) {
                    var itemSlot = Instantiate(itemSlotPrefab, itemSlotContainer.transform);
                    itemSlot.GetComponent<RectTransform>().localPosition = new Vector3(
                        i * 70,
                        j * -70,
                        0
                    );

                    playerInventory.slots[i + j * 8] = itemSlot;
                }
            }
        }
    }
}