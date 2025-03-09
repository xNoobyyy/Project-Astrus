using System;
using Items;
using Items.Items;
using Logic.Events;
using Player.Inventory.Slots;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Inventory {
    public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IDropHandler {
        public Item Item { get; private set; }
        private TextMeshProUGUI floatingText;
        private GameObject floatingTextObject;


        public GameObject itemRenderer;
        public GameObject itemAmountRenderer;
        public TMP_Text itemAmountText;


        private void Start() {
            var sibling = transform.parent.Find("ItemNameText");
            if (sibling != null) { } else {
                sibling = transform.parent.parent.Find("ItemNameText");
                Debug.Log("FloatingText wurde in der Geschwisterebene nicht gefunden.");
                if (sibling == null) return;
            }

            floatingText = sibling.GetComponent<TextMeshProUGUI>();
            floatingTextObject = sibling.gameObject;
            if (floatingText == null)
                Debug.LogError("Kein Text-Component an FloatingText gefunden.");
        }


        public void SetItem(Item item) {
            Item = item;

            var image = itemRenderer.GetComponent<Image>();
            var animator = itemRenderer.GetComponent<Animator>();

            if (Item == null) {
                if (Item is ResourceItem resourceItem) {
                    resourceItem.Amount = 0;
                }

                itemRenderer.SetActive(false);
                itemAmountRenderer.SetActive(false);
            } else {
                itemRenderer.SetActive(true);

                if (ReferenceEquals(Item.AnimatedIcon, null)) {
                    animator.runtimeAnimatorController = null;
                    image.sprite = Item.Icon;
                    image.preserveAspect = true;
                } else {
                    animator.runtimeAnimatorController = Item.AnimatedIcon;
                }

                if (Item is ResourceItem resourceItem) {
                    itemAmountRenderer.SetActive(true);
                    itemAmountText.text = resourceItem.Amount.ToString();
                } else {
                    itemAmountRenderer.SetActive(false);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (Item == null || floatingText == null) return;

            floatingText.text = Item.Name;
            floatingTextObject.transform.position = gameObject.transform.position + new Vector3(0, 30, 0);
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (floatingText == null || Item == null || floatingText.text != Item.Name) return;

            floatingText.text = "";
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
                            new Extric(1));
                    } else {
                        PlayerInventory.Instance.SetItem(
                            Array.IndexOf(PlayerInventory.Instance.Slots, this),
                            new Extric(1));
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

                    //EventManager.Instance.Trigger(new PlayerMoveItemEvent(Item, this));

                    InventoryScreen.Instance.DraggingFrom.SetItem(null);
                } else {
                    resourceItem.SetAmount(resourceItem.MaxAmount);
                    UpdateDisplay();

                    draggedResourceItem.SetAmount(draggedResourceItem.Amount - left);
                    InventoryScreen.Instance.DraggingFrom.UpdateDisplay();

                    //EventManager.Instance.Trigger(new PlayerMoveItemEvent(Item, this));
                }
            } else {
                var currentItem = Item;
                if (currentItem != null) {
                    switch (InventoryScreen.Instance.DraggingFrom) {
                        case AxeSlot when Item is not AxeItem:
                        case CombatSlot when Item is not CombatItem:
                        case ArmorSlot when Item is not ArmorItem:
                        case PickaxeSlot when Item is not PickaxeItem:
                            InventoryScreen.Instance.ResetDragging();
                            return;
                    }
                }

                SetItem(InventoryScreen.Instance.DraggingFrom.Item);
                InventoryScreen.Instance.DraggingFrom.SetItem(currentItem);

                //EventManager.Instance.Trigger(new PlayerMoveItemEvent(Item, this));
                //EventManager.Instance.Trigger(new PlayerMoveItemEvent(currentItem,
            //        InventoryScreen.Instance.DraggingFrom));
            }

            InventoryScreen.Instance.ResetDragging();
        }

        public void ClearSlot() {
            if (Item is ResourceItem resourceItem) {
                resourceItem.Amount = 0;
            }

            Item = null;
            var image = itemRenderer.GetComponent<Image>();
            var animator = itemRenderer.GetComponent<Animator>();

            image.sprite = null;
            animator.runtimeAnimatorController = null;

            itemRenderer.SetActive(false);
            itemAmountRenderer.SetActive(false);
        }

        public void FillSlot(Item item) {
            Item = item;
            PlayerInventory.Instance.SetItem(
                Array.IndexOf(PlayerInventory.Instance.Slots, this),
                item);
        }
    }
}