using Creatures;

namespace Logic.Events {
    public class CreatureDeathEvent {
        public readonly CreatureBase Creature;

        public CreatureDeathEvent(CreatureBase creature) {
            Creature = creature;
        }
    }
}