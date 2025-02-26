using Items;
using Player.Inventory;
using UnityEngine;

namespace Logic.Events {
    public class PlayerMoveItemEvent {
        public readonly Item Item;
        public readonly ItemSlot Slot;

        public PlayerMoveItemEvent(Item item, ItemSlot slot) {
            Debug.Log("PlayerMoveItemEvent " + item.Name + "; slot: " + slot);
            Item = item;
            Slot = slot;
        }
    }
}