using UnityEngine;

namespace Objects.Placeables {
    public interface IInteractable {
        void OnInteract(Transform player);
    }
}