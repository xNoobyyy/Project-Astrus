using System;
using System.Linq;
using Objects;
using Player;
using UnityEditor.Animations;
using UnityEngine;
using Tree = UnityEngine.Tree;

namespace Items {
    public abstract class PickaxeItem : Item {
        public int PickPower { get; private set; }

        protected PickaxeItem(string name, string description, Sprite icon, AnimatorController animator, int pickPower)
            : base(name, description, icon, animator) {
            PickPower = pickPower;
        }

        public override void OnUse(Transform player, Vector3 position, ClickType clickType) {
            if (PlayerItem.Instance.IsBusy) return;

            var colliders = Physics2D.OverlapPointAll(position);
            var oreColliders = Array.FindAll(colliders,
                c => c.GetComponent<Ore>() != null || c.GetComponentInParent<Ore>() != null);
            Array.Sort(oreColliders, (c1, c2) => c1.transform.position.y.CompareTo(c2.transform.position.y));
            var ores = oreColliders.Select(c => c?.GetComponent<Ore>() ?? c?.GetComponentInParent<Ore>()).ToArray();
            var nonDestroyed = Array.FindAll(ores, t => !t.IsDestroyed);
            var ore = nonDestroyed.Length > 0 ? nonDestroyed[0] : null;

            if (ore == null) return;
            if (Vector2.Distance(ore.trigger.ClosestPoint(player.transform.position), player.transform.position) >
                5f) return;

            ore.Break(PickPower);
            PlayerItem.Instance.Chop();

            OnPick();
        }

        protected virtual void OnPick() { }
    }
}