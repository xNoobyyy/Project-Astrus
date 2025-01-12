
using System.Collections.Generic;
using TMPro; // Für TextMeshPro
using UnityEngine;

public class QuestScreenScript : MonoBehaviour
{
    public TextMeshProUGUI questText; // Verweise auf das Text-Objekt im Panel
    private List<string> quests = new List<string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateQuestUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddQuest(string quest) {
        quests.Add(quest);
        UpdateQuestUI();
    }

    public void CompleteQuest(string quest) {
        quests.Remove(quest);
        UpdateQuestUI();
    }

    private void UpdateQuestUI() {
        questText.text = "Aktuelle Quests:\n";
        foreach (string quest in quests) {
            questText.text += quest + "\n";
        }
    }
}
