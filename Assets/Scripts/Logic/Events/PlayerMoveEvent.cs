using UnityEngine;

namespace Logic.Events {
    public class PlayerMoveEvent {
        public readonly Vector2 From;
        public readonly Vector2 To;
        
        public PlayerMoveEvent(Vector2 from, Vector2 to) {
            From = from;
            To = to;
        }
    }
}