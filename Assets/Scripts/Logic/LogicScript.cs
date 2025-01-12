using UnityEngine;

public class LogicScript : MonoBehaviour
{
    public QuestScreenScript questScreen;

    void Start() {
        
    }

    public void AddQuestToQuestScreen(string quest) {
        questScreen.AddQuest(quest);
    }

    public void RemoveQuestFromQuestScreen(string quest) {
        questScreen.CompleteQuest(quest);
    }
}
