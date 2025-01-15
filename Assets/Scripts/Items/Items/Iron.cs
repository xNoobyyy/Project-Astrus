using UnityEngine;

namespace Items.Items {
    public class Iron : ResourceItem {
        public Iron(int amount = 1) : base("Iron", "A piece of iron", ItemManager.Instance.ironIcon, amount) { }
    }
}