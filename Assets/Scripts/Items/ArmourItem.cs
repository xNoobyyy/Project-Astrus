using UnityEditor.Animations;
using UnityEngine;

namespace Items {
    public abstract class ArmourItem : Item {
        public int lvl { get; private set; }

        protected ArmourItem(string name, string description, Sprite icon, AnimatorController animator, int Lvl) : base(name, description, icon, animator) {
            lvl = Lvl;
        }

        public override void OnUse() {
            OnAttack();
        }

        protected virtual void OnAttack() { }
    }
}