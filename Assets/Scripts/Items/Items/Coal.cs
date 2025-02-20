using UnityEngine;

namespace Items.Items {
    public class Coal : ResourceItem {
        public Coal(int amount = 1) : base("Coal", "A piece of coal", ItemManager.Instance.coalIcon,null, amount) { }
    }
}