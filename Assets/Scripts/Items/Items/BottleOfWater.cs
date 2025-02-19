using UnityEngine;

namespace Items.Items {
    public class BottleOfWater : ResourceItem {
        public BottleOfWater(int amount = 1) : base("Bottle of Water", "A Bottle filled with water", ItemManager.Instance.bottleOfWaterIcon,null, amount) { }
    }
}