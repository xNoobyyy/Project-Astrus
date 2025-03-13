using Player;
using UnityEngine;

namespace Items {
    public abstract class Item {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Sprite Icon { get; private set; }
        public RuntimeAnimatorController AnimatedIcon { get; private set; }

        protected Item(string name, string description, Sprite icon, RuntimeAnimatorController animator) {
            Name = name;
            Description = description;
            Icon = icon;
            AnimatedIcon = animator;
        }

        public abstract void OnUse(Transform player, Vector3 position, ClickType clickType);
    }
}