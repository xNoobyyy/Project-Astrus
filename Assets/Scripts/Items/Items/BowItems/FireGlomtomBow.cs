using Creatures;
using UnityEngine;

namespace Items.Items {
    public class FireGlomtomBow : BowItem {
        public FireGlomtomBow() : base("Fire Glomtom Bow", "A Bow made of fire and glomtom",
            ItemManager.Instance.BowFire3Icon, ItemManager.Instance.BowAni2,
            3) { }

        protected override void OnAttack(CreatureBase creature, Transform player) {
            creature.SetOnFire();
        }
    }
}