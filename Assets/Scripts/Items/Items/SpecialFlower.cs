using UnityEngine;

namespace Items.Items {
    public class SpecialFlower : ResourceItem {
        public SpecialFlower(int amount = 1) : base("Special flower", "A special flower", ItemManager.Instance.specialFlower,null, amount) { }
    }
}