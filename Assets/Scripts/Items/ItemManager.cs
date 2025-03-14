﻿using UnityEngine;

namespace Items {
    public class ItemManager : MonoBehaviour {
        [Header("Item Icons")] public Sprite ironPickaxeIcon;
        public Sprite rockPickaxeIcon;
        public Sprite ironIcon;
        public Sprite stoneIcon;
        public Sprite stickIcon;
        public Sprite holzIcon;
        public Sprite bootIcon;
        public Sprite coalIcon;
        public Sprite fireStoneIcon;
        public Sprite fireplaceIcon;
        public Sprite fireIcon;
        public Sprite torch;
        public Sprite bottleOfWaterIcon;
        public Sprite invisiblePotionIcon;
        public Sprite lianaIcon;
        public Sprite specialFlower;
        public Sprite glomtomIcon2;
        public Sprite extricIcon2;
        public Sprite domilitantIcon2;
        public Sprite AstrusIcon2;
        public Sprite Sword1Icon;
        public Sprite Sword2Icon;
        public Sprite Sword3Icon;
        public Sprite Bow1Icon;
        public Sprite Bow2Icon;
        public Sprite Bow3Icon;
        public Sprite BowFire1Icon;
        public Sprite BowFire2Icon;
        public Sprite BowFire3Icon;
        public Sprite Axe1Icon;
        public Sprite Axe2Icon;
        public Sprite Amour1Icon;
        public Sprite Amour2Icon;
        public Sprite Amour3Icon;

        public RuntimeAnimatorController glomtomIcon;
        public RuntimeAnimatorController extricIcon;
        public RuntimeAnimatorController domilitantIcon;
        public RuntimeAnimatorController AstrusIcon;
        public RuntimeAnimatorController SwordAni;
        public RuntimeAnimatorController AxeAni;
        public RuntimeAnimatorController BowAni;
        public RuntimeAnimatorController BowAni2;

        [Header("Prefabs")] public GameObject droppedItemPrefab;

        public static ItemManager Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
                return;
            }
        }

        public GameObject CreateDroppedItem(Vector3 position, Vector3 from, Item item, Vector2 force) {
            var droppedItem = Instantiate(droppedItemPrefab, from, Quaternion.identity);

            var droppedItemComponent = droppedItem.GetComponent<DroppedItem.DroppedItem>();
            droppedItemComponent.Initialize(item);

            var rb = droppedItem.GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.AddForce(force, ForceMode2D.Impulse);
            }

            return droppedItem;
        }

        public GameObject DropItem(Item item, Vector3 origin) {
            var direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            var forceStrength = Random.Range(2f, 5f);
            var force = direction * forceStrength;

            return CreateDroppedItem(origin, origin, item, force);
        }
    }
}