using UnityEngine;

namespace Items.Items {
    public class Fire : ResourceItem {
        public Fire(int amount = 1) : base("Fire", "A Place lit up with fire", ItemManager.Instance.fireIcon,null, amount) { }
    }
}