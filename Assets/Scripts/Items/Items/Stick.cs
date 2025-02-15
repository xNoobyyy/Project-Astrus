using UnityEngine;

namespace Items.Items {
    public class Stick : ResourceItem {
        public Stick(int amount = 1) : base("Stick", "A stick of wood", ItemManager.Instance.stickIcon,null, amount) { }
    }
}
