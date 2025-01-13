using System;
using Items;
using UnityEngine;

namespace Player.Inventory {
    public class PlayerInventory : MonoBehaviour {
        public ItemSlot[] slots = new ItemSlot[32];

        public static PlayerInventory Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        public void SetItem(int index, Item item) {
            slots[index].SetItem(item);
        }

        public Item GetItem(int index) {
            return slots[index].item;
        }

        public void RemoveItem(int index) {
            slots[index].SetItem(null);
        }
    }
}