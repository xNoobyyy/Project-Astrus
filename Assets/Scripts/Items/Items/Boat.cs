using UnityEngine;

namespace Items.Items {
    public class Boat : ResourceItem {
        public Boat(int amount = 1) : base("Boat", "A Boat of wood", ItemManager.Instance.bootIcon,null, amount) { }
    }
}