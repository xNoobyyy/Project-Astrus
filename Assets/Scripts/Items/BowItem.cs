using System;
using System.Linq;
using Creatures;
using UnityEditor.Animations;
using UnityEngine;

namespace Items {
    public abstract class BowItem : Item {
        private const float MAX_DISTANCE = 100f;

        public int Damage { get; private set; }

        protected BowItem(string name, string description, Sprite icon, AnimatorController animator, int damage) : base(
            name, description, icon, animator) {
            Damage = damage;
        }

        public override void OnUse(Transform player, Vector3 position) {
            var v = (Vector2)(position - player.position);
            var hits = Physics2D.RaycastAll(player.position, v, MAX_DISTANCE);
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            hits.Select(hit => hit.collider.GetComponent<CreatureBase>())
                .FirstOrDefault(creature => creature != null)?
                .GetComponent<CreatureBase>()?
                .OnAttack(player, Damage);

            OnAttack();
        }

        protected virtual void OnAttack() { }
    }
}