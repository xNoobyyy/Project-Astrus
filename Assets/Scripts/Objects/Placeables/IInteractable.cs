using UnityEngine;

namespace Objects.Placeables {
    public abstract class Interactable : MonoBehaviour {
        public abstract void OnInteract(Transform player);
    }
}