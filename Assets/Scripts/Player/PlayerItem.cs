using System;
using UnityEngine;

namespace Player {
    public class PlayerItem : MonoBehaviour {
        private Camera mainCamera;

        private void Start() {
            mainCamera = Camera.main;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                // Left Click
                var worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var currentItem = LogicScript.Instance.accessableInventoryManager.CurrentSlot.Item;

                if (currentItem != null) {
                    currentItem.OnUse(transform, worldPosition);
                } else {
                    // may break tree with hand
                    Debug.Log("No item selected");
                }
            } else if (Input.GetMouseButtonDown(1)) {
                // Right Click
                var worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var currentItem = LogicScript.Instance.accessableInventoryManager2.CurrentSlot.Item;

                currentItem?.OnUse(transform, worldPosition);
            }  
        }
    }
}