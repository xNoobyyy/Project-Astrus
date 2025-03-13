using Player;
using UnityEngine;

namespace Items {
    public abstract class ArmorItem : Item {
        public int Lvl { get; private set; }

        protected ArmorItem(string name, string description, Sprite icon, RuntimeAnimatorController animator, int lvl) : base(name, description, icon, animator) {
            Lvl = lvl;
        }

        public override void OnUse(Transform player, Vector3 position, ClickType clickType) {
            OnAttack();
        }

        protected virtual void OnAttack() { }
    }
}