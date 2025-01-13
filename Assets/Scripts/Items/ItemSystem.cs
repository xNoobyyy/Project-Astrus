using UnityEngine;

namespace Items {

    public abstract class Item : ScriptableObject {
        
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Sprite Icon { get; private set; }

        protected Item(string name, string description, Sprite icon) {
            Name = name;
            Description = description;
            Icon = icon;
        }

        public abstract void OnUse();

    }
}