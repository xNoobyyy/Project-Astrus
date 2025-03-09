using System;
using System.IO;
using System.Linq;
using Items;
using JetBrains.Annotations;
using Objects;
using Player.Inventory;
using UnityEngine;

namespace Utils {
    public class SerializeManager : MonoBehaviour {
        public static SerializeManager Instance { get; private set; }

        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private Transform player;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private GameObject vinePrefab;
        [SerializeField] private GameObject vineContainer;

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
            } else {
                Debug.LogError($"No save file found at: {path}");
                return null;
            }
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

            var saveData = new SaveData {
                items = items,
                playerPosition = playerPosition,
                reachedPlateau = reachedPlateau,
                vinePositions = vinePositions,
                IdentificatedInteractables = identifiedInteractables,
                playerHealth = currentHealth
            };

            return saveData;
        }

        public void Load(SaveData saveData) {
            for (var i = 0; i < saveData.items.Length; i++) {
                var saveItem = saveData.items[i];
                if (saveItem == null || saveItem.id == "") continue;

                var itemType = ItemList.items.First(il => il.Item1 == saveItem.id).Item2;
                var item = typeof(ResourceItem).IsAssignableFrom(itemType)
                    ? (Item)Activator.CreateInstance(itemType, saveItem.amount)
                    : (Item)Activator.CreateInstance(itemType);

                playerInventory.SetItem(i, item);
            }

            player.position = saveData.playerPosition;

            playerHealth.plateau = saveData.reachedPlateau;

            if (saveData.vinePositions.Length > 0) {
                foreach (Transform vine in vineContainer.transform) {
                    Destroy(vine.gameObject);
                }

                foreach (var vinePosition in saveData.vinePositions) {
                    Instantiate(vinePrefab, vinePosition, Quaternion.identity, vineContainer.transform);
                }
            }

            var interactables = FindObjectsByType<IdentificatedInteractable>(FindObjectsInactive.Include,
                FindObjectsSortMode.None);
            foreach (var (uuid, interactedAt) in saveData.IdentificatedInteractables) {
                var interactable = interactables.FirstOrDefault(ii => ii.Uuid == uuid);
                if (interactable == null) continue;

                interactable.SetInteractedAt(interactedAt);
            }

            playerHealth.currentHealth = saveData.playerHealth;
        }
    }

    [Serializable]
    public class SaveData {
        [ItemCanBeNull] public SaveItem[] items;
        public Vector2 playerPosition;
        public bool reachedPlateau;
        public Vector2[] vinePositions;
        public (string, long)[] IdentificatedInteractables;
        public int playerHealth;
    }

    [Serializable]
    public class SaveItem {
        public string id;
        public int amount;
    }
}