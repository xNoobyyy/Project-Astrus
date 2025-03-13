using System.Collections.Generic;
using UnityEngine;

namespace Creatures {
    public class SpawnArea : MonoBehaviour {
        private List<SpawnPoint> spawnPoints = new();

        private void Start() {
            spawnPoints = new List<SpawnPoint>(GetComponentsInChildren<SpawnPoint>());

            foreach (var spawnPoint in spawnPoints) {
                spawnPoint.Spawn();
            }
        }
    }
}