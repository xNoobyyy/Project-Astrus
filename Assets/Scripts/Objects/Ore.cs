using System;
using System.Collections;
using Items;
using Items.Items;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects {
    public class Ore : MonoBehaviour {
        private const int RespawnTime = 300000; // 5 minutes
        private const int Health = 6;

        [SerializeField] public PolygonCollider2D trigger;
        [SerializeField] private ParticleSystem damageParticles;
        [SerializeField] private OreType type;
        [SerializeField] private GameObject[] disableOnDestroy;
        [SerializeField] private int requiredPickPower;

        private SpriteRenderer spriteRenderer;

        public int Damage { get; private set; }
        public long DestroyedAt { get; private set; }
        public bool IsDestroyed => DestroyedAt != -1;

        private Coroutine respawnCoroutine;
        private new Collider2D collider;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();

            Damage = 0;
            DestroyedAt = -1;
        }

        private void OnEnable() {
            if (!IsDestroyed) return;
            if (respawnCoroutine != null) {
                StopCoroutine(respawnCoroutine);
                respawnCoroutine = null;
            }

            respawnCoroutine = StartCoroutine(RespawnTimer());
        }

        private void OnDisable() {
            if (respawnCoroutine == null) return;
            StopCoroutine(respawnCoroutine);
            respawnCoroutine = null;
        }

        public void Break(int pickPower) {
            if (IsDestroyed || pickPower < requiredPickPower) return;

            Damage += pickPower;
            damageParticles.Play();
            if (Damage < Health) return;

            Destroy();
            Damage = 0;
        }

        public void Destroy() {
            DestroyedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            spriteRenderer.color = Color.white.WithAlpha(0f);

            switch (type) {
                case OreType.Stone:
                    ItemManager.Instance.DropItem(new Stone(UnityEngine.Random.Range(1, 4)), transform.position);
                    break;
                case OreType.Iron:
                    ItemManager.Instance.DropItem(new Iron(UnityEngine.Random.Range(1, 3)), transform.position);
                    break;
                case OreType.Glomtom:
                    ItemManager.Instance.DropItem(new Glomtom(1), transform.position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(type.ToString());
            }

            foreach (var obj in disableOnDestroy) {
                obj.SetActive(false);
            }

            collider.enabled = false;

            respawnCoroutine = StartCoroutine(RespawnTimer());
        }

        private IEnumerator RespawnTimer() {
            var currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var timeLeft = RespawnTime - (currentTime - DestroyedAt);

            if (timeLeft > 0) {
                yield return new WaitForSeconds(timeLeft / 1000f);
            }

            Respawn();
        }

        public void Respawn() {
            if (respawnCoroutine != null) {
                StopCoroutine(respawnCoroutine);
                respawnCoroutine = null;
            }

            DestroyedAt = -1;
            spriteRenderer.color = Color.white;

            foreach (var obj in disableOnDestroy) {
                obj.SetActive(true);
            }

            collider.enabled = true;
        }
    }

    public enum OreType {
        Stone,
        Iron,
        Glomtom
    }
}