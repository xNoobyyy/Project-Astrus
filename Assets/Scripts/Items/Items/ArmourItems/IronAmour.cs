namespace Items.Items {
    public class IronAmour : ArmourItem {
        public IronAmour(int Lvl) : base("Lvl." + (Lvl - 1) + " iron armour ", "Armour made of iron", ItemManager.Instance.Amour2Icon,null, Lvl) { }
    }
}