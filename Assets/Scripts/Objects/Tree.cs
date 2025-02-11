using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;

namespace Objects {
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class Tree : MonoBehaviour {
        [SerializeField] private Sprite full;
        [SerializeField] private Sprite destroyed;
        [SerializeField] private PolygonCollider2D trigger;

        private SpriteRenderer spriteRenderer;
        private Camera mainCamera;


        public bool IsDestroyed { get; private set; }

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            mainCamera = Camera.main;
        }

        // TODO: Only when axe is equipped
        private void Update() {
            if (IsDestroyed || !Input.GetMouseButtonDown(0)) return;

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

            Destroy();
        }

        public void Destroy() {
            IsDestroyed = true;
            spriteRenderer.sprite = destroyed;
        }

        private void OnValidate() {
            GetComponent<SpriteRenderer>().sprite = full;
        }
    }
}