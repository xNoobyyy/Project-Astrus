using System;
using UnityEngine;
using WatchAda.Quests;

namespace Creatures {
    public class BossArea : MonoBehaviour {
        [SerializeField] private Boss boss;

        private new Collider2D collider;

        private void Awake() {
            collider = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (!other.gameObject.CompareTag("Player")) return;
            if (boss.Entered) return;

            boss.Entered = true;
            other.transform.position = boss.playerSpawn.position;
            collider.isTrigger = false;

            if (!QuestLogic.Instance.laborVerlassenQuest.isCompleted)
                QuestLogic.Instance.laborVerlassenQuest.CompleteQuest();
        }
    }
}