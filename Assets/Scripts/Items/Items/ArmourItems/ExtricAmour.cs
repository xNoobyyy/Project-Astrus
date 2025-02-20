namespace Items.Items {
    public class ExtricAmour : ArmourItem {
        public ExtricAmour(int Lvl) : base("Lvl." + (Lvl - 1) + " extric armour ", "Armour made of extric", ItemManager.Instance.Amour2Icon,null, Lvl) { }
    }
}
