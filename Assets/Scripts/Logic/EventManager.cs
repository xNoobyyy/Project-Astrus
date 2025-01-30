using UnityEngine;

namespace Logic {
    public class EventManager : MonoBehaviour {
        public delegate void PlayerMoveEvent(Vector2 from, Vector2 to);

        public static event PlayerMoveEvent OnPlayerMove;

        public void InvokePlayerMove(Vector2 from, Vector2 to) {
            OnPlayerMove?.Invoke(from, to);
        }
    }
}