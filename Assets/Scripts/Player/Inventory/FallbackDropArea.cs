﻿using System;
using Items;
using Items.DroppedItem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player.Inventory {
    public class FallbackDropArea : MonoBehaviour, IDropHandler {
        public void OnDrop(PointerEventData eventData) {
            if (InventoryScreen.Instance.DraggingFrom == null) return;

            var canvasCenter = InventoryScreen.Instance.Canvas.GetComponent<RectTransform>().rect.center;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                InventoryScreen.Instance.CanvasRect,
                eventData.position,
                InventoryScreen.Instance.Canvas.worldCamera,
                out var canvasPosition
            );

            Vector3 direction = canvasPosition - canvasCenter;
            direction.Normalize();

            var playerPosition = GameObject.FindWithTag("Player").transform.position;
            var dropPosition = playerPosition + direction * (DroppedItemAttractor.AttractionRadius + 0.1f);

            ItemManager.Instance.CreateDroppedItem(
                playerPosition,
                playerPosition,
                InventoryScreen.Instance.DraggingFrom.Item,
                dropPosition
            );

            var draggingItemSlot = Array.IndexOf(
                PlayerInventory.Instance.Slots,
                InventoryScreen.Instance.DraggingFrom
            );

            PlayerInventory.Instance.SetItem(draggingItemSlot, null);
            InventoryScreen.Instance.ResetDragging();
        }
    }
}