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
        private Coroutine checkMusicEndCoroutine;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }

            audioSource = GetComponent<AudioSource>();
        }

        public void PlayMusic(AudioClip clip, float volume = 0.1f, Action onMusicEnd = null) {
            if (audioSource.clip == clip) return;
            StopMusic(onFinish: () => {
                audioSource.clip = clip;
                audioSource.volume = volume;
                audioSource.Play();

                onMusicEndCallback = onMusicEnd;
                checkMusicEndCoroutine = StartCoroutine(CheckMusicEnd());
            });
        }

        private IEnumerator CheckMusicEnd() {
            yield return new WaitUntil(() => !audioSource.isPlaying);
            yield return new WaitForSeconds(5);

            onMusicEndCallback?.Invoke();
        }

        public void StopMusic(Action onFinish = null) {
            StopCoroutine(checkMusicEndCoroutine);
            onMusicEndCallback = null;

            if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = StartCoroutine(FadeOut(onFinish: onFinish));
        }

        private IEnumerator FadeOut(float fadeDuration = 5.0f, Action onFinish = null) {
            var startVolume = audioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime) {
                audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }

            audioSource.volume = 0;
            audioSource.Stop();
            onFinish?.Invoke();
        }

        public void SetVolume(float volume) {
            audioSource.volume = volume;
        }
    }
}