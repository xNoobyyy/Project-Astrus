using UnityEngine;

namespace Utils.Caves {
    public class CaveManager : MonoBehaviour {
        public GameObject overworld;
        public GameObject caves;

        public bool IsCave { get; private set; }

        public static CaveManager Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
                return;
            }

            IsCave = false;
            overworld.SetActive(!IsCave);
            caves.SetActive(IsCave);
        }

        public void ToggleCave() {
            IsCave = !IsCave;
            overworld.SetActive(!IsCave);
            caves.SetActive(IsCave);
        }
    }
}