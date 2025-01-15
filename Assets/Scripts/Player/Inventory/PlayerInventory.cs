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
            return slots[index].Item;
        }

        public void RemoveItem(int index) {
            slots[index].SetItem(null);
        }

        public int FirstEmpty() {
            for (var i = 0; i < slots.Length; i++) {
                if (slots[i].Item == null) {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// If the item is a ResourceItem, it will try to stack it with existing items.
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns>true if it can pick up the full item with the full amount</returns>
        public bool CanPickUpItem(Item item) {
            if (item is not ResourceItem resourceItem) return FirstEmpty() != -1;

            if (FirstEmpty() != -1) return true;

            var restAmount = resourceItem.Amount;

            foreach (var slot in slots) {
                if (slot.Item is ResourceItem slotResourceItem &&
                    slotResourceItem.GetType() == resourceItem.GetType()) {
                    restAmount -= (slotResourceItem.MaxAmount - slotResourceItem.Amount);
                }

                if (restAmount <= 0) return true;
            }

            return false;
        }

        public Item PickupItem(Item item) {
            if (item is ResourceItem resourceItem) {
                var restAmount = resourceItem.Amount;

                foreach (var slot in slots) {
                    if (slot.Item is not ResourceItem slotResourceItem ||
                        slotResourceItem.GetType() != resourceItem.GetType()) continue;

                    var rest = slotResourceItem.MaxAmount - slotResourceItem.Amount;

                    if (rest >= restAmount) {
                        slotResourceItem.SetAmount(slotResourceItem.Amount + restAmount);
                        return null;
                    }

                    slotResourceItem.SetAmount(slotResourceItem.MaxAmount);
                    restAmount -= rest;
                }

                resourceItem.SetAmount(restAmount);

                if (restAmount <= 0) return null;
                var firstEmpty = FirstEmpty();
                if (firstEmpty == -1) return resourceItem;

                SetItem(firstEmpty, resourceItem);
            } else {
                var firstEmpty = FirstEmpty();
                if (firstEmpty == -1) return item;

                SetItem(firstEmpty, item);
            }

            return null;
        }
    }
}