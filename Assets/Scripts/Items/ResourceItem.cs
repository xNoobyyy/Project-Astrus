using UnityEngine;

namespace Items {
    [CreateAssetMenu(fileName = "NewResourceItem", menuName = "Items/Resource")]
    public class ResourceItem : BaseItem {
        private void OnEnable() {
            itemType = ItemType.Resource;
        }
    }
}