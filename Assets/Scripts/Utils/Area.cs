using System;
using UnityEngine;

namespace Utils {
    [RequireComponent(typeof(Collider2D))]
    public class Area : MonoBehaviour {
        public AreaType type;
        public AudioClip[] music;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            PlayRandomMusic();
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            AudioManager.Instance.StopMusic();
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