using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Utils {
    public class AreaManager : MonoBehaviour {
        public static AreaManager Instance { get; private set; }

        public Volume popVolume;
        public Volume swampVolume;
        public Image fogImage;
        public Volume jungleVolume;
        public Volume cityVolume;
        public Volume caveVolume;

        private readonly Dictionary<Volume, Coroutine> volumeFadeCoroutines = new();
        private Coroutine imageFadeCoroutine;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private IEnumerator FadeVolume(Volume volume, float targetWeight, float duration) {
            var startWeight = volume.weight;
            var elapsed = 0f;
            while (elapsed < duration) {
                volume.weight = Mathf.Lerp(startWeight, targetWeight, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            volume.weight = targetWeight;
            volumeFadeCoroutines.Remove(volume);
        }

        public void FadeVolumeTo(Volume volume, float targetWeight, float duration) {
            if (volumeFadeCoroutines.TryGetValue(volume, out var existing)) {
                StopCoroutine(existing);
            }

            var newCoroutine = StartCoroutine(FadeVolume(volume, targetWeight, duration));
            volumeFadeCoroutines[volume] = newCoroutine;
        }

        private IEnumerator FadeImage(Image image, float targetAlpha, float duration) {
            var startAlpha = image.color.a;
            var elapsed = 0f;
            while (elapsed < duration) {
                image.color = new Color(image.color.r, image.color.g, image.color.b,
                    Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration));
                elapsed += Time.deltaTime;
                yield return null;
            }

            image.color = new Color(image.color.r, image.color.g, image.color.b, targetAlpha);
            imageFadeCoroutine = null;
        }

        public void FadeImageTo(Image image, float targetAlpha, float duration) {
            if (imageFadeCoroutine != null) {
                StopCoroutine(imageFadeCoroutine);
            }

            imageFadeCoroutine = StartCoroutine(FadeImage(image, targetAlpha, duration));
        }
    }
}