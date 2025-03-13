using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Creatures;
using Items;
using Items.Items;
using Items.Items.ArmorItems;
using Logic;
using Logic.Events;
using Objects;
using Objects.Placeables;
using Player.Inventory;
using Player.Inventory.Slots;
using TextDisplay;
using UnityEngine;
using Utils;
using WatchAda.Quests;
using Tree = Objects.Tree;

namespace Player {
    public class PlayerItem : MonoBehaviour {
        private static readonly int AttackDirectionAnimator = Animator.StringToHash("attack_direction");

        public static PlayerItem Instance { get; private set; }

        [SerializeField] public Collider2D attackCollider;
        [SerializeField] public Bow.Bow bowContainer;
        [SerializeField] public Bow.Bow bowContainerShift;
        [SerializeField] public GameObject arrowPrefab;

        [SerializeField] private BoxCollider2D boatPreview;
        [SerializeField] private GameObject boatPrefab;
        [SerializeField] private GameObject boatHint;

        [SerializeField] private Transform vinePreview;
        [SerializeField] private GameObject vinePrefab;
        [SerializeField] private GameObject vineContainer;

        [SerializeField] private Collider2D computerCollider;
        [SerializeField] private Canvas computerScreen;

        [SerializeField] private Canvas swampMap;
        [SerializeField] private Canvas cityMap;

        [SerializeField] private SpriteRenderer spaceshipRenderer;
        [SerializeField] private Sprite fullSpaceship;

        [SerializeField] private Collider2D lakesCollider;

        [NonSerialized] public Camera MainCamera;
        [NonSerialized] public Animator Animator;

        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;

        public bool InBoat { get; set; }

        public bool Invisible { get; private set; }
        private Coroutine invisibleCoroutine;

        public bool IsAttacking { get; set; }
        [NonSerialized] public Vector2 AttackDirection;

        public bool IsChopping { get; private set; }

        public bool IsBusy => IsAttacking || IsChopping;

        public bool Finished { get; set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
                return;
            }

            IsAttacking = false;

            MainCamera = Camera.main;
            Animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable() {
            EventManager.Instance.Subscribe<PlayerChangeHeldItemEvent>(OnPlayerChangeHeldItem);
            EventManager.Instance.Subscribe<PlayerMoveEvent>(OnPlayerMove);
            EventManager.Instance.Subscribe<PlayerMoveItemEvent>(OnMoveItemEvent);
        }

        private void OnDisable() {
            EventManager.Instance.Unsubscribe<PlayerChangeHeldItemEvent>(OnPlayerChangeHeldItem);
            EventManager.Instance.Unsubscribe<PlayerMoveEvent>(OnPlayerMove);
            EventManager.Instance.Unsubscribe<PlayerMoveItemEvent>(OnMoveItemEvent);
        }

        private void OnMoveItemEvent(PlayerMoveItemEvent e) {
            if (e.Item is ExtricArmor && e.Slot is ArmorSlot) {
                lakesCollider.isTrigger = true;
            } else {
                if (lakesCollider.isTrigger) lakesCollider.isTrigger = false;
            }
        }

        private void OnPlayerMove(PlayerMoveEvent e) {
            if (!InBoat) {
                if (boatHint.activeSelf) boatHint.SetActive(false);
                return;
            }

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
            if (LogicScript.Instance.watchOpen) return;
            if (computerScreen.gameObject.activeSelf) return;
            if (swampMap.gameObject.activeSelf) return;
            if (cityMap.gameObject.activeSelf) return;

            var zCoord = MainCamera.WorldToScreenPoint(transform.position).z;
            var mousePosition = Input.mousePosition;
            mousePosition.z = zCoord;

            var worldPosition = MainCamera.ScreenToWorldPoint(mousePosition);

            if (CheckBoat(worldPosition)) return;
            if (CheckVine(worldPosition)) return;

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

                var creatures = colliders
                    .Where(c => c.GetComponent<CreatureBase>() != null ||
                                c.GetComponentInParent<CreatureBase>() != null)
                    .Select(c => c.GetComponent<CreatureBase>() ?? c.GetComponentInParent<CreatureBase>())
                    .ToArray();

                var spaceship = colliders
                    .FirstOrDefault(c => c.CompareTag("Spaceship"));

                var computer = colliders
                    .FirstOrDefault(c => c == computerCollider);

                if (spaceship != null && PlayerInventory.Instance.Slots.Any(slot => slot.Item is Astrus)) {
                    Finish();
                }

                if (computer != null &&
                    Vector2.Distance(computer.ClosestPoint(transform.position), transform.position) <= 5f) {
                    computerScreen.gameObject.SetActive(true);
                    computerScreen.GetComponentInChildren<Animator>().Play("computerscreen", 0, 0f);
                    Time.timeScale = 0f;
                    return;
                }

                if (boat != null && Vector2.Distance(boat.transform.position, transform.position) <= 10f) {
                    boat.OnInteract(transform);
                    return;
                }

                if (tree != null && currentItem is not AxeItem && currentItem is not BowItem &&
                    Vector2.Distance(tree.trigger.ClosestPoint(transform.position), transform.position) <= 5f) {
                    tree.Chop(1);
                    Chop();
                    return;
                }

                if (ore != null && currentItem is not PickaxeItem && currentItem is not BowItem &&
                    Vector2.Distance(ore.trigger.ClosestPoint(transform.position), transform.position) <= 5f) {
                    ore.Break(1);
                    Chop();
                    return;
                }

                if (currentItem is null or ResourceItem && creatures.Length > 0) {
                    if (Vector2.Distance(transform.position, worldPosition) <= 4f) {
                        foreach (var creature in creatures) {
                            creature.OnTouch();
                        }
                    }
                }

                if (currentItem != null) {
                    currentItem.OnUse(transform, worldPosition, ClickType.Left);
                    return;
                }
            } else if (Input.GetMouseButtonDown(1)) {
                // Right Click
                var currentItem = LogicScript.Instance.accessableInventoryManager2.CurrentSlot.Item;

                currentItem?.OnUse(transform, worldPosition, ClickType.Right);
            }
        }

