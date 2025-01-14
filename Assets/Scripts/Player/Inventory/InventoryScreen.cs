using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player.Inventory {
    public class InventoryScreen : MonoBehaviour {
        public ItemSlot itemSlotPrefab;
        public GameObject itemSlotContainer;

        public ItemSlot DraggingFrom { get; private set; }

        public static InventoryScreen Instance { get; private set; }

        private RectTransform canvasRect;
        private Canvas canvas;
        private RectTransform draggedItemRendererRect;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            gameObject.SetActive(false);

            var playerInventory = PlayerInventory.Instance;
            for (var i = 0; i < 8; i++) {
                for (var j = 0; j < 4; j++) {
                    var itemSlot = Instantiate(itemSlotPrefab, itemSlotContainer.transform);
                    itemSlot.GetComponent<RectTransform>().localPosition = new Vector3(
                        i * 70,
                        j * -70,
                        0
                    );

                    playerInventory.slots[i + j * 8] = itemSlot;
                }
            }

            canvasRect = GetComponent<RectTransform>();
            canvas = GetComponent<Canvas>();
        }

        private void Update() {
            if (ReferenceEquals(DraggingFrom, null)) return;

            Vector2 cursorPosition = Input.mousePosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                cursorPosition,
                canvas.worldCamera,
                out var canvasPosition
            );

            draggedItemRendererRect.localPosition = canvasPosition;
        }

        public void Open() {
            gameObject.SetActive(true);
        }

        public void Close() {
            gameObject.SetActive(false);
        }

        public void Toggle() {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void StartDragging(ItemSlot itemSlot) {
            DraggingFrom = itemSlot;
            draggedItemRendererRect =
                Instantiate(itemSlot.itemRenderer, canvas.transform).GetComponent<RectTransform>();
        }

        public void ResetDragging() {
            DraggingFrom = null;
            Destroy(draggedItemRendererRect.gameObject);
        }
    }
}