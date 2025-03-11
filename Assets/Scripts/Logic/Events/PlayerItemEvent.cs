using Items;
using Player.Inventory;
using Player.Inventory.Slots;
using UnityEngine;

namespace Logic.Events {
    /// <summary>
    /// Event for when a player picks up an item
    /// </summary>
    public class PlayerItemEvent {
        public readonly Item Item;
        public readonly ItemSlot Slot;

        public PlayerItemEvent(Item item, ItemSlot slot) {
            Item = item;
            this.Slot = slot;
        }
    }
}