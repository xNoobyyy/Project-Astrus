using UnityEngine;

namespace Items {
    public enum ItemType {
        Combat,
        Axe,
        Pickaxe,
        Resource,
    }

    public abstract class BaseItem : ScriptableObject {
        public string itemName;
        public ItemType itemType { get; protected set; }
        public Sprite itemIcon;
        public string itemDescription;
    }

    [CreateAssetMenu(fileName = "NewCombatItem", menuName = "Items/Combat")]
    public class CombatItem : BaseItem {
        private void OnEnable() {
            itemType = ItemType.Combat;
        }
    }

    [CreateAssetMenu(fileName = "NewAxeItem", menuName = "Items/Axe")]
    public class AxeItem : BaseItem {
        private void OnEnable() {
            itemType = ItemType.Axe;
        }
    }

    [CreateAssetMenu(fileName = "NewPickaxeItem", menuName = "Items/Pickaxe")]
    public class PickaxeItem : BaseItem {
        private void OnEnable() {
            itemType = ItemType.Pickaxe;
        }
    }

    [CreateAssetMenu(fileName = "NewResourceItem", menuName = "Items/Resource")]
    public class ResourceItem : BaseItem {
        private void OnEnable() {
            itemType = ItemType.Resource;
        }
    }
}