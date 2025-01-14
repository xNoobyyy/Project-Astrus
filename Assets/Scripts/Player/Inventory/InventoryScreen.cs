using System;
using UnityEngine;

namespace Player.Inventory {
    public class InventoryScreen : MonoBehaviour {
        public ItemSlot itemSlotPrefab;
        public GameObject itemSlotContainer;
        
        public static InventoryScreen Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            gameObject.SetActive(false);
            
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

        public void Open() {
            gameObject.SetActive(true);
        }
        
        public void Close() {
            gameObject.SetActive(false);
        }
        
        public void Toggle() {
            gameObject.SetActive(!gameObject.activeSelf);
        }
        
    }
}