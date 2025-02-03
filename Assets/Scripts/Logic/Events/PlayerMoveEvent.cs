using UnityEngine;

namespace Logic.Events {
    public class PlayerMoveEvent {
        public readonly Vector2 From;
        public readonly Vector2 To;
        public readonly Transform Transform;

        public PlayerMoveEvent(Vector2 from, Vector2 to, Transform transform) {
            From = from;
            To = to;
            Transform = transform;
        }
    }
}