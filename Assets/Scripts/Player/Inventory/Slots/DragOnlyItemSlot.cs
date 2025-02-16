using System;
using Items;
using Items.Items;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Inventory {
    // Diese Klasse erbt von ItemSlot
    public class DragOnlyItemSlot : ItemSlot, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {

        // Überschreibt den Klick-Handler, sodass keine neuen Items in den Slot gesetzt werden.
        public new void OnPointerClick(PointerEventData eventData) {
            // Hier wird absichtlich nichts gemacht,
            // sodass beim Klick (wenn der Slot leer ist) kein neues Item hinzugefügt wird.
        }

        // Überschreibt den Drop-Handler, sodass keine Items in diesen Slot gezogen werden können.
        public new void OnDrop(PointerEventData eventData) {
            // Setze einfach den Dragging-Zustand zurück,
            // ohne das Item zu übernehmen.
            InventoryScreen.Instance.ResetDragging();
        }
    }
}