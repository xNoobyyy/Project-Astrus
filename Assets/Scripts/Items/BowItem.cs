using UnityEditor.Animations;
using UnityEngine;

namespace Items {
    public abstract class BowItem : Item {
        public int Damage { get; private set; }

        protected BowItem(string name, string description, Sprite icon, AnimatorController animator, int damage) : base(name, description, icon, animator) {
            Damage = damage;
        }

        public override void OnUse(Transform player, Vector3 position) {
            OnAttack();
        }

        protected virtual void OnAttack() { }
    }
}