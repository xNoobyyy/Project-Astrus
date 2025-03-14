using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Items;
using JetBrains.Annotations;
using Objects;
using Objects.Placeables;
using Player;
using Player.Inventory;
using TextDisplay;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.Caves;
using WatchAda.Quests;

namespace Utils {
    public class SerializeManager : MonoBehaviour {
        public static SerializeManager Instance { get; private set; }

        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private Transform player;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private GameObject vinePrefab;
        [SerializeField] private GameObject vineContainer;
        [SerializeField] private QuestLogic questLogic;
        [SerializeField] private GameObject boatPrefab;
        [SerializeField] private CircularPhotoPanel circularPhotoPanel;
        [SerializeField] private MapManager mapManager;
        [SerializeField] private PlayerItem playerItem;
        [SerializeField] private CaveManager caveManager;

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start() {
            var json = LoadFromFile();
            Debug.Log(json, this);
            if (json == null) return;

            var saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log(saveData, this);
            Load(saveData);
        }

        private void OnEnable() {
            InvokeRepeating(nameof(SaveToFile), 300f, 300f);
        }

        private void SaveToFile() {
            PauseScreen.SaveToFile(JsonUtility.ToJson(Save(), true));
        }

        private static string LoadFromFile() {
            var path = Path.Combine(Application.persistentDataPath, "savegame.json");
            if (File.Exists(path)) {
                var json = File.ReadAllText(path);
                Debug.Log($"Loaded from: {path}");
                return json;
            }

            Debug.Log($"No save file found at: {path}");
            return null;
        }

        public SaveData Save() {
            var items = playerInventory.Slots.Select(slot =>
                slot.Item != null
                    ? new SaveItem {
                        id = ItemList.items.First(il => il.Item2 == slot.Item.GetType()).Item1,
                        amount = slot.Item is ResourceItem ri ? ri.Amount : -1
                    }
                    : null).ToArray();

            var playerPosition = player.position;

            var reachedPlateau = playerHealth.plateau;

            var vinePositions = vineContainer.transform.Cast<Transform>().Select(vine => (Vector2)vine.position)
                .ToArray();

            var identifiedInteractables =
                FindObjectsByType<IdentificatedInteractable>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                    .Where(ii => ii.InteractedAt != -1).Select(ii => new IdentificatedInteractableState {
                        uuid = ii.Uuid,
                        interactedAt = ii.InteractedAt
                    }).ToArray();

            var currentHealth = playerHealth.currentHealth;

            var questProgresses = questLogic.questGroups.SelectMany(qg => qg.subQuests)
                .Select(q => new QuestState { id = q.id, progress = q.currentProgress })
                .ToArray();

            var dialogueOpenedRecipies = TextDisplayManager.Instance.openedRecipies;

            var dialogueDiedFromZombie = TextDisplayManager.Instance.diedFromZombie;

            var dialogueDrStorm = TextDisplayManager.Instance.textDisplay.talkedToDrStorm;

            var boatPositions = FindObjectsByType<PlaceableBoat>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Select(boat => (Vector2)boat.transform.position).ToArray();

            var advancedRecipies = circularPhotoPanel.plateuGefunden;

            var visited = mapManager.visited;

            var finished = playerItem.Finished;

            var inCave = caveManager.IsCave;

            var saveData = new SaveData {
                items = items,
                playerPosition = playerPosition,
                reachedPlateau = reachedPlateau,
                vinePositions = vinePositions,
                identificatedInteractables = identifiedInteractables,
                playerHealth = currentHealth,
                questProgresses = questProgresses,
                dialogueOpenedRecipies = dialogueOpenedRecipies,
                dialogueDiedFromZombie = dialogueDiedFromZombie,
                dialogueDrStorm = dialogueDrStorm,
                boatPositions = boatPositions,
                advancedRecipies = advancedRecipies,
                Visited = visited,
                finished = finished,
                inCave = inCave
            };

            return saveData;
        }

