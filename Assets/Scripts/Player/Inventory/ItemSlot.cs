using System;
using Items;
using Items.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Inventory {
    public class ItemSlot : MonoBehaviour {
        public Item Item { get; private set; }

        public GameObject itemRenderer;

        private void Start() { }

        public void SetItem(Item item) {
            Item = item;

            if (Item == null) {
                itemRenderer.SetActive(false);
            } else {
                itemRenderer.SetActive(true);
                itemRenderer.GetComponent<Image>().sprite = Item.Icon;
            }
        }

        private void OnMouseDown() {
            Debug.Log("ItemSlot.OnMouseDown()");
            
            if (Item != null) return;
            PlayerInventory.Instance.SetItem(
                Array.FindIndex(PlayerInventory.Instance.slots, slot => slot == this),
                new IronPickaxe());
        }
    }
}