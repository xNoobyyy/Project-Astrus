using Logic.Events;
using UnityEngine;

namespace Utils {
    [RequireComponent(typeof(Collider2D))]
    public class Area : MonoBehaviour {
        private const float FadeDuration = 5.0f;

        public AreaType type;
        public AudioClip[] music;

        public bool secondary;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;
            if (secondary) {
                EventManager.Instance.Trigger(new PlayerAreaEnterEvent(this));
                return;
            }

            PlayRandomMusic();

            AreaManager.Instance.FadeImageTo(AreaManager.Instance.fogImage,
                type is AreaType.Swamp or AreaType.City or AreaType.Plateau or AreaType.Cave ? 1f : 0f, FadeDuration);

            AreaManager.Instance.FadeVolumeTo(AreaManager.Instance.swampVolume, type is AreaType.Swamp ? 1f : 0f,
                FadeDuration);
            AreaManager.Instance.FadeVolumeTo(AreaManager.Instance.popVolume,
                type is not (AreaType.Swamp or AreaType.City or AreaType.Jungle or AreaType.Cave) ? 1f : 0f,
                FadeDuration);
            AreaManager.Instance.FadeVolumeTo(AreaManager.Instance.jungleVolume, type is AreaType.Jungle ? 1f : 0f,
                FadeDuration);
            AreaManager.Instance.FadeVolumeTo(AreaManager.Instance.cityVolume, type is AreaType.City ? 1f : 0f,
                FadeDuration);
            AreaManager.Instance.FadeVolumeTo(AreaManager.Instance.caveVolume, type is AreaType.Cave ? 1f : 0f,
                FadeDuration);

            AreaManager.Instance.LastOrCurrentArea = this;

            EventManager.Instance.Trigger(new PlayerAreaEnterEvent(this));
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;
            if (secondary) return;
            if (!AudioManager.Instance) return;

            AudioManager.Instance.StopMusic();

            AreaManager.Instance.FadeImageTo(AreaManager.Instance.fogImage, 0f, FadeDuration);

            AreaManager.Instance.FadeVolumeTo(AreaManager.Instance.swampVolume, 0f, FadeDuration);
            AreaManager.Instance.FadeVolumeTo(AreaManager.Instance.popVolume, 0f, FadeDuration);
            AreaManager.Instance.FadeVolumeTo(AreaManager.Instance.jungleVolume, 0f, FadeDuration);
            AreaManager.Instance.FadeVolumeTo(AreaManager.Instance.cityVolume, 0f, FadeDuration);
            AreaManager.Instance.FadeVolumeTo(AreaManager.Instance.caveVolume, 0f, FadeDuration);
        }

        private void PlayRandomMusic() {
            AudioManager.Instance.PlayMusic(music[Random.Range(0, music.Length)],
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
        Cave,
        Labor,
        PlateauSight,
        JungleCave,
        LilyPads
    }
}