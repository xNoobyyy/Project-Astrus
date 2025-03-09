using System;
using System.Collections;
using Items;
using Items.Items;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Objects {
    public class Chest : IdentificatedInteractable {
        private const float RespawnTime = 600000; // 10 minutes

        [SerializeField] private Sprite closed;
        [SerializeField] private Sprite open;
        [SerializeField] public Collider2D trigger;
        [SerializeField] private new Light2D light;

        private SpriteRenderer spriteRenderer;
        private Coroutine respawnCoroutine;

        public long OpenedAt { get; private set; }
        public bool IsOpen => OpenedAt != -1;
        public override long InteractedAt => OpenedAt;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();

            OpenedAt = -1;
        }

        private void OnEnable() {
            if (!IsOpen) return;
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
            
            OpenedAt = timestamp;
            respawnCoroutine = StartCoroutine(RespawnTimer());
        }

        private IEnumerator RespawnTimer() {
            var currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var timeLeft = RespawnTime - (currentTime - OpenedAt);

            if (timeLeft > 0) {
                yield return new WaitForSeconds(timeLeft / 1000f);
            }

            Respawn();
        }

        public void Open() {
            spriteRenderer.sprite = open;
            light.enabled = false;

            OpenedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            ItemManager.Instance.DropItem(new Domilitant(1), transform.position);
            ItemManager.Instance.DropItem(new Domilitant(1), transform.position);

            spriteRenderer.sprite = open;

            StartCoroutine(RespawnTimer());
        }

        public void Respawn() {
            if (respawnCoroutine != null) {
                StopCoroutine(respawnCoroutine);
                respawnCoroutine = null;
            }

            spriteRenderer.sprite = closed;
            light.enabled = true;

            OpenedAt = -1;
        }

        private new void OnValidate() {
            base.OnValidate();
            
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = closed;
        }
    }
}