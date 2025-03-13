using Logic;
using Player;
using UnityEngine;

namespace Items.Items {
    public class InvisibilityPotion : ResourceItem {
        public InvisibilityPotion(int amount = 1) : base("Invisibility potion", "A potion to make you invisible",
            ItemManager.Instance.invisiblePotionIcon, null, amount) { }

        public override void OnUse(Transform player, Vector3 position, ClickType clickType) {
            base.OnUse(player, position, clickType);
            PlayerItem.Instance.MakeInvisible();

            Amount--;

            if (clickType == ClickType.Left) {
                LogicScript.Instance.accessableInventoryManager.CurrentItemSlot.SetItem(Amount == 0 ? null : this);
                LogicScript.Instance.accessableInventoryManager.UpdateSlots();
            } else {
                LogicScript.Instance.accessableInventoryManager2.CurrentItemSlot.SetItem(Amount == 0 ? null : this);
                LogicScript.Instance.accessableInventoryManager2.UpdateSlots();
            }
        }
    }
}