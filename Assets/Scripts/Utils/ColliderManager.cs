using System;
using UnityEngine;

namespace Utils {
    public class ColliderManager : MonoBehaviour {
        public static ColliderManager Instance { get; private set; }

        [SerializeField] private PolygonCollider2D[] land;
        [SerializeField] private GameObject swampLakesContainer;
        [SerializeField] private PolygonCollider2D[] cliffs;

        public PolygonCollider2D[] Land => land;
        public PolygonCollider2D[] Cliffs => cliffs;
        public PolygonCollider2D[] SwampLakes { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            SwampLakes = swampLakesContainer.GetComponentsInChildren<PolygonCollider2D>();
        }
    }
}