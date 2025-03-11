using UnityEngine;
using WatchAda.Quests;

namespace Objects {
    public class DrStorm : MonoBehaviour {
        public static DrStorm Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (!other.collider.CompareTag("Player")) return;

            QuestLogic.Instance.stormQuest.CompleteQuest();
        }
    }
}