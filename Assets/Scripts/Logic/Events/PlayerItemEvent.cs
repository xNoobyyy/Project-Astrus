using Items;

namespace Logic.Events {
    /// <summary>
    /// Event for when a player picks up an item
    /// </summary>
    public class PlayerItemEvent {
        public Item Item;

        public PlayerItemEvent(Item item) {
            Item = item;
        }
    }
}