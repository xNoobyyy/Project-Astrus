using UnityEngine;

namespace Items {
    public abstract class ResourceItem : Item {
        public int MaxAmount { get; private set; }
        public int Amount { get; private set; }

        protected ResourceItem(string name, string description, Sprite icon, AnimationClip animator, int amount = 1, int maxAmount = 100) : base(name, description, icon, animator) {
            Amount = amount;
            MaxAmount = maxAmount;
        }

        /// <summary>
        /// Sets the amount of the resource item.
        /// </summary>
        /// <param name="amount">The amount to set. Has to be between 0 and <see cref="MaxAmount"/>.</param>
        /// <returns>True if the amount was set, false otherwise.</returns>
        public bool SetAmount(int amount) {
            if (amount <= 0 || amount > MaxAmount) return false;

            Amount = amount;
            return true;
        }

        public override void OnUse() { }
    }
}