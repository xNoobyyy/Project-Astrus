using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Utils {
    [RequireComponent(typeof(Collider2D))]
    public class Area : MonoBehaviour {
        private const float FadeDuration = 5.0f;

        public AreaType type;
        public AudioClip[] music;

        private readonly Dictionary<Volume, Coroutine> volumeFadeCoroutines = new();
        private Coroutine imageFadeCoroutine;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            PlayRandomMusic();

            FadeImageTo(AreaManager.Instance.fogImage,
                type is AreaType.Swamp or AreaType.City or AreaType.Plateau ? 1f : 0f, FadeDuration);

            FadeVolumeTo(AreaManager.Instance.swampVolume, type is AreaType.Swamp ? 1f : 0f, FadeDuration);
            FadeVolumeTo(AreaManager.Instance.popVolume,
                type is not (AreaType.Swamp or AreaType.City or AreaType.Jungle) ? 1f : 0f, FadeDuration);
            FadeVolumeTo(AreaManager.Instance.jungleVolume, type is AreaType.Jungle ? 1f : 0f, FadeDuration);
            FadeVolumeTo(AreaManager.Instance.cityVolume, type is AreaType.City ? 1f : 0f, FadeDuration);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            AudioManager.Instance.StopMusic();

            FadeImageTo(AreaManager.Instance.fogImage, 0f, FadeDuration);

            FadeVolumeTo(AreaManager.Instance.swampVolume, 0f, FadeDuration);
            FadeVolumeTo(AreaManager.Instance.popVolume, 0f, FadeDuration);
            FadeVolumeTo(AreaManager.Instance.jungleVolume, 0f, FadeDuration);
            FadeVolumeTo(AreaManager.Instance.cityVolume, 0f, FadeDuration);
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

        private void FadeVolumeTo(Volume volume, float targetWeight, float duration) {
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

        private void FadeImageTo(Image image, float targetAlpha, float duration) {
            if (imageFadeCoroutine != null) {
                StopCoroutine(imageFadeCoroutine);
            }

            imageFadeCoroutine = StartCoroutine(FadeImage(image, targetAlpha, duration));
        }

        private void PlayRandomMusic() {
            AudioManager.Instance.PlayMusic(music[UnityEngine.Random.Range(0, music.Length)],
                onMusicEnd: PlayRandomMusic);
        }
    }

    public enum AreaType {
        Swamp,
        Beach,
        Grasslands,
        Plateau,
        City,
        Starter,
        Jungle,
    }
}