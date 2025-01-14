using UnityEngine;
using UnityEngine.EventSystems;

namespace Player.Inventory {
    public class FallbackDropArea : MonoBehaviour, IDropHandler {
        public void OnDrop(PointerEventData eventData) {
            if (InventoryScreen.Instance.DraggingFrom == null) return;

            InventoryScreen.Instance.ResetDragging();
        }
    }
}