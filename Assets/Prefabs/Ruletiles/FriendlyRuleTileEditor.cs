#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Prefabs.Ruletiles {
    [CustomEditor(typeof(FriendlyRuleTile))]
    [CanEditMultipleObjects]
    public class AdvancedRuleTileEditor : RuleTileEditor {
        public Texture2D anyIcon;
        public Texture2D specifiedIcon;
        public Texture2D nothingIcon;

        public override void RuleOnGUI(Rect rect, Vector3Int position, int neighbor) {
            switch (neighbor) {
                case FriendlyRuleTile.Neighbor.Any:
                    GUI.DrawTexture(rect, anyIcon);
                    return;
                case FriendlyRuleTile.Neighbor.Specified:
                    GUI.DrawTexture(rect, specifiedIcon);
                    return;
                case FriendlyRuleTile.Neighbor.Nothing:
                    GUI.DrawTexture(rect, nothingIcon);
                    return;
            }

            base.RuleOnGUI(rect, position, neighbor);
        }
    }
}
#endif