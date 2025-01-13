using UnityEngine;

namespace Items {
    public abstract class PickaxeItem : Item {
        public int PickPower { get; private set; }

        protected PickaxeItem(string name, string description, Sprite icon, int pickPower) : base(name, description,
            icon) {
            pickPower = PickPower;
        }

        public override void OnUse() {
            OnPick();
        }

        protected virtual void OnPick() { }
    }
}