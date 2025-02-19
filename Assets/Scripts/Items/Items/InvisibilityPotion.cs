using UnityEngine;

namespace Items.Items {
    public class InvisibilityPotion : ResourceItem {
        public InvisibilityPotion(int amount = 1) : base("Invisibility potion", "A potion to make you invisible", ItemManager.Instance.invisiblePotionIcon,null, amount) { }
    }
}