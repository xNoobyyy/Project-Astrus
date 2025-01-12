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
        public Sprite itemIcon;
        public string itemDescription;
    }
}