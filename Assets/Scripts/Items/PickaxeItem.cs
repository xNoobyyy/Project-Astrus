using UnityEngine;

namespace Items {
    public abstract class PickaxeItem : Item {
        public int PickPower { get; private set; }

        protected PickaxeItem(string name, string description, Sprite icon, AnimationClip animator, int pickPower) : base(name, description, icon, animator) {
            pickPower = PickPower;
        }

        public override void OnUse() {
            OnPick();
        }

        protected virtual void OnPick() { }
    }
}