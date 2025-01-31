using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Logic {
    public class Pathfinding : MonoBehaviour {
        private const int LOS_RADIUS = 8;

        private Vector2Int playerPosition;
        public Vector2Int[] Tiles { get; private set; }

        private void OnEnable() {
            EventManager.OnPlayerMove += OnPlayerMove;
        }

        private void OnDisable() {
            EventManager.OnPlayerMove -= OnPlayerMove;
        }

        private void OnPlayerMove(Vector2 from, Vector2 to) {
            if (Vector2Int.RoundToInt(from) == Vector2Int.RoundToInt(to)) return;

            playerPosition = Vector2Int.RoundToInt(to);
            Tiles = GetLosTiles();
        }

        private Vector2Int[] GetLosTiles() {
            var losTiles = new List<Vector2Int>();
            var nonLosTiles = new List<Vector2Int>();

            var tilesToCheck = new List<Vector2Int>();
            for (var x = -LOS_RADIUS; x <= LOS_RADIUS; x++) {
                for (var y = -LOS_RADIUS; y <= LOS_RADIUS; y++) {
                    if (x * x + y * y > LOS_RADIUS * LOS_RADIUS) continue;
                    tilesToCheck.Add(playerPosition + new Vector2Int(x, y));
                }
            }

            tilesToCheck.Sort((a, b) =>
                Vector2Int.Distance(playerPosition, a).CompareTo(Vector2Int.Distance(playerPosition, b)));

            foreach (var tile in tilesToCheck) {
                var checkTiles = GetLineTiles(tile, playerPosition);
                var checkedTiles = new List<Vector2Int>();

                foreach (var checkTile in checkTiles.Where(checkTile =>
                             !losTiles.Contains(checkTile) && !nonLosTiles.Contains(checkTile))) {
                    checkedTiles.Add(checkTile);
                    if (!Physics2D.OverlapBoxAll(checkTile, Vector2.one, 0f)
                            .Any(c => c.CompareTag("Obstacle"))) continue;

                    nonLosTiles.AddRange(checkedTiles);
                    checkedTiles.Clear();
                }

                losTiles.AddRange(checkedTiles);
            }

            return losTiles.ToArray();
        }

        private static List<Vector2Int> GetLineTiles(Vector2Int start, Vector2Int end) {
            var result = new List<Vector2Int>();

            var dx = Mathf.Abs(end.x - start.x);
            var dy = Mathf.Abs(end.y - start.y);
            var sx = start.x < end.x ? 1 : -1;
            var sy = start.y < end.y ? 1 : -1;
            var err = dx - dy;

            var x0 = start.x;
            var y0 = start.y;

            while (true) {
                result.Add(new Vector2Int(x0, y0));

                if (x0 == end.x && y0 == end.y)
                    break;

                var e2 = err * 2;
                if (e2 > -dy) {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx) {
                    err += dx;
                    y0 += sy;
                }
            }

            return result;
        }
    }
}