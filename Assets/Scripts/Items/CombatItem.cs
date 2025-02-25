using UnityEditor.Animations;
using UnityEngine;

namespace Items {
    public abstract class CombatItem : Item {
        public int Damage { get; private set; }

        protected CombatItem(string name, string description, Sprite icon, AnimatorController animator, int damage) : base(name, description, icon, animator) {
            Damage = damage;
        }

        public override void OnUse(Transform player, Vector3 position) {
            OnAttack();
        }

        protected virtual void OnAttack() { }
    }
}