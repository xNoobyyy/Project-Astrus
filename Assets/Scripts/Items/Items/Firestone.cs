using UnityEngine;

namespace Items.Items {
    public class Firestone : ResourceItem {
        public Firestone(int amount = 1) : base("Firestone", "Stone to make Fire", ItemManager.Instance.fireStoneIcon,null, amount) { }
    }
}