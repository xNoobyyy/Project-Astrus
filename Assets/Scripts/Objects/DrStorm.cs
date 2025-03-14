using UnityEngine;
using WatchAda.Quests;

namespace Objects {
    public class DrStorm : MonoBehaviour {
        public static DrStorm Instance { get; private set; }

        private void OnEnable() {
            if (Instance == null) {
                Instance = this;
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            QuestLogic.Instance.stormQuest.CompleteQuest();
        }
    }
}