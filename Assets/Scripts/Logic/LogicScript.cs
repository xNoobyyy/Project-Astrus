using Player.Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LogicScript : MonoBehaviour
{
    public QuestScreenScript questScreen;
    public InventoryScreen inventoryScreen;
    public GameObject background;
    public Watch watch;
    public RectTransform inventoryScreenVisu;
    public RectTransform questScreenVisu;
<<<<<<< HEAD
    public GameObject openingIcon;
=======
>>>>>>> d32adcb955bbc4b494285662a2e94ef745dd2462
    private Vector2 QuestPosition;
    private Vector2 QuestSize;
    private bool InventoryOpen = false;
    public bool WatchOpen = false;
<<<<<<< HEAD
    public bool IconOpened = false;
=======
>>>>>>> d32adcb955bbc4b494285662a2e94ef745dd2462
    
    void Start() {
        inventoryScreen.Close();
        QuestPosition = questScreenVisu.transform.position;
        QuestSize = questScreenVisu.sizeDelta;

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenInventoryScreen();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && InventoryOpen) {
            CloseInventoryScreen();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            OpenWatch();
        }
<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.Escape) && WatchOpen && !IconOpened)
=======
        if (Input.GetKeyDown(KeyCode.Escape) && WatchOpen)
>>>>>>> d32adcb955bbc4b494285662a2e94ef745dd2462
        {
            CloseWatch();
            
        }
<<<<<<< HEAD

        if (Input.GetKeyDown(KeyCode.Escape) && IconOpened) {
            watch.ReverseAnimation();
        }
=======
>>>>>>> d32adcb955bbc4b494285662a2e94ef745dd2462
    }

    public void OpenWatch() {
        watch.open();
        //WatchOpen = true;
    }
    
    public void CloseWatch() {
        watch.close();
        //WatchOpen = !watch.closed();
    }

    public void AddQuestToQuestScreen(string quest) {
        questScreen.AddQuest(quest);
    }

    public void RemoveQuestFromQuestScreen(string quest) {
        questScreen.CompleteQuest(quest);
    }

    public void OpenInventoryScreen() {
        
        questScreen.ConvertToInventory(inventoryScreenVisu.transform.position.x, inventoryScreenVisu.transform.position.y, inventoryScreenVisu.sizeDelta.x, inventoryScreenVisu.sizeDelta.y );
        
        //inventoryScreen.Open();
        InventoryOpen = true;
    }

    public void CloseInventoryScreen() {
        questScreen.ConvertToQuestUI(QuestPosition.x, QuestPosition.y, QuestSize.x, QuestSize.y );
        inventoryScreen.Close();
        InventoryOpen = false;
    }
}
