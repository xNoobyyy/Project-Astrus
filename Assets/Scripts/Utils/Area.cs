using UnityEngine;

namespace Utils {
    [RequireComponent(typeof(Collider2D))]
    public class Area : MonoBehaviour {
        public enum AreaType {
            Swamp,
            Beach,
            Grasslands,
            Plateau,
            City,
            Starter
        }

        public AreaType type;
        public AudioClip[] music;
    }
}