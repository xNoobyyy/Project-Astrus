
using System.Collections.Generic;
using Player.Inventory;
using TMPro; // Für TextMeshPro
using UnityEngine;

public class QuestScreenScript : MonoBehaviour
{
    public GameObject questPanel;
    public RectTransform questScreenVisu;
    public TextMeshProUGUI questText; // Verweise auf das Text-Objekt im Panel
    public InventoryScreen inventoryScreen;
    private List<string> quests = new List<string>();
    private bool transforming = false;
    private bool transformingToInventory = false;
    private Vector2 targetPosition;
    private Vector2 targetSize;
    private int transformationSpeed = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateQuestUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (transforming) {
            questScreenVisu.sizeDelta = Vector2.Lerp(
                questScreenVisu.sizeDelta,
                targetSize,
                Time.deltaTime * transformationSpeed
            );

            questScreenVisu.transform.position = Vector2.Lerp(
                questScreenVisu.transform.position,
                targetPosition,
                Time.deltaTime * transformationSpeed
            );

            float tolerance = 0.5f; // Toleranzwert

            if ((Vector2.Distance(questScreenVisu.sizeDelta, targetSize) < tolerance) &&
                (Vector2.Distance(questScreenVisu.transform.position, targetPosition) < tolerance))
            {
                transforming = false;
                
                if (transformingToInventory) {
                    transformingToInventory = false;
                    
                    inventoryScreen.Open();

                }
                
                questScreenVisu.sizeDelta = targetSize;
                questScreenVisu.transform.position = targetPosition;
            }

        }
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

    public void ConvertToInventory(float PosX, float PosY, float Width, float Height) {
        targetPosition = new Vector2(PosX, PosY);
        targetSize = new Vector2(Width, Height);
        transformingToInventory = true;
        transforming = true;
        
        questText.text = "";

    }

    public void ConvertToQuestUI(float PosX, float PosY, float Width, float Height) {
        targetPosition = new Vector2(PosX, PosY);
        targetSize = new Vector2(Width, Height);
        transforming = true;
        UpdateQuestUI();
    }
}
