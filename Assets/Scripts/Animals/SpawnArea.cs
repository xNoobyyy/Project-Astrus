using System.Collections.Generic;
using UnityEngine;

namespace Animals {
    [RequireComponent(typeof(PolygonCollider2D))]
    public class SpawnArea : MonoBehaviour {
        private List<SpawnPoint> spawnPoints = new();

        private PolygonCollider2D polygonCollider;

        private void Awake() {
            polygonCollider = GetComponent<PolygonCollider2D>();
        }

        private void Start() {
            spawnPoints = new List<SpawnPoint>(GetComponentsInChildren<SpawnPoint>());

            foreach (var spawnPoint in spawnPoints) {
                spawnPoint.Area = polygonCollider;
                spawnPoint.Spawn();
            }
        }
    }
}