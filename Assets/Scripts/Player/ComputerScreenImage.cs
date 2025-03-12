using UnityEngine;
using WatchAda.Quests;

namespace Player {
    public class ComputerScreenImage : MonoBehaviour {
        public void CloseComputerScreen() {
            transform.parent.gameObject.SetActive(false);
            Time.timeScale = 1f;

            if (!QuestLogic.Instance.virusQuest.isCompleted) QuestLogic.Instance.virusQuest.CompleteQuest();
        }
    }
}