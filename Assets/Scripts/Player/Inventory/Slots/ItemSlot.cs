using System;
using Items;
using Items.Items;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Inventory {
    public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler,
        IDropHandler {
        public Item Item { get; private set; }

        public GameObject itemRenderer;
        public GameObject itemAmountRenderer;
        public TMP_Text itemAmountText;

        public void SetItem(Item item) {
            Item = item;

            var itemRenderImageComponent = itemRenderer.GetComponent<Image>();
            var itemRenderAnimatorComponent = itemRenderer.GetComponent<Animator>();
            

            if (Item == null) {
                if (Item is ResourceItem resourceItem) {
                    resourceItem.Amount = 0;
                }
                itemRenderer.SetActive(false);
                itemAmountRenderer.SetActive(false);
            } else {
                
                itemRenderer.SetActive(true);
                if (itemRenderer.GetComponent<Animator>() == null) {
                    itemRenderImageComponent.sprite = Item.Icon;
                    itemRenderImageComponent.preserveAspect = true;
                } else {
                    itemRenderImageComponent.sprite = Item.Icon;
                    itemRenderImageComponent.preserveAspect = true;
                    itemRenderAnimatorComponent.runtimeAnimatorController = Item.AnimatedIcon;
                    
                    
                }

                if (Item is ResourceItem resourceItem) {
                    itemAmountRenderer.SetActive(true);
                    itemAmountText.outlineWidth = 0.2f;
                    itemAmountText.text = resourceItem.Amount.ToString();
                } else {
                    itemAmountRenderer.SetActive(false);
                }
            }
        }

        public void UpdateDisplay() {
            SetItem(Item);
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (Item != null) return;

            switch (eventData.button) {
                case PointerEventData.InputButton.Left:
                    if (itemRenderer.GetComponent<Animator>() == null) {
                        PlayerInventory.Instance.SetItem(
                            Array.IndexOf(PlayerInventory.Instance.Slots, this),
                            new Stick(1));
                    } else {
                        PlayerInventory.Instance.SetItem(
                            Array.IndexOf(PlayerInventory.Instance.Slots, this),
                            new Stick(1));
                    }

                    break;
                case PointerEventData.InputButton.Right:
                    if (itemRenderer.GetComponent<Animator>() == null) {
                        PlayerInventory.Instance.SetItem(
                            Array.IndexOf(PlayerInventory.Instance.Slots, this),
                            new Iron(1));
                    } else {
                        PlayerInventory.Instance.SetItem(
                            Array.IndexOf(PlayerInventory.Instance.Slots, this),
                            new Iron(1));
                    }
                    break;
            }
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (Item == null) return;

            InventoryScreen.Instance.StartDragging(this);
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (InventoryScreen.Instance.DraggingFrom == null) return;

            InventoryScreen.Instance.ResetDragging();
        }

        public void OnDrag(PointerEventData eventData) { } // DO NOT DELETE

        public void OnDrop(PointerEventData eventData) {
            if (InventoryScreen.Instance.DraggingFrom == null) return;

            if (InventoryScreen.Instance.DraggingFrom == this) {
                InventoryScreen.Instance.ResetDragging();
                return;
            }
            
            if (InventoryScreen.Instance.DraggingFrom.Item is ResourceItem draggedResourceItem &&
                Item is ResourceItem resourceItem && draggedResourceItem.GetType() == resourceItem.GetType() &&
                resourceItem.Amount < resourceItem.MaxAmount) {
                var left = resourceItem.MaxAmount - resourceItem.Amount;

                if (left >= draggedResourceItem.Amount) {
                    resourceItem.SetAmount(resourceItem.Amount + draggedResourceItem.Amount);
                    UpdateDisplay();
                    InventoryScreen.Instance.DraggingFrom.SetItem(null);
                } else {
                    resourceItem.SetAmount(resourceItem.MaxAmount);
                    UpdateDisplay();

                    draggedResourceItem.SetAmount(draggedResourceItem.Amount - left);
                    InventoryScreen.Instance.DraggingFrom.UpdateDisplay();
                }
            } else {
                var currentItem = Item;

                SetItem(InventoryScreen.Instance.DraggingFrom.Item);
                InventoryScreen.Instance.DraggingFrom.SetItem(currentItem);
            }

            InventoryScreen.Instance.ResetDragging();
        }

        public void clearSlot() {
            if (Item is ResourceItem resourceItem) {
                resourceItem.Amount = 0;
            }
            Item = null;
            var itemRenderImageComponent = itemRenderer.GetComponent<Image>();
            var itemRenderAnimatorComponent = itemRenderer.GetComponent<Animator>();

            itemRenderImageComponent = null;
            itemRenderAnimatorComponent = null;
            
            itemRenderer.SetActive(false);
            itemAmountRenderer.SetActive(false);
        }

        public void fillSlot(Item item) {
            Item = item;
            PlayerInventory.Instance.SetItem(
                Array.IndexOf(PlayerInventory.Instance.Slots, this),
                item);
        }
    }
}