using System.Collections;
using UnityEngine;

namespace Utils.WhiteFlash {
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteFlashEffect : MonoBehaviour {
        private static readonly int Overlay = Shader.PropertyToID("_Overlay");
        private Material mat;

        private void Awake() {
            mat = GetComponent<SpriteRenderer>().material;
        }

        public void StartWhiteFlash(float fadeInTime = 0.1f, float fadeOutTime = 0.25f) {
            StopAllCoroutines();
            StartCoroutine(WhiteFlashRoutine(fadeInTime, fadeOutTime));
        }

        private IEnumerator WhiteFlashRoutine(float fadeInTime, float fadeOutTime) {
            float overlayAmount;
            var elapsed = 0f;

            const float maxOverlay = 0.5f;

            // Fade In
            while (elapsed < fadeInTime) {
                overlayAmount = Mathf.Lerp(0f, maxOverlay, elapsed / fadeInTime);
                mat.SetFloat(Overlay, overlayAmount);
                elapsed += Time.deltaTime;
                yield return null;
            }

            mat.SetFloat(Overlay, maxOverlay);

            // Fade Out
            elapsed = 0f;
            while (elapsed < fadeOutTime) {
                overlayAmount = Mathf.Lerp(maxOverlay, 0f, elapsed / fadeOutTime);
                mat.SetFloat(Overlay, overlayAmount);
                elapsed += Time.deltaTime;
                yield return null;
            }

            mat.SetFloat(Overlay, 0f);
        }
    }
}