using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Creatures;
using Items;
using Logic.Events;
using Objects;
using Player.Inventory;
using TextDisplay;
using UnityEngine;
using Tree = Objects.Tree;

namespace Player {
    public class PlayerItem : MonoBehaviour {
        private static readonly int AttackDirection = Animator.StringToHash("attack_direction");

        public static PlayerItem Instance { get; private set; }

        [SerializeField] public Collider2D attackCollider;
        [SerializeField] public Bow.Bow bowContainer;
        [SerializeField] public Bow.Bow bowContainerShift;
        [SerializeField] public GameObject arrowPrefab;

        [NonSerialized] public Camera mainCamera;
        [NonSerialized] public Animator animator;
        private Rigidbody2D rb;

        public bool IsAttacking { get; set; }
        [NonSerialized] public Vector2 attackDirection;

        public bool IsChopping { get; private set; }

        public bool IsBusy => IsAttacking || IsChopping;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
                return;
            }

            IsAttacking = false;

            mainCamera = Camera.main;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable() {
            EventManager.Instance.Subscribe<PlayerChangeHeldItemEvent>(OnPlayerChangeHeldItem);
        }

        private void OnDisable() {
            EventManager.Instance.Unsubscribe<PlayerChangeHeldItemEvent>(OnPlayerChangeHeldItem);
        }

        private void Update() {
            if (TextDisplayManager.Instance.textDisplay.isDialogueActive) return;
            if (LogicScript.Instance.WatchOpen) return;

            if (Input.GetMouseButtonDown(0)) {
                // Left Click
                var zCoord = mainCamera.WorldToScreenPoint(transform.position).z;
                var mousePosition = Input.mousePosition;
                mousePosition.z = zCoord;

                var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                var currentItem = LogicScript.Instance.accessableInventoryManager.CurrentSlot.Item;

                var colliders = Physics2D.OverlapPointAll(worldPosition);
                var chestCollider =
                    colliders.FirstOrDefault(c => c.GetComponent<Chest>() || c.GetComponentInParent<Chest>());

                if (chestCollider != null) {
                    var chest = chestCollider.GetComponent<Chest>() ?? chestCollider.GetComponentInParent<Chest>();

                    if (!chest.IsOpen &&
                        Vector2.Distance(chest.trigger.ClosestPoint(transform.position), transform.position) < 5f) {
                        chest.Open();
                        return;
                    }
                }

                if (currentItem != null) {
                    currentItem.OnUse(transform, worldPosition, ClickType.Left);
                } else {
                    if (IsBusy) return;

                    var treeColliders = Array.FindAll(colliders,
                        c => c.GetComponent<Tree>() != null || c.GetComponentInParent<Tree>() != null);
                    Array.Sort(treeColliders, (c1, c2) => c1.transform.position.y.CompareTo(c2.transform.position.y));
                    var trees = treeColliders.Select(c => c?.GetComponent<Tree>() ?? c?.GetComponentInParent<Tree>())
                        .ToArray();
                    var nonDestroyed = Array.FindAll(trees, t => !t.IsDestroyed);
                    var tree = nonDestroyed.Length > 0 ? nonDestroyed[0] : null;

                    var ore = colliders
                        .Where(c => c.GetComponent<Ore>() != null || c.GetComponentInParent<Ore>() != null)
                        .Select(c => c.GetComponent<Ore>() ?? c.GetComponentInParent<Ore>())
                        .FirstOrDefault(ore => !ore.IsDestroyed);

                    if (tree == null) {
                        if (ore == null) return;
                        if (Vector2.Distance(ore.trigger.ClosestPoint(transform.position), transform.position) >
                            5f) return;

                        ore.Break(1);
                        Chop();

                        return;
                    }

                    if (Vector2.Distance(tree.trigger.ClosestPoint(transform.position), transform.position) >
                        5f) return;

                    tree.GetComponent<Tree>()?.Chop(1);
                    Chop();
                }
            } else if (Input.GetMouseButtonDown(1)) {
                // Right Click
                var zCoord = mainCamera.WorldToScreenPoint(transform.position).z;
                var mousePosition = Input.mousePosition;
                mousePosition.z = zCoord;

                var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                var currentItem = LogicScript.Instance.accessableInventoryManager2.CurrentSlot.Item;

                currentItem?.OnUse(transform, worldPosition, ClickType.Right);
            }
        }

        public void Chop() {
            IsChopping = true;
            StartCoroutine(FinishChop());
        }

        private IEnumerator FinishChop() {
            yield return new WaitForSeconds(1);
            IsChopping = false;
        }

        private void Attack() {
            rb.AddForce(-attackDirection.normalized * 3f, ForceMode2D.Impulse);

            var colliders = new List<Collider2D>();
            Physics2D.OverlapCollider(attackCollider, new ContactFilter2D().NoFilter(), colliders);
            colliders.ForEach(c => c.GetComponent<IAttackable>()?.OnAttack(transform, 1));
        }

        private void AttackFinished() {
            animator.SetInteger(AttackDirection, 0);
            IsAttacking = false;
            attackDirection = Vector2.zero;
        }

        private void OnPlayerChangeHeldItem(PlayerChangeHeldItemEvent e) {
            if (e.Item is not BowItem bowItem) {
                if (bowContainer.gameObject.activeSelf && !e.Shift) bowContainer.gameObject.SetActive(false);
                if (bowContainerShift.gameObject.activeSelf && e.Shift) bowContainerShift.gameObject.SetActive(false);

                return;
            }

            if (e.Shift) {
                bowContainerShift.gameObject.SetActive(true);
                bowContainerShift.SetItem(bowItem);
            } else {
                bowContainer.gameObject.SetActive(true);
                bowContainer.SetItem(bowItem);
            }
        }

        public Coroutine StartThirdPartyCoroutine(IEnumerator coroutine) {
            return StartCoroutine(coroutine);
        }

        public AccessableInventoryManager GetInventoryManager(ClickType clickType) {
            return clickType switch {
                ClickType.Left => LogicScript.Instance.accessableInventoryManager,
                ClickType.Right => LogicScript.Instance.accessableInventoryManager2,
                _ => null
            };
        }
    }

    public enum ClickType {
        Left,
        Right
    }
}