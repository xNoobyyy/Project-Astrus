using UnityEngine;

namespace Items {
    public abstract class AxeItem : Item {
        public int ChopPower { get; private set; }

        protected AxeItem(string name, string description, Sprite icon, AnimationClip animator, int chopPower) : base(name, description, icon, animator) {
            ChopPower = chopPower;
        }

        public override void OnUse() {
            OnChop();
        }

        protected virtual void OnChop() { }
    }
}