using Creatures;

namespace Logic.Events {
    public class CreatureDamageEvent {
        public readonly CreatureType CreatureType;
        public readonly CreatureBase Creature;

        public CreatureDamageEvent(CreatureBase creature) {
            CreatureType = creature.type;
            Creature = creature;
        }
    }
}