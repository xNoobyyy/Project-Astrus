using System.Collections.Generic;
using System.Linq;
using Items;
using Logic.Events;
using UnityEngine;

namespace Player.Inventory {
    public class PlayerInventory : MonoBehaviour {
        public ItemSlot[] Slots { get; private set; }

        public ItemSlot[] NeutralSlots =>
            Slots.Where(slot => slot != null && !slot.GetType().IsSubclassOf(typeof(ItemSlot)))
                .ToArray();

        [SerializeField] private GameObject slotContainer;

        public static PlayerInventory Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            Slots = slotContainer.GetComponentsInChildren<ItemSlot>();
            Debug.Log($"All Slots: {Slots.Length} and Neutral Slots {NeutralSlots.Length}");
        }

        public void SetItem(int index, Item item) {
            var slot = Slots[index];
            EventManager.Instance.Trigger(new PlayerItemEvent(item, slot));
            slot.SetItem(item);
        }

        public Item GetItem(int index) {
            return Slots[index].Item;
        }

        public void RemoveItem(int index) {
            Slots[index].SetItem(null);
        }

        public int FirstEmpty() {
            foreach (var slot in NeutralSlots) {
                if (slot.Item == null) {
                    return Slots.ToList().IndexOf(slot);
                }
            }

            return -1;
        }

        /// <summary>
        /// If the item is a ResourceItem, it will try to stack it with existing items.
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns>true if it can pick up any amount of the item</returns>
        public bool CanPickUpItem(Item item) {
            if (item is not ResourceItem resourceItem) return FirstEmpty() != -1;
            if (FirstEmpty() != -1) return true;

            foreach (var slot in NeutralSlots) {
                if (slot.Item is not ResourceItem slotResourceItem ||
                    slotResourceItem.GetType() != resourceItem.GetType()) continue;

                if (slotResourceItem.Amount != slotResourceItem.MaxAmount) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to pick up the item. If the item is a ResourceItem, it will first try to stack it with existing items.
        /// </summary>
        /// <param name="item">The item to pick up</param>
        /// <returns>true, when the full item was picked up, false when the full item was not picked up and the amount of the referenced item was changed</returns>
        public bool PickupItem(Item item) {
            if (item is ResourceItem resourceItem) {
                var restAmount = resourceItem.Amount;

                foreach (var slot in NeutralSlots) {
                    if (slot.Item is not ResourceItem slotResourceItem ||
                        slotResourceItem.GetType() != resourceItem.GetType()) {
                        continue;
                    }

                    var rest = slotResourceItem.MaxAmount - slotResourceItem.Amount;

                    if (rest >= restAmount) {
                        slotResourceItem.SetAmount(slotResourceItem.Amount + restAmount);
                        EventManager.Instance.Trigger(new PlayerItemEvent(slotResourceItem, slot));
                        slot.UpdateDisplay();
                        return true;
                    }

                    slotResourceItem.SetAmount(slotResourceItem.MaxAmount);
                    EventManager.Instance.Trigger(new PlayerItemEvent(slotResourceItem, slot));
                    slot.UpdateDisplay();
                    restAmount -= rest;
                }

                resourceItem.SetAmount(restAmount);

                if (restAmount <= 0) {
                    return true;
                }

                var firstEmpty = FirstEmpty();
                if (firstEmpty == -1) {
                    return false;
                }

                SetItem(firstEmpty, resourceItem);
            } else {
                var firstEmpty = FirstEmpty();
                if (firstEmpty == -1) {
                    return false;
                }

                SetItem(firstEmpty, item);
            }

            return true;
        }
    }
}