using System;
using Items;
using Items.Items;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Inventory {
    public class ArmourSlot : ItemSlot, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
        /// <summary>
        /// Setzt das Item in diesem Slot. Akzeptiert nur null oder Items vom Typ PickaxeItem.
        /// Wird ein anderes Item übergeben, wird es ignoriert.
        /// </summary>
        /// <param name="item">Das zu setzende Item</param>
        public GameObject panel;
        public new void SetItem(Item item) {
            // Falls das Item nicht null ist und kein PickaxeItem, wird es abgelehnt.
            if (item != null && !(item is ArmourItem)) {
                panel.SetActive(true);
                return;
            }
            // Aufruf der Basisimplementierung für die visuelle Darstellung.
            panel.SetActive(false);
            base.SetItem(item);
        }

        /// <summary>
        /// Beim Klick in den Slot: Falls der Slot leer ist, wird immer eine neue IronPickaxe eingefügt.
        /// </summary>
        /// <param name="eventData">Informationen zum Klick-Event</param>
        public new void OnPointerClick(PointerEventData eventData) {
            // Falls bereits ein Item vorhanden ist, passiert nichts.
            if (Item != null) return;

            int index = Array.IndexOf(PlayerInventory.Instance.Slots, this);
            // Bei beiden Maustasten wird eine IronPickaxe eingefügt.
            PlayerInventory.Instance.SetItem(index, new GlomtomSword());
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
        /// Akzeptiert werden nur Items vom Typ PickaxeItem.
        /// Andernfalls wird das Item in den vorherigen Slot zurückgestellt.
        /// </summary>
        /// <param name="eventData">Informationen zum Drop-Event</param>
        public new void OnDrop(PointerEventData eventData) {
            if (InventoryScreen.Instance.DraggingFrom == null) return;

            // Prüfe, ob das gezogene Item ein PickaxeItem ist.
            if (!(InventoryScreen.Instance.DraggingFrom.Item is ArmourItem)) {
                // Nicht akzeptiert: Setze den Drag-Vorgang zurück.
                InventoryScreen.Instance.ResetDragging();
                return;
            }
            
            // Falls versucht wird, den Slot auf sich selbst zu droppen, wird nichts getan.
            if (InventoryScreen.Instance.DraggingFrom == this) {
                InventoryScreen.Instance.ResetDragging();
                return;
            }

            // Tausche die Items zwischen diesem Slot und dem Ursprungs-Slot.
            var draggedItem = InventoryScreen.Instance.DraggingFrom.Item;
            var currentItem = Item;

            SetItem(draggedItem);
            InventoryScreen.Instance.DraggingFrom.SetItem(currentItem);

            InventoryScreen.Instance.ResetDragging();
        }
    }
}
