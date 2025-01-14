using System;
using Items;
using Items.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Inventory {
    public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IDropHandler {
        public Item Item { get; private set; }

        public GameObject itemRenderer;

        public void SetItem(Item item) {
            Item = item;
            
            var itemRenderImageComponent = itemRenderer.GetComponent<Image>();

            if (Item == null) {
                itemRenderImageComponent.enabled = false;
            } else {
                itemRenderImageComponent.enabled = true;
                itemRenderImageComponent.GetComponent<Image>().sprite = Item.Icon;
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (Item != null) return;
            PlayerInventory.Instance.SetItem(
                Array.IndexOf(PlayerInventory.Instance.slots, this),
                new IronPickaxe());
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (Item == null) return;

            InventoryScreen.Instance.StartDragging(this);
        }

        // DO NOT DELETE
        public void OnDrag(PointerEventData eventData) { }

        public void OnDrop(PointerEventData eventData) {
            if (InventoryScreen.Instance.DraggingFrom == null) return;

            var draggingItemSlot = Array.IndexOf(
                PlayerInventory.Instance.slots,
                InventoryScreen.Instance.DraggingFrom
            );

            var thisItemSlot = Array.IndexOf(
                PlayerInventory.Instance.slots,
                this
            );

            var currentItem = Item;

            PlayerInventory.Instance.SetItem(
                thisItemSlot,
                InventoryScreen.Instance.DraggingFrom.Item
            );

            PlayerInventory.Instance.SetItem(
                draggingItemSlot,
                currentItem
            );

            InventoryScreen.Instance.ResetDragging();
        }
    }
}