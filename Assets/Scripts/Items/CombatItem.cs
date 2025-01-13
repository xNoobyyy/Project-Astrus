using UnityEngine;

namespace Items {
    public abstract class CombatItem : Item {
        public int Damage { get; private set; }

        protected CombatItem(string name, string description, Sprite icon, int damage) : base(name, description, icon) {
            Damage = damage;
        }

        public override void OnUse() {
            OnAttack();
        }

        protected virtual void OnAttack() { }
    }
}