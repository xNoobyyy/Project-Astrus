using Items;
using UnityEngine;

namespace Logic.Events {
    public class PlayerChangeHeldItemEvent {
        public readonly Item Item;
        public readonly bool Shift;

        public PlayerChangeHeldItemEvent(Item item, bool shift) {
            Debug.Log("PlayerChangeHeldItemEvent " + item.Name + "; shift: " + shift);
            Item = item;
            Shift = shift;
        }
    }
}