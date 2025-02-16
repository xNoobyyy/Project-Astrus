using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Utils {
    public class AreaManager : MonoBehaviour {
        public static AreaManager Instance { get; private set; }

        public Volume popVolume;
        public Volume swampVolume;
        public Image fogImage;
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