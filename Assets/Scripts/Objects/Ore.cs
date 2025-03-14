﻿using System;
using System.Collections;
using Items;
using Items.Items;
using UnityEngine;
using Utils;

namespace Objects {
    public class Ore : IdentificatedInteractable {
        private const int RespawnTime = 300000; // 5 minutes
        private const int Health = 6;

        [SerializeField] public PolygonCollider2D trigger;
        [SerializeField] private ParticleSystem damageParticles;
        [SerializeField] private OreType type;
        [SerializeField] private int requiredPickPower;

        private SpriteRenderer spriteRenderer;

        public int Damage { get; private set; }
        public long DestroyedAt { get; private set; }
        public bool IsDestroyed => DestroyedAt != -1;
        public override long InteractedAt => DestroyedAt;

        private Coroutine respawnCoroutine;
        private Collider2D polyCollider;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            polyCollider = GetComponent<PolygonCollider2D>();

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

        public override void SetInteractedAt(long timestamp) {
            if (respawnCoroutine != null && isActiveAndEnabled) StopCoroutine(respawnCoroutine);

            DestroyedAt = timestamp;
            
            if (!isActiveAndEnabled) return;
            respawnCoroutine = StartCoroutine(RespawnTimer());
        }

        public void Break(int pickPower) {
            if (IsDestroyed || pickPower < requiredPickPower) return;

            SoundManager.Instance.PlaySound(type == OreType.Glomtom ? SoundEffect.HitGlomtom : SoundEffect.HitOre);

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
                    ItemManager.Instance.DropItem(new Stone(UnityEngine.Random.Range(3, 6)), transform.position);
                    break;
                case OreType.Iron:
                    ItemManager.Instance.DropItem(new Iron(UnityEngine.Random.Range(2, 4)), transform.position);
                    break;
                case OreType.Glomtom:
                    ItemManager.Instance.DropItem(new Glomtom(), transform.position);
                    break;
                case OreType.Coal:
                    ItemManager.Instance.DropItem(new Coal(UnityEngine.Random.Range(3, 6)), transform.position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(type.ToString());
            }

            polyCollider.enabled = false;

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

            polyCollider.enabled = true;
        }
    }

    public enum OreType {
        Stone,
        Iron,
        Glomtom,
        Coal
    }
}