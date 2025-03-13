using System;
using System.Collections;
using Items;
using UnityEngine;

namespace Objects {
    public class SpecialFlower : IdentificatedInteractable {
        private const int RespawnTime = 300000; // 5 minutes

        public long DestroyedAt { get; private set; }
        public bool IsDestroyed => DestroyedAt != -1;
        public override long InteractedAt => DestroyedAt;

        private Coroutine respawnCoroutine;
        private SpriteRenderer spriteRenderer;
        private new Collider2D collider;
        private new ParticleSystem particleSystem;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();
            particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        public override void SetInteractedAt(long timestamp) {
            StopCoroutine(respawnCoroutine);

            DestroyedAt = timestamp;
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
            collider.enabled = true;
            particleSystem.gameObject.SetActive(true);

            collider.enabled = true;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("Player") && !IsDestroyed) {
                SetInteractedAt(DateTimeOffset.Now.ToUnixTimeMilliseconds());
                spriteRenderer.color = Color.clear;
                collider.enabled = false;
                particleSystem.gameObject.SetActive(false);

                ItemManager.Instance.DropItem(new Items.Items.SpecialFlower(1), transform.position);
            }
        }
    }
}