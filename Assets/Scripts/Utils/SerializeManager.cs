using System;
using System.IO;
using System.Linq;
using Items;
using JetBrains.Annotations;
using Objects;
using Player.Inventory;
using UnityEngine;
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
                    .Where(ii => ii.InteractedAt != -1).Select(ii => (UUID: ii.Uuid, ii.InteractedAt)).ToArray();

            var currentHealth = playerHealth.currentHealth;

            var questProgresses = questLogic.questGroups.SelectMany(qg => qg.subQuests)
                .Select(q => (q.id, q.currentProgress)).ToArray();

            var saveData = new SaveData {
                items = items,
                PlayerPosition = playerPosition,
                ReachedPlateau = reachedPlateau,
                vinePositions = vinePositions,
                IdentificatedInteractables = identifiedInteractables,
                PlayerHealth = currentHealth,
                QuestProgresses = questProgresses
            };

            return saveData;
        }

        public void Load(SaveData saveData) {
            if (saveData.items is { Length: 0 }) {
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

            if (saveData.PlayerPosition.HasValue) player.position = saveData.PlayerPosition.Value;

            if (saveData.ReachedPlateau.HasValue) playerHealth.plateau = saveData.ReachedPlateau.Value;

            if (saveData.vinePositions is { Length: > 0 }) {
                foreach (Transform vine in vineContainer.transform) {
                    Destroy(vine.gameObject);
                }

                foreach (var vinePosition in saveData.vinePositions) {
                    Instantiate(vinePrefab, vinePosition, Quaternion.identity, vineContainer.transform);
                }
            }

            if (saveData.IdentificatedInteractables is { Length: > 0 }) {
                var interactables = FindObjectsByType<IdentificatedInteractable>(FindObjectsInactive.Include,
                    FindObjectsSortMode.None);
                foreach (var (uuid, interactedAt) in saveData.IdentificatedInteractables) {
                    var interactable = interactables.FirstOrDefault(ii => ii.Uuid == uuid);
                    if (interactable == null) continue;

                    interactable.SetInteractedAt(interactedAt);
                }
            }

            if (saveData.PlayerHealth.HasValue) playerHealth.currentHealth = saveData.PlayerHealth.Value;

            if (saveData.QuestProgresses is { Length: > 0 }) {
                foreach (var (id, progress) in saveData.QuestProgresses) {
                    questLogic.questGroups.SelectMany(qg => qg.subQuests).FirstOrDefault(q => q.id == id)
                        ?.UpdateProgress(progress);
                    questLogic.UpdateSideQuests();
                    questLogic.UpdateMainQuest();
                }
            }
        }
    }

    [Serializable]
    public class SaveData {
        [ItemCanBeNull] public SaveItem[] items;
        public Vector2? PlayerPosition;
        public bool? ReachedPlateau;
        public Vector2[] vinePositions;
        public (string, long)[] IdentificatedInteractables;
        public int? PlayerHealth;
        public (string, int)[] QuestProgresses;
    }

    [Serializable]
    public class SaveItem {
        public string id;
        public int amount;
    }
}