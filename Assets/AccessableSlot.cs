using System;
using Items;
using Items.Items;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Inventory {
    public class AccessableSlot : MonoBehaviour, IPointerClickHandler{
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