using System;
using System.Collections;
using Items;
using Player;
using UnityEngine;

namespace Objects {
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class Tree : MonoBehaviour {
        private static readonly int Destroyed = Animator.StringToHash("Destroyed");
        private const int RespawnTime = 300000; // 5 minutes
        private const int Health = 3;

        [SerializeField] private Sprite full;
        [SerializeField] private Sprite destroyed;
        [SerializeField] private PolygonCollider2D trigger;

        private SpriteRenderer spriteRenderer;
        private Camera mainCamera;
        private Animator animator;

        public int Damage { get; private set; }
        public long DestroyedAt { get; private set; }
        public bool IsDestroyed => DestroyedAt != -1;

        private Coroutine respawnCoroutine;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            mainCamera = Camera.main;
            animator = GetComponent<Animator>();
        }

        private void OnEnable() {
            if (IsDestroyed) {
                if (respawnCoroutine != null) {
                    StopCoroutine(respawnCoroutine);
                    respawnCoroutine = null;
                }

                respawnCoroutine = StartCoroutine(RespawnTimer());
            }
        }

        private void OnDisable() {
            if (respawnCoroutine != null) {
                StopCoroutine(respawnCoroutine);
                respawnCoroutine = null;
            }
        }

        private void Update() {
            if (LogicScript.Instance.accessableInventoryManager.CurrentSlot.Item is not AxeItem axeItem) return;
            if (IsDestroyed || !Input.GetMouseButtonDown(0)) return;
            if (Vector2.Distance(PlayerMovement.Instance.transform.position, transform.position) > 3f) return;

            // Convert mouse position to world position
            var zCoord = mainCamera.WorldToScreenPoint(transform.position).z;
            var mousePosition = Input.mousePosition;
            mousePosition.z = zCoord;

            var pos = mainCamera.ScreenToWorldPoint(mousePosition);
            if (!trigger.OverlapPoint(pos)) return;

            // In case multiple trees are on the same spot
            var colliders = Physics2D.OverlapPointAll(pos);
            foreach (var c in colliders) {
                if (!c.CompareTag("Obstacle")) continue;
                var tree = c.GetComponent<Tree>();
                if (tree == null) return;
                if (tree.IsDestroyed) continue;

                return;
            }

            Chop(axeItem.ChopPower);
        }

        public void Chop(int chopPower) {
            Damage += chopPower;
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