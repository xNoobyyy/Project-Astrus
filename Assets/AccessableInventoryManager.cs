using System;
using System.Linq;
using Logic;
using Logic.Events;
using Player.Inventory.Slots;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Player.Inventory {
    /*
    Anleitung zur Einrichtung:
    1. Erstelle ein leeres GameObject in deiner Szene (z.B. "InventoryManager") und hänge dieses Script daran.
    2. Lege 5 UI-GameObjects (z.B. als Child-Objekte eines Canvas oder Panels) an und füge an jedes von ihnen das "AccessableSlot"-Script an.
    3. Ordne die 5 Slots in der gewünschten Reihenfolge (im Inventarzyklus) an.
    4. Weise im Inspector die 5 Slots dem Array "slots" zu (die Reihenfolge im Array bestimmt den Kreis, also z.B. [0] bis [4]).
    5. Stelle über die öffentlichen Variablen "mainSlotPosition", "previousSlotPosition" und "nextSlotPosition" sowie "mainSlotScale" und "secondarySlotScale" die Position und Größe der sichtbaren Slots ein.
    6. Starte das Spiel: Mit dem Mausrad wechselst du zwischen den Slots. Der Slot links bzw. rechts (bzw. oben und unten, je nach deinen Positionseinstellungen) wird dabei ein- oder ausgeblendet.
    */

    public class AccessableInventoryManager : MonoBehaviour {
        [Header("Slot Referenzen (genau 5 AccessableSlot Objekte zuweisen)")]
        public AccessableSlot[] slots; // Das Array MUSS exakt 5 Elemente enthalten

        public ItemSlot[] invenSlots;

        [SerializeField] private GameObject hints;

        [Header("UI-Einstellungen")]
        // Zielpositionen (in lokalen Koordinaten) für die 3 sichtbaren Slots
        public Vector2 mainSlotPosition = new(0, 0); // Hauptslot (z.B. mittig)

        public Vector2 previousSlotPosition = new(0, 50); // Slot oberhalb des Hauptslots
        public Vector2 nextSlotPosition = new(0, -50); // Slot unterhalb des Hauptslots

        // Zielskalierungen: Hauptslot größer, die anderen etwas kleiner
        public Vector3 mainSlotScale = Vector3.one;
        public Vector3 secondarySlotScale = new(0.8f, 0.8f, 0.8f);

        [Header("Animationseinstellungen")]
        public float animationSpeed = 10f; // Je höher der Wert, desto schneller die Animation

        public bool requireShift;
        private int currentIndex = 0; // Index des aktuell aktiven (Haupt-)Slots
        public AccessableSlot CurrentSlot => slots[currentIndex];
        public ItemSlot CurrentItemSlot => invenSlots[currentIndex];

        private void Start() {
            if (slots == null || slots.Length != 5) {
                Debug.LogError("Bitte weise dem InventoryManager genau 5 Slots zu!");
                return;
            }

            currentIndex = 0;
            UpdateActiveSlots();
        }

        private void OnEnable() {
            EventManager.Instance.Subscribe<PlayerMoveItemEvent>(OnPlayerMoveItem);
            EventManager.Instance.Subscribe<PlayerItemEvent>(OnPlayerItemPickup);
        }

        private void OnDisable() {
            EventManager.Instance.Unsubscribe<PlayerMoveItemEvent>(OnPlayerMoveItem);
            EventManager.Instance.Unsubscribe<PlayerItemEvent>(OnPlayerItemPickup);
        }

        private void Update() {
            if (LogicScript.Instance.watchOpen) return;

            // Überprüfe den Mausradinput zum Rotieren
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            var shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            if ((scroll > 0f && !requireShift && !shift) || (scroll > 0f && shift && requireShift)) {
                RotateForward();
            } else if ((scroll < 0f && !requireShift && !shift) || (scroll < 0f && shift && requireShift)) {
                RotateBackward();
            }

            // Berechne Indizes der sichtbaren Slots (vorher und nächst)
            var prePreviousIndex = (currentIndex - 2 + slots.Length) % slots.Length;
            var previousIndex = (currentIndex - 1 + slots.Length) % slots.Length;
            var nextIndex = (currentIndex + 1) % slots.Length;
            var nextNextIndex = (currentIndex + 2) % slots.Length;

            // Animiert die Position und Skalierung der drei aktiven Slots mittels Lerp
            AnimateSlot(slots[prePreviousIndex], previousSlotPosition - new Vector2(0, 50), new Vector2(0, 0));
            AnimateSlot(slots[previousIndex], previousSlotPosition, secondarySlotScale);
            AnimateSlot(slots[currentIndex], mainSlotPosition, mainSlotScale);
            AnimateSlot(slots[nextNextIndex], nextSlotPosition + new Vector2(0, 50), new Vector2(0, 0));
            AnimateSlot(slots[nextIndex], nextSlotPosition, secondarySlotScale);
        }

        private void OnPlayerMoveItem(PlayerMoveItemEvent e) {
            if (invenSlots.Contains(e.Slot)) {
                UpdateSlots();
                UpdateSlots();
            }

            if (e.Slot == invenSlots[currentIndex]) {
                EventManager.Instance.Trigger(new PlayerChangeHeldItemEvent(e.Item, requireShift));
            }
        }

        private void OnPlayerItemPickup(PlayerItemEvent e) {
            if (invenSlots.Contains(e.Slot)) {
                UpdateSlots();
                UpdateSlots();
            }

            if (e.Slot == invenSlots[currentIndex]) {
                EventManager.Instance.Trigger(new PlayerChangeHeldItemEvent(e.Item, requireShift));
            }
        }

        // Rotiert das Inventar vorwärts (Mausrad nach oben/vorne)
        void RotateForward() {
            currentIndex = (currentIndex + 1) % slots.Length;
            UpdateActiveSlots();
        }

        // Rotiert das Inventar rückwärts (Mausrad nach unten/hinten)
        void RotateBackward() {
            currentIndex = (currentIndex - 1 + slots.Length) % slots.Length;
            UpdateActiveSlots();
        }

        // Aktiviert nur die drei relevanten Slots (vorher, aktuell, nächst) und deaktiviert den Rest
        void UpdateActiveSlots() {
            var previousIndex = (currentIndex - 1 + slots.Length) % slots.Length;
            var nextIndex = (currentIndex + 1) % slots.Length;

            slots[previousIndex].gameObject.SetActive(true);
            slots[currentIndex].gameObject.SetActive(true);
            slots[nextIndex].gameObject.SetActive(true);

            EventManager.Instance.Trigger(new PlayerChangeHeldItemEvent(slots[currentIndex]?.Item, requireShift));
        }

        // Animiert einen Slot in Richtung der Zielposition und Zielskalierung mittels Lerp
        private void AnimateSlot(AccessableSlot slot, Vector2 targetPosition, Vector3 targetScale) {
            var rt = slot.GetComponent<RectTransform>();
            if (rt != null) {
                rt.anchoredPosition =
                    Vector2.Lerp(rt.anchoredPosition, targetPosition, Time.deltaTime * animationSpeed);
                rt.localScale = Vector3.Lerp(rt.localScale, targetScale, Time.deltaTime * animationSpeed);
            }
        }

        public void UpdateSlots() {
            for (var i = 0; i < slots.Length; i++) {
                Debug.Log($"Slot {i}: {invenSlots[i].Item}");
                slots[i].SetItem(invenSlots[i].Item);
            }
        }

        public void HideHints() {
            hints.SetActive(false);
        }

        public void ShowHints() {
            hints.SetActive(true);
        }
    }
}