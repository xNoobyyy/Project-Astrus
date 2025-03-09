using Creatures;

namespace Logic.Events {
    public class CreatureDeathEvent {
        public readonly CreatureType CreatureType;
        public readonly CreatureBase Creature;

        public CreatureDeathEvent(CreatureBase creature) {
            CreatureType = creature.type;
            Creature = creature;
        }
    }
}