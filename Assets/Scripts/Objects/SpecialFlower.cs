﻿using System;
using System.Collections;
using Items;
using UnityEngine;
using Utils;

namespace Objects {
    public class SpecialFlower : IdentificatedInteractable {
        private const int RespawnTime = 300000; // 5 minutes

        public long DestroyedAt { get; private set; }
        public bool IsDestroyed => DestroyedAt != -1;
        public override long InteractedAt => DestroyedAt;

        private Coroutine respawnCoroutine;
        private SpriteRenderer spriteRenderer;
        private new Collider2D collider;
        [SerializeField] private new ParticleSystem particleSystem;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();
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

        public void Destroy() {
            if (IsDestroyed) return;
            if (respawnCoroutine != null) {
                StopCoroutine(respawnCoroutine);
                respawnCoroutine = null;
            }

            DestroyedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            spriteRenderer.color = Color.white.WithAlpha(0f);
            particleSystem.gameObject.SetActive(false);
            collider.enabled = false;

            ItemManager.Instance.DropItem(new Items.Items.SpecialFlower(), transform.position);
            respawnCoroutine = StartCoroutine(RespawnTimer());
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

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player") || IsDestroyed) return;

            Destroy();
        }
    }
}