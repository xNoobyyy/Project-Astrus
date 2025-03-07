using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Creatures;
using Items;
using Items.Items;
using Logic.Events;
using Objects;
using Objects.Placeables;
using Player.Inventory;
using TextDisplay;
using UnityEngine;
using Utils;
using Tree = Objects.Tree;

namespace Player {
    public class PlayerItem : MonoBehaviour {
        private static readonly int AttackDirection = Animator.StringToHash("attack_direction");

        public static PlayerItem Instance { get; private set; }

        [SerializeField] public Collider2D attackCollider;
        [SerializeField] public Bow.Bow bowContainer;
        [SerializeField] public Bow.Bow bowContainerShift;
        [SerializeField] public GameObject arrowPrefab;

        [SerializeField] private BoxCollider2D boatPreview;
        [SerializeField] private GameObject boatPrefab;
        [SerializeField] private GameObject boatHint;

        [NonSerialized] public Camera mainCamera;
        [NonSerialized] public Animator animator;

        private Rigidbody2D rb;

        public bool InBoat { get; set; }

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
            EventManager.Instance.Subscribe<PlayerMoveEvent>(OnPlayerMove);
        }

        private void OnDisable() {
            EventManager.Instance.Unsubscribe<PlayerChangeHeldItemEvent>(OnPlayerChangeHeldItem);
            EventManager.Instance.Unsubscribe<PlayerMoveEvent>(OnPlayerMove);
        }

        private void OnPlayerMove(PlayerMoveEvent e) {
            if (!InBoat) return;
            if (ColliderManager.Instance == null || ColliderManager.Instance.Land == null) return;

            var nearestPoint = ColliderManager.Instance.Land
                .Select(land => PlaceableBoat.CustomClosestPoint(land, transform.position))
                .OrderBy(point => Vector2.Distance(point, transform.position))
                .FirstOrDefault();

            if (nearestPoint == default || Vector2.Distance(nearestPoint, transform.position) > 10f) {
                boatHint.SetActive(false);
            } else {
                boatHint.SetActive(true);
            }
        }

        private void Update() {
            if (TextDisplayManager.Instance.textDisplay.isDialogueActive) return;
            if (LogicScript.Instance.WatchOpen) return;

            var zCoord = mainCamera.WorldToScreenPoint(transform.position).z;
            var mousePosition = Input.mousePosition;
            mousePosition.z = zCoord;

            var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            if (CheckBoat(worldPosition)) return;

            if (Input.GetMouseButtonDown(0)) {
                // Left Click
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

                    var boat = colliders
                        .Where(c => c.GetComponent<PlaceableBoat>() != null ||
                                    c.GetComponentInParent<PlaceableBoat>() != null)
                        .Select(c => c.GetComponent<PlaceableBoat>() ?? c.GetComponentInParent<PlaceableBoat>())
                        .FirstOrDefault();

                    if (boat != null) {
                        if (Vector2.Distance(boat.transform.position, transform.position) > 10f) return;

                        boat.OnInteract(transform);
                        return;
                    }

                    if (tree != null) {
                        if (Vector2.Distance(tree.trigger.ClosestPoint(transform.position), transform.position) >
                            5f) return;

                        tree.Chop(1);
                        Chop();
                        return;
                    }

                    if (ore != null) {
                        if (Vector2.Distance(ore.trigger.ClosestPoint(transform.position), transform.position) >
                            5f) return;

                        ore.Break(1);
                        Chop();
                        return;
                    }
                }
            } else if (Input.GetMouseButtonDown(1)) {
                // Right Click
                var currentItem = LogicScript.Instance.accessableInventoryManager2.CurrentSlot.Item;

                currentItem?.OnUse(transform, worldPosition, ClickType.Right);
            }
        }

        private bool CheckBoat(Vector2 worldPosition) {
            if (LogicScript.Instance.accessableInventoryManager.CurrentSlot.Item is Boat boat) {
                if (Vector2.Distance(transform.position, worldPosition) > 10f) {
                    HideBoatPreview();
                    return false;
                }

                boatPreview.transform.position = worldPosition;

                if (!PlaceableBoat.IsPlaceable(boatPreview)) {
                    HideBoatPreview();
                    return false;
                }

                if (Input.GetMouseButtonDown(0)) {
                    boat.Amount--;
                    LogicScript.Instance.accessableInventoryManager.CurrentItemSlot.SetItem(boat.Amount == 0
                        ? null
                        : boat);
                    LogicScript.Instance.accessableInventoryManager.UpdateSlots();

                    Instantiate(boatPrefab, worldPosition, Quaternion.identity);
                    return true;
                }

                if (!boatPreview.gameObject.activeSelf) boatPreview.gameObject.SetActive(true);
                return false;
            }

            if (LogicScript.Instance.accessableInventoryManager2.CurrentSlot.Item is Boat boat2) {
                if (Vector2.Distance(transform.position, worldPosition) > 10f) {
                    HideBoatPreview();
                    return false;
                }

                boatPreview.transform.position = worldPosition;

                if (!PlaceableBoat.IsPlaceable(boatPreview)) {
                    HideBoatPreview();
                    return false;
                }

                if (Input.GetMouseButtonDown(1)) {
                    boat2.Amount--;
                    LogicScript.Instance.accessableInventoryManager2.CurrentItemSlot.SetItem(boat2.Amount == 0
                        ? null
                        : boat2);
                    LogicScript.Instance.accessableInventoryManager2.UpdateSlots();

                    Instantiate(boatPrefab, worldPosition, Quaternion.identity);
                    return true;
                }

                if (!boatPreview.gameObject.activeSelf) boatPreview.gameObject.SetActive(true);
                return false;
            }

            HideBoatPreview();
            return false;
        }

        public void Chop() {
            IsChopping = true;
            StartCoroutine(FinishChop());
        }

        private IEnumerator FinishChop() {
            yield return new WaitForSeconds(1);
            IsChopping = false;
        }

        private void HideBoatPreview() {
            if (boatPrefab.gameObject.activeSelf) boatPreview.gameObject.SetActive(false);
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