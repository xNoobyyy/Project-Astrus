using UnityEngine;

namespace Utils {
    public static class ColorUtils {
        public static Color WithAlpha(this Color color, float alpha) {
            color.a = alpha;
            return color;
        }
    }
}