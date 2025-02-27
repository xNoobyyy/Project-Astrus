using Items;

namespace Logic.Events {
    public class PlayerChangeHeldItemEvent {
        public readonly Item Item;
        public readonly bool Shift;

        public PlayerChangeHeldItemEvent(Item item, bool shift) {
            Item = item;
            Shift = shift;
        }
    }
}