        public void Load(SaveData saveData) {
            if (saveData.items is { Length: > 0 }) {
                for (var i = 0; i < saveData.items.Length; i++) {
                    var saveItem = saveData.items[i];
                    if (saveItem == null || saveItem.id == "") continue;

                    var itemType = ItemList.items.First(il => il.Item1 == saveItem.id).Item2;
                    var item = typeof(ResourceItem).IsAssignableFrom(itemType)
                        ? (Item)Activator.CreateInstance(itemType, saveItem.amount)
                        : (Item)Activator.CreateInstance(itemType);

                    playerInventory.SetItem(i, item);
                }
            }

            if (saveData.playerPosition != Vector2.zero)
                player.position = saveData.playerPosition;

            if (saveData.reachedPlateau) playerHealth.plateau = saveData.reachedPlateau;

            if (saveData.vinePositions is { Length: > 0 }) {
                foreach (Transform vine in vineContainer.transform) {
                    Destroy(vine.gameObject);
                }

                foreach (var vinePosition in saveData.vinePositions) {
                    Instantiate(vinePrefab, vinePosition, Quaternion.identity, vineContainer.transform);
                }
            }

            if (saveData.identificatedInteractables is { Length: > 0 }) {
                var interactables = FindObjectsByType<IdentificatedInteractable>(FindObjectsInactive.Include,
                    FindObjectsSortMode.None);
                foreach (var state in saveData.identificatedInteractables) {
                    var interactable = interactables.FirstOrDefault(ii => ii.Uuid == state.uuid);
                    if (interactable == null) continue;

                    interactable.SetInteractedAt(state.interactedAt);
                }
            }

            if (saveData.playerHealth != 0) playerHealth.currentHealth = saveData.playerHealth;

            if (saveData.questProgresses is { Length: > 0 }) {
                foreach (var qp in saveData.questProgresses) {
                    var quest = questLogic.questGroups.SelectMany(qg => qg.subQuests)
                        .FirstOrDefault(q => q.id == qp.id);
                    Debug.Log($"Setting quest progress for {quest} to {qp.progress}");
                    if (quest != null) quest.UpdateProgress(qp.progress, false);
                    else Debug.LogError($"Quest {qp.id} not found");
                }
            }

            if (saveData.dialogueOpenedRecipies)
                TextDisplayManager.Instance.openedRecipies = saveData.dialogueOpenedRecipies;

            if (saveData.dialogueDiedFromZombie)
                TextDisplayManager.Instance.diedFromZombie = saveData.dialogueDiedFromZombie;

            if (saveData.dialogueDrStorm) {
                TextDisplayManager.Instance.textDisplay.talkedToDrStorm = saveData.dialogueDrStorm;
                DrStorm.Instance.gameObject.SetActive(!saveData.dialogueDrStorm);
            }

            if (saveData.boatPositions is { Length: > 0 }) {
                foreach (var boat in FindObjectsByType<PlaceableBoat>(FindObjectsInactive.Include,
                             FindObjectsSortMode.None).Select(boat => boat.transform)) {
                    Destroy(boat.gameObject);
                }

                foreach (var boatPosition in saveData.boatPositions) {
                    Instantiate(boatPrefab, boatPosition, Quaternion.identity);
                }
            }

            if (saveData.advancedRecipies) circularPhotoPanel.plateuGefunden = saveData.advancedRecipies;

            if (saveData.Visited is { Length: > 0 }) {
                mapManager.visited = saveData.Visited;
            }

            if (saveData.finished) playerItem.Finish();

            caveManager.SetInCave(saveData.inCave);
        }
    }

    [Serializable]
    public class SaveData {
        [ItemCanBeNull] public SaveItem[] items;
        public Vector2 playerPosition;
        public bool reachedPlateau;
        public Vector2[] vinePositions;
        public IdentificatedInteractableState[] identificatedInteractables;
        public int playerHealth;
        public QuestState[] questProgresses;
        public bool dialogueOpenedRecipies;
        public bool dialogueDiedFromZombie;
        public bool dialogueDrStorm;
        public Vector2[] boatPositions;
        public bool advancedRecipies;
        public bool[,] Visited;
        public bool finished;
        public bool inCave;
    }

    [Serializable]
    public class SaveItem {
        public string id;
        public int amount;
    }

    [Serializable]
    public class QuestState {
        public string id;
        public int progress;
    }

    [Serializable]
    public class IdentificatedInteractableState {
        public string uuid;
        public long interactedAt;
    }
}