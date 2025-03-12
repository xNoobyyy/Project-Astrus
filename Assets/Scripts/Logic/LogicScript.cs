using Player.Inventory;
using TextDisplay;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic {
    public class LogicScript : MonoBehaviour {
        public static LogicScript Instance { get; private set; }

        public AccessableInventoryManager accessableInventoryManager;
        public AccessableInventoryManager accessableInventoryManager2;
        public InventoryScreen inventoryScreen;
        public Watch watch;
        public OpeningIcon openingIcon;
        public GameObject pauseScreen;
        public bool watchOpen;
        public bool iconOpened;
        public Canvas computerScreen;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            inventoryScreen.Close();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape) && !watchOpen) {
                if (Mathf.Approximately(Time.timeScale, 1.0f)) {
                    pauseScreen.SetActive(true);
                    Time.timeScale = 0f;
                } else {
                    Time.timeScale = 1f;
                    pauseScreen.SetActive(false);
                }

                return;
            }
            
            if (computerScreen.gameObject.activeSelf) return;
            if (TextDisplayManager.Instance.textDisplay.isDialogueActive) return;

            if (Input.GetKeyDown(KeyCode.E) && !watchOpen) {
                OpenWatch();
            }

            if (Input.GetKeyDown(KeyCode.E) && watchOpen && !iconOpened && !openingIcon.animationActive) {
                CloseWatch();
            }

            if (Input.GetKeyDown(KeyCode.E) && iconOpened) {
                watch.ReverseAnimation();
            }
        }

        private void OpenWatch() {
            watch.Open();
            accessableInventoryManager.HideHints();
            accessableInventoryManager2.HideHints();
            Time.timeScale = 0f;
            //WatchOpen = true;
        }

        private void CloseWatch() {
            watch.Close();
            accessableInventoryManager.ShowHints();
            accessableInventoryManager2.ShowHints();
            Time.timeScale = 1f;
            //WatchOpen = !watch.closed();
        }
    }
}