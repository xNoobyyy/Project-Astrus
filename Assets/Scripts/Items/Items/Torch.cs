using UnityEngine;

namespace Items.Items {
    public class Torch : ResourceItem {
        public Torch(int amount = 1) : base("Torch", "A torch to light up", ItemManager.Instance.torch,null, amount) { }
    }
}