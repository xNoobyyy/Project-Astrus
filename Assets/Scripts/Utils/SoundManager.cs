using System;
using System.Linq;
using UnityEngine;

namespace Utils {
    using System.Collections.Generic;
    using UnityEngine;

    public class SoundManager : MonoBehaviour {
        public static SoundManager Instance;

        [System.Serializable]
        public class SoundEntry {
            public SoundEffect soundEffect;
            public AudioClip clip;
        }

        public List<SoundEntry> soundEntries = new();

        private readonly Dictionary<SoundEffect, AudioClip> soundDictionary = new();
        private readonly List<AudioSource> audioSources = new();
        private const int InitialPoolSize = 10;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                InitializeAudioPool();
                InitializeSoundDictionary();
            } else {
                Destroy(gameObject);
            }
        }

        private void InitializeSoundDictionary() {
            soundDictionary.Clear();
            foreach (var entry in soundEntries.Where(entry => !soundDictionary.ContainsKey(entry.soundEffect))) {
                soundDictionary[entry.soundEffect] = entry.clip;
            }
        }

        private void InitializeAudioPool() {
            for (var i = 0; i < InitialPoolSize; i++) {
                CreateNewAudioSource();
            }
        }

        private AudioSource CreateNewAudioSource() {
            var newAudioSource = new GameObject("PooledAudioSource");
            newAudioSource.transform.SetParent(transform);
            var source = newAudioSource.AddComponent<AudioSource>();
            audioSources.Add(source);
            return source;
        }

        private AudioSource GetAvailableAudioSource() {
            foreach (var source in audioSources.Where(source => !source.isPlaying)) {
                return source;
            }

            return CreateNewAudioSource();
        }

        public void PlaySound(SoundEffect soundEffect, float volume, float pitch) {
            if (!soundDictionary.ContainsKey(soundEffect)) {
                Debug.LogWarning($"SoundEffect {soundEffect} not found in dictionary!");
                return;
            }

            var source = GetAvailableAudioSource();
            source.clip = soundDictionary[soundEffect];
            source.volume = volume;
            source.pitch = pitch;
            source.Play();
        }

        public void PlaySound(SoundEffect soundEffect, float volume = 1f) {
            PlaySound(soundEffect, volume, Random.Range(0.8f, 1.2f));
        }
    }

    public enum SoundEffect { }
}