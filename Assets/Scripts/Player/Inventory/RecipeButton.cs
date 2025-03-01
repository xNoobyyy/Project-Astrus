using System;
using Items;
using Logic.Events;
using Player.Inventory.Slots;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Inventory {
    public class RecipeButton : MonoBehaviour {
        [SerializeField] private Image itemImage;
        [SerializeField] private Image backgroundImage;

        [SerializeField] private Sprite nonAdvancedSprite;
        [SerializeField] private Sprite advancedSprite;

        private Recipies Recipe { get; set; }

        private bool craftable;

        private void OnEnable() {
            EventManager.Instance.Subscribe<PlayerItemEvent>(OnPlayerItem);
        }

        private void OnDisable() {
            EventManager.Instance.Unsubscribe<PlayerItemEvent>(OnPlayerItem);
        }

        public void SetRecipe(Recipies recipe) {
            Recipe = recipe;

            itemImage.sprite = recipe.CraftedItem.Icon;
            backgroundImage.sprite = recipe.advanced ? advancedSprite : nonAdvancedSprite;

            if (recipe.CraftedItem.Icon == null) {
                Debug.LogWarning("Icon is null for " + recipe.CraftedItem.Name);
            }

            CheckCraftable();
        }

        // ReSharper disable once UnusedMember.Global - called by Unity
        public void OnClick() {
            CheckCraftable();
            if (!craftable) return;

            var slots = PlayerInventory.Instance.Slots;

            var item1 = Recipe.Item1;
            var item1Amount = Recipe.NumberOfItem1;

            var item2 = Recipe.Item2;
            var item2Amount = Recipe.NumberOfItem2;

            var item3 = Recipe.Item3;
            var item3Amount = Recipe.NumberOfItem3;

            var item4 = Recipe.Item4;
            var item4Amount = Recipe.NumberOfItem4;

            // put them in Crafting.Instance.CraftingSlot1, Crafting.Instance.CraftingSlot2, etc.
            var remainingItem1 = item1Amount ?? 0;
            var remainingItem2 = item2Amount ?? 0;
            var remainingItem3 = item3Amount ?? 0;
            var remainingItem4 = item4Amount ?? 0;

            foreach (var slot in PlayerInventory.Instance.Slots) {
                if (slot is CraftingSlot or DragOnlyItemSlot) continue;
                if (remainingItem1 == 0 && remainingItem2 == 0 && remainingItem3 == 0 && remainingItem4 == 0) break;
                if (slot.Item == null) continue;

                if (item1 != null && slot.Item.GetType() == item1.GetType() && remainingItem1 > 0) {
                    if (slot.Item is ResourceItem resourceItem) {
                        if (resourceItem.Amount > remainingItem1) {
                            resourceItem.Amount -= remainingItem1;
                            slot.UpdateDisplay();

                            remainingItem1 = 0;
                        } else {
                            remainingItem1 -= resourceItem.Amount;
                            slot.UpdateDisplay();

                            slot.SetItem(null);
                            continue;
                        }
                    } else {
                        remainingItem1--;
                        slot.SetItem(null);
                        continue;
                    }
                }

                if (item2 != null && slot.Item.GetType() == item2.GetType() && remainingItem2 > 0) {
                    if (slot.Item is ResourceItem resourceItem) {
                        if (resourceItem.Amount > remainingItem2) {
                            resourceItem.Amount -= remainingItem2;
                            slot.UpdateDisplay();

                            remainingItem2 = 0;
                        } else {
                            remainingItem2 -= resourceItem.Amount;
                            slot.UpdateDisplay();

                            slot.SetItem(null);
                            continue;
                        }
                    } else {
                        remainingItem2--;
                        slot.SetItem(null);
                        continue;
                    }
                }

                if (item3 != null && slot.Item.GetType() == item3.GetType() && remainingItem3 > 0) {
                    if (slot.Item is ResourceItem resourceItem) {
                        if (resourceItem.Amount > remainingItem3) {
                            resourceItem.Amount -= remainingItem3;
                            slot.UpdateDisplay();

                            remainingItem3 = 0;
                        } else {
                            remainingItem3 -= resourceItem.Amount;
                            slot.UpdateDisplay();

                            slot.SetItem(null);
                            continue;
                        }
                    } else {
                        remainingItem3--;
                        slot.SetItem(null);
                        continue;
                    }
                }

                if (item4 != null && slot.Item.GetType() == item4.GetType() && remainingItem4 > 0) {
                    if (slot.Item is ResourceItem resourceItem) {
                        if (resourceItem.Amount > remainingItem4) {
                            resourceItem.Amount -= remainingItem4;
                            slot.UpdateDisplay();

                            remainingItem4 = 0;
                        } else {
                            remainingItem4 -= resourceItem.Amount;
                            slot.UpdateDisplay();

                            slot.SetItem(null);
                            continue;
                        }
                    } else {
                        remainingItem4--;
                        slot.SetItem(null);
                        continue;
                    }
                }
            }

            var beforeItem1 = Crafting.Instance.CraftingSlot1.Item;
            var beforeItem2 = Crafting.Instance.CraftingSlot2.Item;
            var beforeItem3 = Crafting.Instance.CraftingSlot3.Item;
            var beforeItem4 = Crafting.Instance.CraftingSlot4.Item;

            if (item1 is ResourceItem resourceItem1) {
                Crafting.Instance.CraftingSlot1.SetItem(item1);
                resourceItem1.Amount = item1Amount ?? 1;
            } else {
                Crafting.Instance.CraftingSlot1.SetItem(item1);
            }

            if (item2 is ResourceItem resourceItem2) {
                Crafting.Instance.CraftingSlot2.SetItem(item2);
                resourceItem2.Amount = item2Amount ?? 1;
            } else {
                Crafting.Instance.CraftingSlot2.SetItem(item2);
            }

            if (item3 is ResourceItem resourceItem3) {
                Crafting.Instance.CraftingSlot3.SetItem(item3);
                resourceItem3.Amount = item3Amount ?? 1;
            } else {
                Crafting.Instance.CraftingSlot3.SetItem(item3);
            }

            if (item4 is ResourceItem resourceItem4) {
                Crafting.Instance.CraftingSlot4.SetItem(item4);
                resourceItem4.Amount = item4Amount ?? 1;
            } else {
                Crafting.Instance.CraftingSlot4.SetItem(item4);
            }

            Crafting.Instance.CraftingSlot1.UpdateDisplay();
            Crafting.Instance.CraftingSlot2.UpdateDisplay();
            Crafting.Instance.CraftingSlot3.UpdateDisplay();
            Crafting.Instance.CraftingSlot4.UpdateDisplay();

            if (beforeItem1 != null) {
                ItemManager.Instance.DropItem(beforeItem1, PlayerItem.Instance.transform.position);
            }

            if (beforeItem2 != null) {
                ItemManager.Instance.DropItem(beforeItem2, PlayerItem.Instance.transform.position);
            }

            if (beforeItem3 != null) {
                ItemManager.Instance.DropItem(beforeItem3, PlayerItem.Instance.transform.position);
            }

            if (beforeItem4 != null) {
                ItemManager.Instance.DropItem(beforeItem4, PlayerItem.Instance.transform.position);
            }
        }

        private void OnPlayerItem(PlayerItemEvent e) {
            CheckCraftable();
        }

        private void CheckCraftable() {
            if (Recipe == null) return;

            var slots = PlayerInventory.Instance.Slots;

            if (slots == null) return;

            var item1 = Recipe.Item1;
            var item1Amount = Recipe.NumberOfItem1;

            var item2 = Recipe.Item2;
            var item2Amount = Recipe.NumberOfItem2;

            var item3 = Recipe.Item3;
            var item3Amount = Recipe.NumberOfItem3;

            var item4 = Recipe.Item4;
            var item4Amount = Recipe.NumberOfItem4;

            // check items
            var item1Count = 0;
            var item2Count = 0;
            var item3Count = 0;
            var item4Count = 0;

            foreach (var slot in slots) {
                if (slot.Item == null) continue;

                if (item1 != null && slot.Item.GetType() == item1.GetType())
                    item1Count += slot.Item is ResourceItem resourceItem ? resourceItem.Amount : 1;
                if (item2 != null && slot.Item.GetType() == item2.GetType())
                    item2Count += slot.Item is ResourceItem resourceItem ? resourceItem.Amount : 1;
                if (item3 != null && slot.Item.GetType() == item3.GetType())
                    item3Count += slot.Item is ResourceItem resourceItem ? resourceItem.Amount : 1;
                if (item4 != null && slot.Item.GetType() == item4.GetType())
                    item4Count += slot.Item is ResourceItem resourceItem ? resourceItem.Amount : 1;
            }

            craftable = item1Count >= item1Amount && item2Count >= item2Amount && item3Count >= item3Amount &&
                        item4Count >= item4Amount;

            if (craftable) {
                itemImage.color = Color.white;
                backgroundImage.color = Color.white;
            } else {
                itemImage.color = Color.white.WithAlpha(0.5f);
                backgroundImage.color = Color.white.WithAlpha(0.5f);
            }
        }
    }
}