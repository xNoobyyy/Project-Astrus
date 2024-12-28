using UnityEngine;

namespace Items {
    [CreateAssetMenu(fileName = "NewCombatItem", menuName = "Items/Combat")]
    public class CombatItem : BaseItem {
        private void OnEnable() {
            itemType = ItemType.Combat;
        }
    }
}