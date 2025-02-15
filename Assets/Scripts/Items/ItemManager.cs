using UnityEditor.Animations;
using UnityEngine;

namespace Items {
    public class ItemManager : MonoBehaviour {
        [Header("Item Icons")] public Sprite ironPickaxeIcon;
        public Sprite rockPickaxeIcon;
        public Sprite ironIcon;
        public Sprite stoneIcon;
        public Sprite glomtomIcon2;
        public Sprite extricIcon2;
        public Sprite domilitantIcon2;
        public Sprite AstrusIcon2;
        public Sprite Sword1Icon;
        public Sprite Sword2Icon;
        public Sprite Sword3Icon;
        public Sprite Bow1Icon;
        public Sprite Bow2Icon;
        public Sprite Bow3Icon;
        public Sprite BowFire1Icon;
        public Sprite BowFire2Icon;
        public Sprite BowFire3Icon;
        public Sprite Axe1Icon;
        public Sprite Axe2Icon;
        public Sprite Amour1Icon;
        public Sprite Amour2Icon;
        
        public AnimatorController glomtomIcon;
        public AnimatorController extricIcon;
        public AnimatorController domilitantIcon;
        public AnimatorController AstrusIcon;
        public AnimatorController SwordAni;
        public AnimatorController AxeAni;
        public AnimatorController BowAni;
        public AnimatorController BowAni2;

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