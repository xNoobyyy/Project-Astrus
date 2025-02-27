using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Inventory {
    public class AccessableSlot : MonoBehaviour {
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
                    itemAmountText.text = resourceItem.Amount.ToString();
                } else {
                    itemAmountRenderer.SetActive(false);
                }
            }
        }
    }
}