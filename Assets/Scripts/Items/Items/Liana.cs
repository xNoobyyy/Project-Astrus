using UnityEngine;

namespace Items.Items {
    public class Liana : ResourceItem {
        public Liana(int amount = 1) : base("Liana", "A liana from the jungle", ItemManager.Instance.lianaIcon,null, amount) { }
    }
}