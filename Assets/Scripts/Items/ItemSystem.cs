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
}