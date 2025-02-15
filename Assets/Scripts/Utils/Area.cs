using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Utils {
    [RequireComponent(typeof(Collider2D))]
    public class Area : MonoBehaviour {
        private const float FadeDuration = 2.0f;

        public AreaType type;
        public AudioClip[] music;

        private Coroutine canvasFadeCoroutine;

        private readonly Dictionary<Volume, Coroutine> volumeFadeCoroutines = new Dictionary<Volume, Coroutine>();

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            PlayRandomMusic();

            AreaManager.Instance.fogCanvas.enabled = type is AreaType.Swamp or AreaType.City;

            FadeVolumeTo(AreaManager.Instance.swampVolume, (type is AreaType.Swamp) ? 1f : 0f, FadeDuration);
            FadeVolumeTo(AreaManager.Instance.popVolume,
                (type is not (AreaType.Swamp or AreaType.City or AreaType.Jungle)) ? 1f : 0f, FadeDuration);
            FadeVolumeTo(AreaManager.Instance.jungleVolume, (type is AreaType.Jungle) ? 1f : 0f, FadeDuration);
            FadeVolumeTo(AreaManager.Instance.cityVolume, (type is AreaType.City) ? 1f : 0f, FadeDuration);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            AudioManager.Instance.StopMusic();

            AreaManager.Instance.fogCanvas.enabled = false;

            FadeVolumeTo(AreaManager.Instance.swampVolume, 0f, FadeDuration);
            FadeVolumeTo(AreaManager.Instance.popVolume, 0f, FadeDuration);
            FadeVolumeTo(AreaManager.Instance.jungleVolume, 0f, FadeDuration);
            FadeVolumeTo(AreaManager.Instance.cityVolume, 0f, FadeDuration);
        }

        private static IEnumerator FadeVolume(Volume volume, float targetWeight, float duration) {
            var startWeight = volume.weight;
            var elapsed = 0f;
            while (elapsed < duration) {
                volume.weight = Mathf.Lerp(startWeight, targetWeight, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            volume.weight = targetWeight;
        }

        private void FadeVolumeTo(Volume volume, float targetWeight, float duration) {
            if (volumeFadeCoroutines.TryGetValue(volume, out var existing)) {
                StopCoroutine(existing);
            }

            var newCoroutine = StartCoroutine(FadeVolume(volume, targetWeight, duration));
            volumeFadeCoroutines[volume] = newCoroutine;
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