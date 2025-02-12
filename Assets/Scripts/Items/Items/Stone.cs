namespace Items.Items {
    public class Stone : ResourceItem {
        public Stone(int amount = 1) : base("Stone", "A piece of stone", ItemManager.Instance.stoneIcon,null, amount) { }
    }
}