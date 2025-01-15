using UnityEngine;

namespace Player.Inventory {
    public class InventoryInputs : MonoBehaviour {
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                InventoryScreen.Instance.Toggle();
            }
        }
    }
}