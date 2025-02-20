using UnityEngine;

namespace Items.Items {
    public class Fireplace : ResourceItem {
        public Fireplace(int amount = 1) : base("Fireplace", "A place that can be lit up with fire", ItemManager.Instance.fireplaceIcon,null, amount) { }
    }
}