using UnityEditor.Animations;
using UnityEngine;

namespace Items {
    public abstract class AxeItem : Item {
        public int ChopPower { get; private set; }

        protected AxeItem(string name, string description, Sprite icon, AnimatorController animator, int chopPower) :
            base(name, description, icon, animator) {
            ChopPower = chopPower;
        }

        public override void OnUse(Transform player, Vector3 position) {
            if (Vector2.Distance(player.position, position) > 3f) return;
            
            OnChop();
        }

        protected virtual void OnChop() { }
    }
}