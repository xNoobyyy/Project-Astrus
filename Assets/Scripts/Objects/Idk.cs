using System;
using UnityEngine;
using WatchAda.Quests;

namespace Objects {
    public class Idk : MonoBehaviour {
        [SerializeField] private CircularPhotoPanel circularPhotoPanel;

        private void OnCollisionEnter2D(Collision2D other) {
            if (!other.gameObject.CompareTag("Player")) return;
            if (circularPhotoPanel.plateuGefunden) return;

            circularPhotoPanel.plateuGefunden = true;
            if (!QuestLogic.Instance.RezepteQuest.isCompleted) QuestLogic.Instance.RezepteQuest.CompleteQuest();
        }
    }
}