using Items;
using TMPro;
using UnityEngine;
using WatchAda.Quests;
using Image = UnityEngine.UI.Image;

namespace Player.Inventory {
    public class InventoryScreen : MonoBehaviour {
        public GameObject draggedItem;
        public Image draggedItemImage;
        public GameObject draggedItemAmount;
        public TMP_Text draggedItemAmountText;

        public ItemSlot DraggingFrom { get; private set; }

        public static InventoryScreen Instance { get; private set; }
        public Canvas Canvas { get; private set; }
        public RectTransform CanvasRect { get; private set; }

        private RectTransform draggedItemRect;

        public QuestLogic ql;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                return;
            }

            Destroy(gameObject);
        }

        private void Start() {
            //gameObject.SetActive(false);

            CanvasRect = GetComponent<RectTransform>();
            Canvas = GetComponent<Canvas>();
            draggedItemRect = draggedItem.GetComponent<RectTransform>();
        }

        private void Update() {
            if (ReferenceEquals(DraggingFrom, null)) return;

            Vector2 cursorPosition = Input.mousePosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                CanvasRect,
                cursorPosition,
                Canvas.worldCamera,
                out var canvasPosition
            );

            draggedItemRect.localPosition = canvasPosition;
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

            draggedItem.SetActive(true);
            draggedItemImage.sprite = itemSlot.Item.Icon;

            if (itemSlot.Item is ResourceItem resourceItem) {
                draggedItemAmount.SetActive(true);
                draggedItemAmountText.text = resourceItem.Amount.ToString();
            } else {
                draggedItemAmount.SetActive(false);
            }
        }

        public void ResetDragging() {
            DraggingFrom = null;
            draggedItem.SetActive(false);
        }

        void OnEnable() {
            ql.Slots();
        }
    }
}