using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editor {
    public class TilemapFixer : EditorWindow {
        private Tilemap targetTilemap;
        private TileBase replacementTile;

        [MenuItem("Tools/Tilemap Fixer")]
        public static void ShowWindow() {
            GetWindow<TilemapFixer>("Tilemap Fixer");
        }

        private void OnGUI() {
            GUILayout.Label("Tilemap Fixer", EditorStyles.boldLabel);

            targetTilemap = (Tilemap)EditorGUILayout.ObjectField("Tilemap", targetTilemap, typeof(Tilemap), true);
            replacementTile =
                (TileBase)EditorGUILayout.ObjectField("Replacement Tile", replacementTile, typeof(TileBase), false);

            if (GUILayout.Button("Replace Missing Tiles") && targetTilemap != null && replacementTile != null) {
                ReplaceUnknownTiles();
            }
        }

        private void ReplaceUnknownTiles() {
            BoundsInt bounds = targetTilemap.cellBounds;

            Undo.RecordObject(targetTilemap, "Replace Missing Tiles"); // Enable undo functionality

            int replacedCount = 0;
            foreach (Vector3Int pos in bounds.allPositionsWithin) {
                TileBase tile = targetTilemap.GetTile(pos);
                Color tileColor = targetTilemap.GetColor(pos);

                if (tileColor != Color.white) // Check for missing tiles with a non-white color
                {
                    targetTilemap.SetTile(pos, replacementTile);
                    targetTilemap.SetColor(pos, Color.white);
                    replacedCount++;
                }
            }

            Debug.Log($"Replaced {replacedCount} missing or unknown tiles with the replacement tile.");
            targetTilemap.RefreshAllTiles();
        }
    }
}