using Items;
using Player.Inventory;
using UnityEngine;

namespace Logic.Events {
    public class PlayerMoveItemEvent {
        public readonly Item Item;
        public readonly ItemSlot Slot;

        public PlayerMoveItemEvent(Item item, ItemSlot slot) {
            Item = item;
            Slot = slot;
        }
    }
}