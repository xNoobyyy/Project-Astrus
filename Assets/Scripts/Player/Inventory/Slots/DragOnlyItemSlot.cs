using UnityEngine.EventSystems;

namespace Player.Inventory.Slots {
    // Diese Klasse erbt von ItemSlot
    public class DragOnlyItemSlot : ItemSlot, IDropHandler {
        // Überschreibt den Drop-Handler, sodass keine Items in diesen Slot gezogen werden können.
        public new void OnDrop(PointerEventData eventData) {
            // Setze einfach den Dragging-Zustand zurück,
            // ohne das Item zu übernehmen.
            InventoryScreen.Instance.ResetDragging();
        }
    }
}