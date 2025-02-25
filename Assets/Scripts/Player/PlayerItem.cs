using System;
using System.Collections.Generic;
using Creatures;
using TextDisplay;
using UnityEngine;
using Tree = Objects.Tree;

namespace Player {
    public class PlayerItem : MonoBehaviour {
        private static readonly int AttackDirection = Animator.StringToHash("attack_direction");

        public static PlayerItem Instance { get; private set; }

        [SerializeField] public Collider2D attackCollider;

        [NonSerialized] public Camera mainCamera;
        [NonSerialized] public Animator animator;
        private Rigidbody2D rb;

        public bool IsAttacking { get; set; }
        [NonSerialized] public Vector2 attackDirection;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }

            IsAttacking = false;

            mainCamera = Camera.main;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
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

                if (currentItem != null) {
                    currentItem.OnUse(transform, worldPosition);
                } else {
                    if (IsAttacking || Vector2.Distance(transform.position, worldPosition) > 3f) return;

                    var colliders = Physics2D.OverlapPointAll(worldPosition);
                    var trees = Array.FindAll(colliders,
                        c => c.GetComponent<Tree>() != null || c.GetComponentInParent<Tree>() != null);
                    Array.Sort(trees, (c1, c2) => c1.transform.position.y.CompareTo(c2.transform.position.y));
                    var treeCollider = trees.Length > 0 ? trees[0] : null;
                    var tree = treeCollider?.GetComponent<Tree>() ?? treeCollider?.GetComponentInParent<Tree>();

                    tree?.GetComponent<Tree>()?.Chop(1);
                }
            } else if (Input.GetMouseButtonDown(1)) {
                // Right Click
                var zCoord = mainCamera.WorldToScreenPoint(transform.position).z;
                var mousePosition = Input.mousePosition;
                mousePosition.z = zCoord;

                var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                var currentItem = LogicScript.Instance.accessableInventoryManager2.CurrentSlot.Item;

                currentItem?.OnUse(transform, worldPosition);
            }
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
    }
}