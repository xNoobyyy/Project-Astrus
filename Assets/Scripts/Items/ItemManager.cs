using UnityEditor.Animations;
using UnityEngine;

namespace Items {
    public class ItemManager : MonoBehaviour {
        [Header("Item Icons")] public Sprite ironPickaxeIcon;
        public Sprite ironIcon;
        public Sprite stoneIcon;
        public AnimatorController glomtomIcon;
        public AnimatorController extricIcon;
        public AnimatorController domilitantIcon;
        public AnimatorController AstrusIcon;

        [Header("Prefabs")] public GameObject droppedItemPrefab;

        public static ItemManager Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        public GameObject CreateDroppedItem(Vector3 position, Vector3 from, Item item) {
            var droppedItem = Instantiate(droppedItemPrefab, from, Quaternion.identity);

            var droppedItemComponent = droppedItem.GetComponent<DroppedItem.DroppedItem>();
            droppedItemComponent.Initialize(item);

            droppedItemComponent.MoveTo(position);

            return droppedItem;
        }
    }
}