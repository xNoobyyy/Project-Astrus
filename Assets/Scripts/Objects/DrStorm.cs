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

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            QuestLogic.Instance.stormQuest.CompleteQuest();
        }
    }
}