using System;
using Items;
using Items.Items;
using Items.Items.AxeItems;
using Player.Inventory.Slots;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Inventory {
    public class AxeSlot : ItemSlot, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
        /// <summary>
        /// Setzt das Item in diesem Slot. Akzeptiert nur null oder Items vom Typ PickaxeItem.
        /// Wird ein anderes Item übergeben, wird es ignoriert.
        /// </summary>
        /// <param name="item">Das zu setzende Item</param>
        public GameObject panel;

        public new void SetItem(Item item) {
            // Falls das Item nicht null ist und kein PickaxeItem, wird es abgelehnt.
            if (item != null && !(item is AxeItem)) {
                panel.SetActive(true);
                return;
            }

            panel.SetActive(false);
            // Aufruf der Basisimplementierung für die visuelle Darstellung.
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
            if (InventoryScreen.Instance.DraggingFrom == null) {
                panel.SetActive(true);
                return;
            }

            InventoryScreen.Instance.ResetDragging();
        }

        /// <summary>
        /// Wird während des Drag-Vorgangs aufgerufen. (Muss vorhanden sein – bleibt hier leer)
        /// </summary>
        /// <param name="eventData">Informationen zum Drag-Event</param>
        public new void OnDrag(PointerEventData eventData) { } // DO NOT DELETE

        /// <summary>
        /// Wird aufgerufen, wenn ein Item auf diesen Slot gedroppt wird.
        /// Akzeptiert werden nur Items vom Typ AxeItem.
        /// Andernfalls wird das Item in den vorherigen Slot zurückgestellt.
        /// </summary>
        /// <param name="eventData">Informationen zum Drop-Event</param>
        public new void OnDrop(PointerEventData eventData) {
            if (InventoryScreen.Instance.DraggingFrom?.Item is not AxeItem) {
                InventoryScreen.Instance.ResetDragging();
                return;
            }

            base.OnDrop(eventData);
        }
    }
}