using System;
using Player;
using UnityEditor.Animations;
using UnityEngine;
using Tree = Objects.Tree;

namespace Items {
    public abstract class AxeItem : Item {
        public int ChopPower { get; private set; }

        protected AxeItem(string name, string description, Sprite icon, AnimatorController animator, int chopPower) :
            base(name, description, icon, animator) {
            ChopPower = chopPower;
        }

        public override void OnUse(Transform player, Vector3 position) {
            if (PlayerItem.Instance.IsBusy || Vector2.Distance(player.position, position) > 3f) return;

            var colliders = Physics2D.OverlapPointAll(position);
            var trees = Array.FindAll(colliders,
                c => c.GetComponent<Tree>() != null || c.GetComponentInParent<Tree>() != null);
            Array.Sort(trees, (c1, c2) => c1.transform.position.y.CompareTo(c2.transform.position.y));
            var treeCollider = trees.Length > 0 ? trees[0] : null;
            var tree = treeCollider?.GetComponent<Tree>() ?? treeCollider?.GetComponentInParent<Tree>();

            tree?.Chop(ChopPower);
            PlayerItem.Instance.Chop();
            OnChop();
        }

        protected virtual void OnChop() { }
    }
}