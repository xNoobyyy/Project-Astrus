using System;
using System.Linq;
using Player;
using UnityEditor.Animations;
using UnityEngine;
using Tree = Objects.Tree;

namespace Items {
    public abstract class AxeItem : Item {
        public int ChopPower { get; private set; }

        protected AxeItem(string name, string description, Sprite icon, RuntimeAnimatorController animator, int chopPower) :
            base(name, description, icon, animator) {
            ChopPower = chopPower;
        }

        public override void OnUse(Transform player, Vector3 position, ClickType clickType) {
            if (PlayerItem.Instance.IsBusy) return;

            var colliders = Physics2D.OverlapPointAll(position);
            var treeColliders = Array.FindAll(colliders,
                c => c.GetComponent<Tree>() != null || c.GetComponentInParent<Tree>() != null);
            Array.Sort(treeColliders, (c1, c2) => c1.transform.position.y.CompareTo(c2.transform.position.y));
            var trees = treeColliders.Select(c => c?.GetComponent<Tree>() ?? c?.GetComponentInParent<Tree>()).ToArray();
            var nonDestroyed = Array.FindAll(trees, t => !t.IsDestroyed);
            var tree = nonDestroyed.Length > 0 ? nonDestroyed[0] : null;

            if (tree == null) return;
            if (Vector2.Distance(tree.trigger.ClosestPoint(player.transform.position), player.transform.position) >
                5f) return;

            tree.Chop(ChopPower);
            PlayerItem.Instance.Chop();
            OnChop();
        }

        protected virtual void OnChop() { }
    }
}