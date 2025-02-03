using UnityEngine;

namespace Logic.Events {
    public class PlayerDamageEvent {
        public readonly int Damage;
        public readonly Transform Source;

        public PlayerDamageEvent(int damage, Transform source) {
            Damage = damage;
            Source = source;
        }
    }
}