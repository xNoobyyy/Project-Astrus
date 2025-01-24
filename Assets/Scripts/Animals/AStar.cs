using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Animals {
    /// <summary>
    /// AStar modified to implement LazyTheta*, an any-angle pathfinding approach.
    /// </summary>
    public class AStar {
        private Animal animal;

        /// <summary>
        /// Initializes the AStar (LazyTheta*) with a reference to the Animal.
        /// </summary>
        public AStar(Animal animal) {
            this.animal = animal;
        }

        /// <summary>
        /// Finds a path from 'position' to 'target' using LazyTheta*.
        /// Returns a list of grid positions (Vector2Int) if a path is found; otherwise, null.
        /// </summary>
        public List<Vector2Int> FindPath(Vector2 position, Vector2 target) {
            var start = Vector2Int.CeilToInt(position);
            var goal = Vector2Int.CeilToInt(target);

            var openSet = new List<Vector2Int> { start };
            var closedSet = new HashSet<Vector2Int>();

            // Dictionaries for costs and path reconstruction
            var gCost = new Dictionary<Vector2Int, float>();
            var hCost = new Dictionary<Vector2Int, float>();
            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

            // Initialize costs for the start node
            gCost[start] = 0f;
            hCost[start] = Heuristic(start, goal);

            while (openSet.Count > 0) {
                // Sort by F = G + H
                openSet.Sort((a, b) => (gCost[a] + hCost[a]).CompareTo(gCost[b] + hCost[b]));
                var current = openSet[0];

                // Remove current from openSet
                openSet.Remove(current);

                if (cameFrom.TryGetValue(current, out var parent) && parent != current &&
                    HasLineOfSight(parent, current)) {
                    var costViaParent = gCost[parent] + Heuristic(parent, current);
                    if (costViaParent < gCost[current]) {
                        gCost[current] = costViaParent;
                        cameFrom[current] = parent;
                    }
                }

                // If current is goal after potential shortcut, reconstruct path
                if (current == goal)
                    return ReconstructPath(cameFrom, current);

                closedSet.Add(current);

                // Standard A*-style neighbor expansion
                foreach (var neighbor in GetNeighbors(current)) {
                    if (!IsWalkable(neighbor) || closedSet.Contains(neighbor))
                        continue;

                    gCost.TryAdd(neighbor, Mathf.Infinity);

                    var costToNeighbor = gCost[current] + Heuristic(current, neighbor);

                    if (!(costToNeighbor < gCost[neighbor])) continue;

                    cameFrom[neighbor] = current;
                    gCost[neighbor] = costToNeighbor;
                    hCost[neighbor] = Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }

            // No path found
            return null;
        }

        /// <summary>
        /// Checks if a grid cell is walkable (no obstacles).
        /// </summary>
        private bool IsWalkable(Vector2Int position) {
            var colliders = Physics2D.OverlapBoxAll(position, new Vector2(1, 1), 0f);
            return colliders.All(collider => !collider.CompareTag("Obstacle")) && animal.area.OverlapPoint(position);
        }

        /// <summary>
        /// Heuristic function: Euclidean distance between two grid points.
        /// </summary>
        private static float Heuristic(Vector2Int a, Vector2Int b) {
            return Vector2Int.Distance(a, b);
        }

        /// <summary>
        /// Retrieves 8-directional neighbors around the given grid position.
        /// </summary>
        private static IEnumerable<Vector2Int> GetNeighbors(Vector2Int current) {
            yield return current + new Vector2Int(1, 0);
            yield return current + new Vector2Int(-1, 0);
            yield return current + new Vector2Int(0, 1);
            yield return current + new Vector2Int(0, -1);
            yield return current + new Vector2Int(1, 1);
            yield return current + new Vector2Int(-1, -1);
            yield return current + new Vector2Int(1, -1);
            yield return current + new Vector2Int(-1, 1);
        }

        /// <summary>
        /// Reconstructs path by tracing back from the 'current' node to the start.
        /// </summary>
        private static List<Vector2Int>
            ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current) {
            var path = new List<Vector2Int> { current };
            while (cameFrom.ContainsKey(current)) {
                current = cameFrom[current];
                path.Add(current);
            }

            path.Reverse();
            return path;
        }

        /// <summary>
        /// Performs a simple Bresenham-like check for obstacles between two points.
        /// </summary>
        private bool HasLineOfSight(Vector2Int start, Vector2Int end) {
            var x0 = start.x;
            var y0 = start.y;
            var x1 = end.x;
            var y1 = end.y;
            var dx = Mathf.Abs(x1 - x0);
            var dy = -Mathf.Abs(y1 - y0);

            var sx = (x0 < x1) ? 1 : -1;
            var sy = (y0 < y1) ? 1 : -1;

            var error = dx + dy;
            while (true) {
                if (!IsWalkable(new Vector2Int(x0, y0))) return false;
                if (x0 == x1 && y0 == y1) return true;

                var e2 = 2 * error;
                if (e2 >= dy) {
                    if (x0 == x1) return true;
                    error += dy;
                    x0 += sx;
                }

                if (e2 > dx) continue;
                if (y0 == y1) return true;

                error += dx;
                y0 += sy;
            }
        }
    }
}