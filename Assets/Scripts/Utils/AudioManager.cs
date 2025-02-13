using UnityEngine;
using System;
using System.Collections;

namespace Utils {
    using UnityEngine;

    public class AudioManager : MonoBehaviour {
        public static AudioManager Instance;
        private AudioSource audioSource;
        private Action onMusicEndCallback;
        private Coroutine fadeOutCoroutine;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }

            audioSource = GetComponent<AudioSource>();
        }

        public void PlayMusic(AudioClip clip, float volume = 1.0f, Action onMusicEnd = null) {
            if (audioSource.clip == clip) return;
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();

            onMusicEndCallback = onMusicEnd;
            StartCoroutine(CheckMusicEnd());
        }

        private IEnumerator CheckMusicEnd() {
            yield return new WaitUntil(() => !audioSource.isPlaying);
            yield return new WaitForSeconds(5);

            onMusicEndCallback?.Invoke();
        }

        public void StopMusic() {
            StopCoroutine(CheckMusicEnd());
            onMusicEndCallback = null;

            if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = StartCoroutine(FadeOut());

            audioSource.Stop();
        }

        private IEnumerator FadeOut(float fadeDuration = 4.0f) {
            var startVolume = audioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime) {
                audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }

            audioSource.volume = 0;
            audioSource.Stop();
        }

        public void SetVolume(float volume) {
            audioSource.volume = volume;
        }
    }
}