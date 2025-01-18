using Player.Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LogicScript : MonoBehaviour
{
    public QuestScreenScript questScreen;
    public InventoryScreen inventoryScreen;
    public GameObject background;
    public RectTransform inventoryScreenVisu;
    
    
    void Start() {
        inventoryScreen.Close();
        
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenInventoryScreen();
        }
    }

    public void AddQuestToQuestScreen(string quest) {
        questScreen.AddQuest(quest);
    }

    public void RemoveQuestFromQuestScreen(string quest) {
        questScreen.CompleteQuest(quest);
    }

    public void OpenInventoryScreen() {
        
        questScreen.ConvertToInventory(inventoryScreenVisu.transform.position.x, inventoryScreenVisu.transform.position.y, inventoryScreenVisu.sizeDelta.x, inventoryScreenVisu.sizeDelta.y );
        inventoryScreen.Open();
    }

    public void CloseInventoryScreen() {
        inventoryScreen.Close();
    }
}
