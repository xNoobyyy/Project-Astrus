using UnityEditor.Animations;
using UnityEngine;

namespace Items {
    public abstract class ArmorItem : Item {
        public int lvl { get; private set; }

        protected ArmorItem(string name, string description, Sprite icon, AnimatorController animator, int Lvl) : base(name, description, icon, animator) {
            lvl = Lvl;
        }

        public override void OnUse(Transform player, Vector3 position) {
            OnAttack();
        }

        protected virtual void OnAttack() { }
    }
}