using System;
using System.Collections;
using Items;
using Items.Items;
using Player;
using UnityEngine;
using Utils;

namespace Objects {
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class Tree : MonoBehaviour {
        private static readonly int Destroyed = Animator.StringToHash("Destroyed");
        private const int RespawnTime = 300000; // 5 minutes
        private const int Health = 6;

        [SerializeField] private Sprite full;
        [SerializeField] private Sprite destroyed;
        [SerializeField] public PolygonCollider2D trigger;
        [SerializeField] private ParticleSystem damageParticles;

        private SpriteRenderer spriteRenderer;
        private Animator animator;

        public int Damage { get; private set; }
        public long DestroyedAt { get; private set; }
        public bool IsDestroyed => DestroyedAt != -1;

        private Coroutine respawnCoroutine;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

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

        public void Chop(int chopPower) {
            if (IsDestroyed) return;

            Damage += chopPower;
            damageParticles.Play();
            if (Damage < Health) return;

            Destroy();
            Damage = 0;
        }

        public void Destroy() {
            DestroyedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (animator != null) {
                animator.SetBool(Destroyed, true);
            } else {
                spriteRenderer.sprite = destroyed;
            }

            var woodAmount = UnityEngine.Random.Range(1, 4);
            var stickAmount = UnityEngine.Random.Range(1, 4);

            ItemManager.Instance.DropItem(new Wood(woodAmount), transform.position);
            ItemManager.Instance.DropItem(new Stick(stickAmount), transform.position);

            if (AreaManager.Instance.LastOrCurrentArea?.type == AreaType.Jungle) {
                if (UnityEngine.Random.Range(0, 10) != 0) return;
                ItemManager.Instance.DropItem(new Liana(1), transform.position);
            }

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
            if (animator != null) {
                animator.SetBool(Destroyed, false);
            } else {
                spriteRenderer.sprite = full;
            }
        }

        private void OnValidate() {
            GetComponent<SpriteRenderer>().sprite = full;
        }
    }
}