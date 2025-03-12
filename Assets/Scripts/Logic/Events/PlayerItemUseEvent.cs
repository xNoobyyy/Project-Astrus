using Items;
using Player;
using UnityEngine;

namespace Logic.Events {
    public class PlayerItemUseEvent {
        public readonly Item Item;
        public readonly Vector2 Position;
        public readonly ClickType ClickType;

        public PlayerItemUseEvent(Item item, Vector2 position, ClickType clickType) {
            Item = item;
            Position = position;
            ClickType = clickType;
        }
    }
}