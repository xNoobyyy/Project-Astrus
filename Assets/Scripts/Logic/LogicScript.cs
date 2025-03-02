using System;
using Player.Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LogicScript : MonoBehaviour {
    public static LogicScript Instance { get; private set; }

    public AccessableInventoryManager accessableInventoryManager;
    public AccessableInventoryManager accessableInventoryManager2;
    public QuestScreenScript questScreen;
    public InventoryScreen inventoryScreen;
    public GameObject background;
    public Watch watch;
    public RectTransform inventoryScreenVisu;
    public RectTransform questScreenVisu;
    public GameObject openingIcon;
    public GameObject pauseScreen;
    private Vector2 QuestPosition;
    private Vector2 QuestSize;
    private bool InventoryOpen = false;
    public bool WatchOpen = false;
    public bool IconOpened = false;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }

    void Start() {
        inventoryScreen.Close();
        QuestPosition = questScreenVisu.transform.position;
        QuestSize = questScreenVisu.sizeDelta;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            OpenInventoryScreen();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !WatchOpen) {
            if (Mathf.Approximately(Time.timeScale, 1.0f)) {
                pauseScreen.SetActive(true);
                Time.timeScale = 0f;
            } else {
                Time.timeScale = 1f;
                pauseScreen.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && InventoryOpen) {
            CloseInventoryScreen();
        }

        if (Input.GetKeyDown(KeyCode.E) && !WatchOpen) {
            OpenWatch();
        }

        if (Input.GetKeyDown(KeyCode.E) && WatchOpen && !IconOpened) {
            CloseWatch();
        }

        if (Input.GetKeyDown(KeyCode.E) && IconOpened) {
            watch.ReverseAnimation();
        }
    }

    public void OpenWatch() {
        watch.open();
        accessableInventoryManager.HideHints();
        accessableInventoryManager2.HideHints();
        //WatchOpen = true;
    }

    public void CloseWatch() {
        watch.close();
        accessableInventoryManager.ShowHints();
        accessableInventoryManager2.ShowHints();
        //WatchOpen = !watch.closed();
    }

    public void AddQuestToQuestScreen(string quest) {
        questScreen.AddQuest(quest);
    }

    public void RemoveQuestFromQuestScreen(string quest) {
        questScreen.CompleteQuest(quest);
    }

    public void OpenInventoryScreen() {
        questScreen.ConvertToInventory(inventoryScreenVisu.transform.position.x,
            inventoryScreenVisu.transform.position.y, inventoryScreenVisu.sizeDelta.x, inventoryScreenVisu.sizeDelta.y);

        //inventoryScreen.Open();
        InventoryOpen = true;
    }

    public void CloseInventoryScreen() {
        questScreen.ConvertToQuestUI(QuestPosition.x, QuestPosition.y, QuestSize.x, QuestSize.y);
        inventoryScreen.Close();
        InventoryOpen = false;
    }
}