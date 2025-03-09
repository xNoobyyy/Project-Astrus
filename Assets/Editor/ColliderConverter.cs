using UnityEditor;
using UnityEngine;

namespace Editor {
    public class ColliderConverter : EditorWindow {
        [MenuItem("Tools/Convert BoxCollider to BoxCollider2D")]
        private static void ConvertBoxColliders() {
            foreach (var go in Selection.gameObjects) {
                var colliders = go.GetComponentsInChildren<BoxCollider>(true);

                foreach (var bc in colliders) {
                    var obj = bc.gameObject;

                    // Copy existing properties
                    var center = bc.center;
                    var size = bc.size;
                    var isTrigger = bc.isTrigger;

                    // Remove old collider
                    DestroyImmediate(bc);

                    // Add 2D collider
                    var bc2d = obj.AddComponent<BoxCollider2D>();
                    bc2d.offset = new Vector2(center.x, center.y);
                    bc2d.size = new Vector2(size.x, size.y);
                    bc2d.isTrigger = isTrigger;
                }
            }

            Debug.Log("Conversion Complete.");
        }
    }
}