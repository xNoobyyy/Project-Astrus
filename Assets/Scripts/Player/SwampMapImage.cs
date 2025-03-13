using UnityEngine;
using WatchAda.Quests;

namespace Player {
    public class SwampMapImage : MonoBehaviour {
        public void CloseSwampMap() {
            transform.parent.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}