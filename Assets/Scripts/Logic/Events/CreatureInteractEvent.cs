using Creatures;

namespace Logic.Events {
    public class CreatureInteractEvent {
        public readonly CreatureBase Creature;
        public readonly InteractionType InteractionType;

        public CreatureInteractEvent(CreatureBase creature, InteractionType type) {
            Creature = creature;
            InteractionType = type;
        }
    }

    public enum InteractionType {
        Pet,
        Attack,
    }
}