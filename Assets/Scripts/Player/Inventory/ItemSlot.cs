using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Inventory {
    public class ItemSlot : MonoBehaviour {
        
        public BaseItem item;
        
        public GameObject itemRenderer;

        private void Start() { }

        public void SetItem(BaseItem newItem) {
            item = newItem;
            
            if (item == null) {
                itemRenderer.SetActive(false);
            } else {
                itemRenderer.SetActive(true);
                itemRenderer.GetComponent<Image>().sprite = item.itemIcon;
            }
        }
    }
}