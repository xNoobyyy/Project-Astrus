using UnityEngine;
using UnityEngine.EventSystems;

namespace Player.Inventory {
    public class DeleteSlot : MonoBehaviour, IDropHandler {
        public void OnDrop(PointerEventData eventData) {
            if (InventoryScreen.Instance.DraggingFrom == null) return;

            InventoryScreen.Instance.DraggingFrom.SetItem(null);
            InventoryScreen.Instance.ResetDragging();
        }
    }
}