        public void Finish() {
            Finished = true;
            spaceshipRenderer.sprite = fullSpaceship;
            QuestLogic.Instance.reparierenQuest.CompleteQuest();
            QuestLogic.Instance.nachHauseQuest.CompleteQuest();
        }

        public void MakeInvisible() {
            if (invisibleCoroutine != null) StopCoroutine(invisibleCoroutine);

            Invisible = true;
            spriteRenderer.color = Color.white.WithAlpha(0.5f);
            invisibleCoroutine = StartCoroutine(InvisibleTimer());
        }

        private IEnumerator InvisibleTimer() {
            yield return new WaitForSeconds(60f);
            spriteRenderer.color = Color.white;
            Invisible = false;

            invisibleCoroutine = null;
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

        private bool CheckVine(Vector2 worldPosition) {
            if (LogicScript.Instance.accessableInventoryManager.CurrentSlot.Item is Liana vine) {
                if (Vector2.Distance(transform.position, worldPosition) > 10f) {
                    HineVinePreview();
                    return false;
                }

                var placeableCollider = ColliderManager.Instance.Placeables
                    .FirstOrDefault(pc => pc.OverlapPoint(worldPosition));

                if (placeableCollider == null) {
                    HineVinePreview();
                    return false;
                }

                var placePosition = new Vector2(worldPosition.x, placeableCollider.bounds.center.y);
                vinePreview.position = placePosition;

                if (Input.GetMouseButtonDown(0)) {
                    vine.Amount--;
                    LogicScript.Instance.accessableInventoryManager.CurrentItemSlot.SetItem(vine.Amount == 0
                        ? null
                        : vine);
                    LogicScript.Instance.accessableInventoryManager.UpdateSlots();

                    Instantiate(vinePrefab, placePosition, Quaternion.identity, vineContainer.transform);
                    return true;
                }

                if (!vinePreview.gameObject.activeSelf) vinePreview.gameObject.SetActive(true);
                return false;
            }

            if (LogicScript.Instance.accessableInventoryManager2.CurrentSlot.Item is Liana vine2) {
                if (Vector2.Distance(transform.position, worldPosition) > 10f) {
                    HineVinePreview();
                    return false;
                }

                var placeableCollider = ColliderManager.Instance.Placeables
                    .FirstOrDefault(pc => pc.OverlapPoint(worldPosition));

                if (placeableCollider == null) {
                    HineVinePreview();
                    return false;
                }

                var placePosition = new Vector2(worldPosition.x, placeableCollider.bounds.center.y);
                vinePreview.position = placePosition;

                if (Input.GetMouseButtonDown(1)) {
                    vine2.Amount--;
                    LogicScript.Instance.accessableInventoryManager2.CurrentItemSlot.SetItem(vine2.Amount == 0
                        ? null
                        : vine2);
                    LogicScript.Instance.accessableInventoryManager2.UpdateSlots();

                    Instantiate(vinePrefab, placePosition, Quaternion.identity, vineContainer.transform);
                    return true;
                }

                if (!vinePreview.gameObject.activeSelf) vinePreview.gameObject.SetActive(true);
                return false;
            }

            HineVinePreview();
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
            if (boatPreview.gameObject.activeSelf) boatPreview.gameObject.SetActive(false);
        }

        private void HineVinePreview() {
            if (vinePreview.gameObject.activeSelf) vinePreview.gameObject.SetActive(false);
        }

        private void Attack() {
            rb.AddForce(-AttackDirection.normalized * 3f, ForceMode2D.Impulse);

            var colliders = new List<Collider2D>();
            Physics2D.OverlapCollider(attackCollider, new ContactFilter2D().NoFilter(), colliders);
            colliders.ForEach(c => c.GetComponent<IAttackable>()?.OnAttack(transform, 1));
        }

        private void AttackFinished() {
            Animator.SetInteger(AttackDirectionAnimator, 0);
            IsAttacking = false;
            AttackDirection = Vector2.zero;
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