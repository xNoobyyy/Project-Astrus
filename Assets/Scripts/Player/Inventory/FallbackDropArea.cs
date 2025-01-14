using UnityEngine;
using UnityEngine.EventSystems;

namespace Player.Inventory {
    public class FallbackDropArea : MonoBehaviour, IPointerClickHandler, IEndDragHandler {
        public void OnPointerClick(PointerEventData eventData) {
            Debug.Log("FallbackDropArea.OnPointerClick();");
        }

        public void OnEndDrag(PointerEventData eventData) {
            Debug.Log("FallbackDropArea.OnEndDrag();");

            if (InventoryScreen.Instance.DraggingFrom == null) return;

            InventoryScreen.Instance.ResetDragging();
        }
    }
}