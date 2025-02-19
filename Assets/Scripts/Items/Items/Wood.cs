using UnityEngine;

namespace Items.Items {
    public class Wood : ResourceItem {
        public Wood(int amount = 1) : base("Wood", "A piece of Wood", ItemManager.Instance.holzIcon,null, amount) { }
    }
}