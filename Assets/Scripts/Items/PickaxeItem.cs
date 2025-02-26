using UnityEditor.Animations;
using UnityEngine;

namespace Items {
    public abstract class PickaxeItem : Item {
        public int PickPower { get; private set; }

        protected PickaxeItem(string name, string description, Sprite icon, AnimatorController animator, int pickPower) : base(name, description, icon, animator) {
            pickPower = PickPower;
        }

        public override void OnUse(Transform player, Vector3 position) {
            OnPick();
        }

        protected virtual void OnPick() { }
    }
}