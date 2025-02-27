using System;
using UnityEngine;

namespace TextDisplay {
    public class TextDisplayManager : MonoBehaviour {
        public static TextDisplayManager Instance { get; private set; }

        public TextDisplay textDisplay;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
                return;
            }
        }
    }
}