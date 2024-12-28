using UnityEngine;

namespace Items {
    [CreateAssetMenu(fileName = "NewAxeItem", menuName = "Items/Axe")]
    public class AxeItem : BaseItem {
        private void OnEnable() {
            itemType = ItemType.Axe;
        }
    }
}