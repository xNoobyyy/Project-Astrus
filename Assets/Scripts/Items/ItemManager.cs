using UnityEngine;

namespace Items {
    public class ItemManager : MonoBehaviour {
        [Header("Item Icons")] public Sprite ironPickaxeIcon;
        public Sprite ironIcon;
        public Sprite stoneIcon;

        [Header("Prefabs")] public GameObject droppedItemPrefab;

        public static ItemManager Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        public GameObject CreateDroppedItem(Vector3 position, Item item) {
            var droppedItem = Instantiate(droppedItemPrefab, position, Quaternion.identity);

            var droppedItemComponent = droppedItem.GetComponent<DroppedItem.DroppedItem>();
            droppedItemComponent.Initialize(item);

            return droppedItem;
        }
    }
}