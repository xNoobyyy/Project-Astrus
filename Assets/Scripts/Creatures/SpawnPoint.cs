using System.Collections;
using Logic.Events;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures {
    public class SpawnPoint : MonoBehaviour {
        [SerializeField] private CreatureBase prefab;

        public PolygonCollider2D Area { get; set; }

        private CreatureBase currentCreature;

        private void OnEnable() {
            EventManager.Instance.Subscribe<CreatureDeathEvent>(OnCreatureDeath);
        }

        private void OnDisable() {
            EventManager.Instance.Unsubscribe<CreatureDeathEvent>(OnCreatureDeath);
        }

        public CreatureBase Spawn() {
            currentCreature = Instantiate(prefab, transform.position, Quaternion.identity, transform);
            currentCreature.Area = Area;
            return currentCreature;
        }

        private void OnCreatureDeath(CreatureDeathEvent e) {
            if (e.Creature != currentCreature) return;

            currentCreature = null;
            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn() {
            yield return new WaitForSeconds(Random.Range(60, 120));
            Spawn();
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position, 0.2f);

            Handles.color = Color.white;
            Handles.Label(
                transform.position + Vector3.up * 0.5f,
                "Spawn" + (prefab != null ? " (" + prefab.name + ")" : ""),
                new GUIStyle {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 10,
                    normal = {
                        textColor = Color.white
                    }
                }
            );
        }
    }
}