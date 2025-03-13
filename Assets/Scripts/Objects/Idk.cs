using System;
using System.Collections;
using Items;
using Items.Items;
using Player;
using UnityEngine;
using WatchAda.Quests;

namespace Objects {
    public class Idk : MonoBehaviour {
        [SerializeField] private CircularPhotoPanel circularPhotoPanel;
        [SerializeField] private Canvas swampMapCanvas;

        private void OnCollisionEnter2D(Collision2D other) {
            if (!other.gameObject.CompareTag("Player")) return;
            if (circularPhotoPanel.plateuGefunden) return;

            circularPhotoPanel.plateuGefunden = true;
            if (!QuestLogic.Instance.RezepteQuest.isCompleted) QuestLogic.Instance.RezepteQuest.CompleteQuest();
            ItemManager.Instance.DropItem(new BottleOfWater(), transform.position);
            
            swampMapCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
            StartCoroutine(CloseMap());
        }

        private IEnumerator CloseMap() {
            yield return new WaitForSeconds(5);
            swampMapCanvas.GetComponentInChildren<SwampMapImage>().CloseSwampMap();
        }
    }
}