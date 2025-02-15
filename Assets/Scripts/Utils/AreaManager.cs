using UnityEngine;
using UnityEngine.Rendering;

namespace Utils {
    public class AreaManager : MonoBehaviour {
        public static AreaManager Instance { get; private set; }

        public Volume popVolume;
        public Volume swampVolume;
        public Canvas fogCanvas;
        public Volume jungleVolume;
        public Volume cityVolume;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }
    }
}