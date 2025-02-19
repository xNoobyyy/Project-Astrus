using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class QuestLogic : MonoBehaviour {

    public TextMeshProUGUI mainTitle;
    public TextMeshProUGUI mainDescription;
    public TextMeshProUGUI mainProgress;
    public TextMeshProUGUI sideText;
    public GameObject sideDescription;

    public Quest testQuest;
    public Quest testQuest2;
    public Quest testQuest3;
    public Quest testQuest4;
    public Quest testQuest5;
    public Quest testQuest6;
    public Quest testQuest7;
    public Quest testQuest8;
    public Quest testQuest9;

    public List<Quest> sideQuests = new List<Quest>();

    void Start()
    {
        testQuest = CreateQuest("TestQuest", "Do a test", "This is a test", true, 10);
        testQuest2 = CreateQuest("TestQuest2", "Do test1", "This is test1", false, 10);
        testQuest3 = CreateQuest("TestQuest3", "Do test2", "This is test2", false, 10);
        testQuest4 = CreateQuest("TestQuest4", "Do test3", "This is test3", false, 10);
        testQuest5 = CreateQuest("TestQuest5", "Do test4", "This is test4", false, 10);
        testQuest6 = CreateQuest("TestQuest6", "Do test5", "This is test5", false, 10);
        testQuest7 = CreateQuest("TestQuest7", "Do test6", "This is test6", false, 10);
        testQuest8 = CreateQuest("TestQuest8", "Do test7", "This is test7", false, 10);
        testQuest9 = CreateQuest("TestQuest9", "Do test8", "This is test8", false, 10);

        // Setze die Hauptquestanzeige
        ChangeMainquestTo(testQuest);

        // Füge alle Quests zur sideQuests-Liste hinzu
        sideQuests.Add(testQuest);
        sideQuests.Add(testQuest2);
        sideQuests.Add(testQuest3);
        sideQuests.Add(testQuest4);
        sideQuests.Add(testQuest5);
        sideQuests.Add(testQuest6);
        sideQuests.Add(testQuest7);
        sideQuests.Add(testQuest8);
        sideQuests.Add(testQuest9);

        UpdateSideQuests();
    }

    // Methode zur Erstellung und Initialisierung einer Quest
    private Quest CreateQuest(string objectName, string title, string description, bool isMainQuest, int requiredProgress)
    {
        GameObject go = new GameObject(objectName);
        Quest quest = go.AddComponent<Quest>();
        quest.title = title;
        quest.description = description;
        quest.isMainQuest = isMainQuest;
        quest.requiredProgress = requiredProgress;
        quest.currentProgress = 0;
        return quest;
    }

    void Update()
    {
        // Update-Logik falls erforderlich
    }

    public void ChangeMainquestTo(Quest quest)
    {
        mainTitle.text = quest.title;
        mainDescription.text = quest.description;
        mainProgress.text = quest.currentProgress.ToString() + "/" + quest.requiredProgress.ToString();
    }

    public void UpdateSideQuests()
    {
        sideText.text = "";
        foreach (Quest quest in sideQuests)
        {
            sideText.text += "\n\n" + quest.title + " - " + quest.description + " (" +
                              quest.currentProgress.ToString() + "/" + quest.requiredProgress.ToString() + ")";
        }
    }
}
