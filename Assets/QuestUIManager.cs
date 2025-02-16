using UnityEngine;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    [Header("Hauptquest UI")]
    public TMP_Text mainQuestTitle;
    public TMP_Text mainQuestDescription;
    
    [Header("Nebenquests UI")]
    public Transform sideQuestContainer; // Das Content-Objekt der ScrollView
    public GameObject sideQuestPrefab;    // Prefab für einen Nebenquest-Eintrag

    // Beispielmethode zum Aktualisieren der Hauptquest
    public void UpdateMainQuest(Quest mainQuest)
    {
        if(mainQuest == null) return;
        mainQuestTitle.text = mainQuest.title;
        mainQuestDescription.text = mainQuest.description;
    }
    
    // Beispielmethode zum Aktualisieren der Nebenquests
    public void UpdateSideQuests(Quest[] sideQuests)
    {
        // Vorherige Einträge löschen:
        foreach (Transform child in sideQuestContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Neue Einträge erstellen:
        foreach (Quest quest in sideQuests)
        {
            GameObject questItem = Instantiate(sideQuestPrefab, sideQuestContainer);
            TMP_Text questText = questItem.GetComponentInChildren<TMP_Text>();
            if (questText != null)
                questText.text = quest.title; // Du kannst hier weitere Details hinzufügen
        }
    }
}