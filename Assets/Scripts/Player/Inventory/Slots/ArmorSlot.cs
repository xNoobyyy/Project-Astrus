using Items;
using Player.Inventory.Slots;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player.Inventory {
    public class ArmorSlot : ItemSlot, IBeginDragHandler, IDragHandler, IEndDragHandler,
        IDropHandler {
        /// <summary>
        /// Setzt das Item in diesem Slot. Akzeptiert nur null oder Items vom Typ PickaxeItem.
        /// Wird ein anderes Item übergeben, wird es ignoriert.
        /// </summary>
        /// <param name="item">Das zu setzende Item</param>
        public GameObject panel;

        public new void SetItem(Item item) {
            // Falls das Item nicht null ist und kein PickaxeItem, wird es abgelehnt.
            if (item != null && item is not ArmorItem) {
                panel.SetActive(true);
                return;
            }

            // Aufruf der Basisimplementierung für die visuelle Darstellung.
            panel.SetActive(false);
            base.SetItem(item);
        }

        /// <summary>
        /// Startet den Drag-Vorgang, sofern ein Item in diesem Slot vorhanden ist.
        /// </summary>
        /// <param name="eventData">Informationen zum Drag-Event</param>
        public new void OnBeginDrag(PointerEventData eventData) {
            if (Item == null) return;
            InventoryScreen.Instance.StartDragging(this);
        }

        /// <summary>
        /// Beendet den Drag-Vorgang.
        /// </summary>
        /// <param name="eventData">Informationen zum End-Drag-Event</param>
        public new void OnEndDrag(PointerEventData eventData) {
            if (InventoryScreen.Instance.DraggingFrom == null) return;
            InventoryScreen.Instance.ResetDragging();
        }

        /// <summary>
        /// Wird während des Drag-Vorgangs aufgerufen. (Muss vorhanden sein – bleibt hier leer)
        /// </summary>
        /// <param name="eventData">Informationen zum Drag-Event</param>
        public new void OnDrag(PointerEventData eventData) { } // DO NOT DELETE

        /// <summary>
        /// Wird aufgerufen, wenn ein Item auf diesen Slot gedroppt wird.
        /// Akzeptiert werden nur Items vom Typ ArmorItem.
        /// Andernfalls wird das Item in den vorherigen Slot zurückgestellt.
        /// </summary>
        /// <param name="eventData">Informationen zum Drop-Event</param>
        public new void OnDrop(PointerEventData eventData) {
            if (InventoryScreen.Instance.DraggingFrom?.Item is not ArmorItem) {
                InventoryScreen.Instance.ResetDragging();
                return;
            }

            base.OnDrop(eventData);
        }
    }
}