using UnityEngine;

namespace Utils.Caves {
    public class Cave : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                CaveManager.Instance.ToggleCave();
            }
        }
    }
}