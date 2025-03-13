using System.Collections;
using System.Collections.Generic;
using Items;
using Items.Items;
using Logic.Events;
using UnityEngine;
using Utils;
using Utils.WhiteFlash;
using Random = UnityEngine.Random;

namespace Creatures {
    public class Boss : MonoBehaviour, IAttackable {
        private static readonly int Direction = Animator.StringToHash("Direction");

        [SerializeField] public Transform playerSpawn;
        [SerializeField] private GameObject zombiePrefab;
        [SerializeField] private BoxCollider2D area;
        [SerializeField] private AudioClip bossSound;

        public bool Entered { get; set; }

        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private List<CreatureBase> zombies;

        private float health = 100f;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            zombies = new List<CreatureBase>();
        }

        private void OnEnable() {
            EventManager.Instance.Subscribe<PlayerDeathEvent>(OnPlayerDeath);
            EventManager.Instance.Subscribe<CreatureDeathEvent>(OnCreatureDeath);
            EventManager.Instance.Subscribe<PlayerMoveEvent>(OnPlayerMove);
        }

        public void Start() {
            AudioManager.Instance.PlayMusic(bossSound);
            for (var i = 0; i < 10; i++) StartCoroutine(SpawnZombieAfterDelay(i));
        }

        private void OnDisable() {
            EventManager.Instance.Unsubscribe<PlayerDeathEvent>(OnPlayerDeath);
            EventManager.Instance.Unsubscribe<CreatureDeathEvent>(OnCreatureDeath);
            EventManager.Instance.Unsubscribe<PlayerMoveEvent>(OnPlayerMove);

            StopAllCoroutines();

            foreach (var zombie in zombies) zombie.Kill();
        }

        private void OnPlayerDeath(PlayerDeathEvent e) {
            gameObject.SetActive(false);
        }

        private void OnCreatureDeath(CreatureDeathEvent e) {
            if (!zombies.Contains(e.Creature)) return;

            zombies.Remove(e.Creature);
            StartCoroutine(SpawnZombieAfterDelay(5f));
        }

        private void OnPlayerMove(PlayerMoveEvent e) {
            var v = e.To - (Vector2)transform.position;

            // directions: 0 north, 1 east, 2 south, 3 west
            int direction;
            if (Mathf.Abs(v.x) > Mathf.Abs(v.y)) {
                direction = v.x > 0 ? 1 : 3;
            } else {
                direction = v.y > 0 ? 0 : 2;
            }

            animator.SetInteger(Direction, direction);
        }

        private IEnumerator SpawnZombieAfterDelay(float delay) {
            yield return new WaitForSeconds(delay);

            SpawnZombie();
        }

        private void SpawnZombie() {
            var spawnPoint = new Vector2(Random.Range(area.bounds.min.x, area.bounds.max.x),
                Random.Range(area.bounds.min.y, area.bounds.max.y));

            var zombie = Instantiate(zombiePrefab, spawnPoint, Quaternion.identity, area.transform);

            zombies.Add(zombie.GetComponent<CreatureBase>());
        }

        public void OnAttack(Transform attacker, float damage) {
            health -= damage;
            GetComponent<SpriteFlashEffect>().StartWhiteFlash();

            if (health > 0) return;
            Kill();
        }

        private void Kill() {
            spriteRenderer.enabled = false;
            for (var i = 0; i < 50; i++) {
                ItemManager.Instance.DropItem(new Astrus(), transform.position);
            }

            foreach (var zombie in zombies) zombie.Kill();
        }
    }
}