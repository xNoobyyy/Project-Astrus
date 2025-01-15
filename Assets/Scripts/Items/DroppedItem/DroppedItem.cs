using System;
using Items.Items;
using UnityEngine;

namespace Items.DroppedItem {
    // use ItemManager#CreateDroppedItem to create a dropped item
    public class DroppedItem : MonoBehaviour {
        public Item Item { get; private set; }

        public void Initialize(Item item) {
            Item = item;
            GetComponent<SpriteRenderer>().sprite = Item.Icon;
        }
    }
}