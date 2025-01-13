using UnityEngine;

namespace Items {
    public abstract class ResourceItem : Item {
        protected ResourceItem(string name, string description, Sprite icon) : base(name, description, icon) { }

        public override void OnUse() { }
    }
}