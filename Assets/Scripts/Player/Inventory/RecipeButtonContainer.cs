using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Inventory {
    public class RecipeButtonContainer : MonoBehaviour {
        [SerializeField] private GameObject recipeButtonPrefab;

        private void OnEnable() {
            foreach (var recipe in Crafting.Instance.AllRecipies) {
                if (recipe.advanced && !PlayerHealth.Instance.plateau) continue;

                var recipeButton = Instantiate(recipeButtonPrefab, transform);
                recipeButton.GetComponent<RecipeButton>().SetRecipe(recipe);
            }
        }

        private void OnDisable() {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
        }
    }
}