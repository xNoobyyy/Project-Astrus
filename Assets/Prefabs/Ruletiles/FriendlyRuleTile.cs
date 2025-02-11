using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Prefabs.Ruletiles {
    [CreateAssetMenu]
    public class FriendlyRuleTile : RuleTile<FriendlyRuleTile.Neighbor> {
        [Header("Advanced Tile")]
        [Tooltip("If enabled, the tile will connect to these tiles too when the mode is set to \"This\"")]
        public bool alwaysConnect;

        [Tooltip("Tiles to connect to")] public TileBase[] tilesToConnect;

        [Space] [Tooltip("Check itself when the mode is set to \"any\"")]
        public bool checkSelf = true;

        public class Neighbor : RuleTile.TilingRuleOutput.Neighbor {
            public const int Any = 3;
            public const int Specified = 4;
            public const int Nothing = 5;
        }

        public override bool RuleMatch(int neighbor, TileBase tile) {
            return neighbor switch {
                TilingRuleOutput.Neighbor.This => Check_This(tile),
                TilingRuleOutput.Neighbor.NotThis => Check_NotThis(tile),
                Neighbor.Any => Check_Any(tile),
                Neighbor.Specified => Check_Specified(tile),
                Neighbor.Nothing => Check_Nothing(tile),
                _ => base.RuleMatch(neighbor, tile)
            };
        }

        /// <summary>
        /// Returns true if the tile is this, or if the tile is one of the tiles specified if always connect is enabled.
        /// </summary>
        /// <param name="tile">Neighboring tile to compare to</param>
        /// <returns></returns>
        private bool Check_This(TileBase tile) {
            if (!alwaysConnect) return tile == this;
            return tilesToConnect.Contains(tile) || tile == this;

            //.Contains requires "using System.Linq;"
        }

        /// <summary>
        /// Returns true if the tile is not this.
        /// </summary>
        /// <param name="tile">Neighboring tile to compare to</param>
        /// <returns></returns>
        private bool Check_NotThis(TileBase tile) {
            if (!alwaysConnect) return tile != this;
            return !tilesToConnect.Contains(tile) && tile != this;

            //.Contains requires "using System.Linq;"
        }

        /// <summary>
        /// Return true if the tile is not empty, or not this if the check self option is disabled.
        /// </summary>
        /// <param name="tile">Neighboring tile to compare to</param>
        /// <returns></returns>
        private bool Check_Any(TileBase tile) {
            if (checkSelf) return tile != null;
            return tile != null && tile != this;
        }

        /// <summary>
        /// Returns true if the tile is one of the specified tiles.
        /// </summary>
        /// <param name="tile">Neighboring tile to compare to</param>
        /// <returns></returns>
        private bool Check_Specified(TileBase tile) {
            return tilesToConnect.Contains(tile);
        }

        /// <summary>
        /// Returns true if the tile is empty.
        /// </summary>
        /// <param name="tile">
        ///     Neighboring tile to compare to
        /// </param>
        /// <returns></returns>
        private static bool Check_Nothing(TileBase tile) {
            return tile == null;
        }
    }
}