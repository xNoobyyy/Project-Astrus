using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Items.Items;
using Items.Items.ArmorItems;
using Items.Items.AxeItems;
using Items.Items.BowItems;
using Items.Items.CombatItems;
using Logic.Events;
using Player.Inventory.Slots;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

public class Crafting : MonoBehaviour {
    public static Crafting Instance { get; private set; }

    public ItemSlot craftingSlot1;
    public ItemSlot craftingSlot2;
    public ItemSlot craftingSlot3;
    public ItemSlot craftingSlot4;
    public DragOnlyItemSlot craftedSlot;

    private Item craftingItem1;
    private Item craftingItem2;
    private Item craftingItem3;
    private Item craftingItem4;
    private int craftingAmountItem1;
    private int craftingAmountItem2;
    private int craftingAmountItem3;
    private int craftingAmountItem4;

    public Item Stone;
    public Item Stick;
    public Item Iron;
    public Item Glomtom;
    public Item Extric;
    public Item Domilitant;
    public Item Astrus;
    public Item Coal;
    public Item Torch;
    public Item Wood;
    public Item Boat;
    public Item Fire;
    public Item Firestone;
    public Item Fireplace;
    public Item BottleOfWater;
    public Item InvisibilityPotion;
    public Item SpecialFlower;
    public Item Liana;
    public Item StoneAxe;
    public Item IronAxe;
    public Item StonePickaxe;
    public Item IronPickaxe;
    public Item StoneSword;
    public Item IronSword;
    public Item GlomtomSword;
    public Item StoneBow;
    public Item IronBow;
    public Item GlomtomBow;
    public Item FireBow;
    public Item IronAmour;
    public Item Lvl1ExtricArmor;
    public Item Lvl2ExtricArmor;
    public Item Lvl3ExtricArmor;
    public Item ExtricAmour;

    public Recipies ExtricRecipy;
    public Recipies BoatRecipy;
    public Recipies TorchRecipy;
    public Recipies FireRecipy;
    public Recipies FirestoneRecipy;
    public Recipies FireplaceRecipy;
    public Recipies InvisibilityPotionRecipy;
    public Recipies StoneBowRecipy;
    public Recipies IronBowRecipy;
    public Recipies GlomtomBowRecipy;
    public Recipies FireBowRecipy;
    public Recipies IronAmourRecipy;
    public Recipies Lvl1ExtricArmorRecipy;
    public Recipies Lvl2ExtricArmorRecipy;
    public Recipies Lvl3ExtricArmorRecipy;
    public Recipies ExtricAmourRecipy;
    public Recipies StoneAxeRecipy;
    public Recipies IronAxeRecipy;
    public Recipies StonePickaxeRecipy;
    public Recipies IronPickaxeRecipy;
    public Recipies StoneSwordRecipy;
    public Recipies IronSwordRecipy;
    public Recipies GlomtomSwordRecipy;

    public List<Recipies> AllRecipies = new List<Recipies>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        // Initialisiere die Items hier – zu diesem Zeitpunkt sollten alle Unity-Ressourcen bereitstehen.

        Stick = new Stick();
        Stone = new Stone();
        Iron = new Iron();
        Glomtom = new Glomtom();
        Extric = new Extric();
        Domilitant = new Domilitant();
        Astrus = new Astrus();
        Coal = new Coal();
        Torch = new Torch();
        Wood = new Wood();
        Boat = new Boat();
        Fire = new Fire();
        Firestone = new Firestone();
        Fireplace = new Fireplace();
        BottleOfWater = new BottleOfWater();
        InvisibilityPotion = new InvisibilityPotion();
        SpecialFlower = new SpecialFlower();
        Liana = new Liana();

        StoneAxe = new StoneAxe();
        IronAxe = new IronAxe();
        StonePickaxe = new StonePickaxe();
        IronPickaxe = new IronPickaxe();
        StoneSword = new StoneSword();
        IronSword = new IronSword();
        GlomtomSword = new GlomtomSword();
        StoneBow = new StoneBow();
        IronBow = new IronBow();
        GlomtomBow = new GlomtomBow();
        FireBow = new FireGlomtomBow();
        IronAmour = new IronArmor();
        Lvl1ExtricArmor = new Lvl1Amour();
        Lvl2ExtricArmor = new Lvl2Amour();
        Lvl3ExtricArmor = new Lvl3Amour();
        ExtricAmour = new ExtricArmor();

        // Rezepte-Initialisierung
        // Basisrezepte (Werkzeuge & Waffen)
        // 2 Sticks + 3 Steine/Eisen = Axt
        StoneAxeRecipy = new Recipies(Stick, Stone, null, null, 2, 3, 0, 0, StoneAxe, 1, false);
        IronAxeRecipy = new Recipies(Stick, Iron, null, null, 2, 3, 0, 0, IronAxe, 1, false);

        // 4 Steine/Eisen (Pickaxe-Rezepte)
        StonePickaxeRecipy = new Recipies(Stone, Stick, null, null, 4, 2, 0, 0, StonePickaxe, 1, false);
        IronPickaxeRecipy = new Recipies(Iron, Stick, null, null, 4, 2, 0, 0, IronPickaxe, 1, false);

        // Schwertrezepte
        // 1 Stick + 3 Steine = Schwert lvl. 1
        StoneSwordRecipy = new Recipies(Stone, Stick, null, null, 3, 1, 0, 0, StoneSword, 1, false);
        // 1 Stick + 3 Eisen = Schwert lvl. 2
        IronSwordRecipy = new Recipies(Iron, Stick, null, null, 3, 1, 0, 0, IronSword, 1, false);
        // 1 Stick + 3 Glomtom = Schwert lvl. 3
        GlomtomSwordRecipy = new Recipies(Glomtom, Stick, null, null, 3, 1, 0, 0, GlomtomSword, 1, false);

        // -------------------
        // Weitere Rezepte (Basis)
        // Boot: 4 Holz + 2 Sticks = Boot
        // (Hier wird – analog zu den Axt-Rezepten – zuerst Stick (2) und dann Holz (4) übergeben)
        BoatRecipy = new Recipies(Stick, Wood, null, null, 2, 4, 0, 0, Boat, 1, false);

        // Feuerstein: 1 Kohle + 1 Stein = Feuerstein
        FirestoneRecipy = new Recipies(Coal, Stone, null, null, 1, 1, 0, 0, Firestone, 1, false);

        // Lagerfeuer: 1 Feuerstein + 1 Feuerstelle = Lagerfeuer
        FireRecipy = new Recipies(Firestone, Fireplace, null, null, 1, 1, 0, 0, Fire, 1, false);

        // Feuerstelle: 5 Sticks + 5 Steine = Feuerstelle
        FireplaceRecipy = new Recipies(Stick, Stone, null, null, 5, 5, 0, 0, Fireplace, 1, false);

        // Fackel: 1 Stick + 1 Feuerstein = Fackel
        TorchRecipy = new Recipies(Stick, Firestone, null, null, 1, 1, 0, 0, Torch, 1, false);

        // -------------------
        // Rezepte (auf dem Plateau gefundene)
        // Unsichtbarkeitstrank: 1 Glas mit Wasser + 4 Domilitant = Unsichtbarkeitstrank
        InvisibilityPotionRecipy =
            new Recipies(BottleOfWater, Domilitant, null, null, 1, 4, 0, 0, InvisibilityPotion, 1, true);

        // 1 Holz + 1 Glomtom + 1 Eisen + 1 Besondere Blume = 10 Extric
        ExtricRecipy = new Recipies(Wood, Glomtom, Iron, SpecialFlower, 1, 1, 1, 1, Extric, 10, true);

        // Rüstungsrezepte:
        // 4 Eisen = Schwache Rüstung
        IronAmourRecipy = new Recipies(Iron, null, null, null, 4, 0, 0, 0, IronAmour, 1, false);
        // (4-X) Eisen + X Extric = lvl. X Rüstung (Beispiel: 2 Eisen + 2 Extric = gemischte Rüstung lvl.2)
        Lvl1ExtricArmorRecipy = new Recipies(Iron, Extric, null, null, 3, 1, 0, 0, Lvl1ExtricArmor, 1, true);
        Lvl2ExtricArmorRecipy = new Recipies(Iron, Extric, null, null, 2, 2, 0, 0, Lvl2ExtricArmor, 1, true);
        Lvl3ExtricArmorRecipy = new Recipies(Iron, Extric, null, null, 1, 3, 0, 0, Lvl3ExtricArmor, 1, true);
        // 4 Extric = Full Rüstung
        ExtricAmourRecipy = new Recipies(Extric, null, null, null, 4, 0, 0, 0, ExtricAmour, 1, true);

        // -------------------
        // Rezepte (Bögen)
        // Bogen lvl. 1: 1 Holz + 2 Steine + 1 Liane
        StoneBowRecipy = new Recipies(Wood, Stone, Liana, null, 1, 2, 1, 0, StoneBow, 1, false);
        // Bogen lvl. 2: 1 Holz + 2 Eisen + 1 Liane
        IronBowRecipy = new Recipies(Wood, Iron, Liana, null, 1, 2, 1, 0, IronBow, 1, false);
        // Bogen lvl. 3: 1 Holz + 2 Glomtom + 1 Liane
        GlomtomBowRecipy = new Recipies(Wood, Glomtom, Liana, null, 1, 2, 1, 0, GlomtomBow, 1, false);
        // Feuerbogen: 1 Bogen + 1 Feuerstein = Feuerbogen
        FireBowRecipy = new Recipies(StoneBow, Firestone, null, null, 1, 1, 0, 0, FireBow, 1, false);

        Debug.Log(StoneAxeRecipy);
        AllRecipies.Add(StoneAxeRecipy);
        AllRecipies.Add(IronAxeRecipy);
        AllRecipies.Add(StonePickaxeRecipy);
        AllRecipies.Add(IronPickaxeRecipy);
        AllRecipies.Add(StoneSwordRecipy);
        AllRecipies.Add(IronSwordRecipy);
        AllRecipies.Add(GlomtomSwordRecipy);
        AllRecipies.Add(StoneBowRecipy);
        AllRecipies.Add(IronBowRecipy);
        AllRecipies.Add(GlomtomBowRecipy);
        AllRecipies.Add(BoatRecipy);
        AllRecipies.Add(FirestoneRecipy);
        AllRecipies.Add(FireplaceRecipy);
        AllRecipies.Add(TorchRecipy);
        AllRecipies.Add(InvisibilityPotionRecipy);
        AllRecipies.Add(ExtricRecipy);
        AllRecipies.Add(FireRecipy);
        AllRecipies.Add(IronAmourRecipy);
        AllRecipies.Add(Lvl1ExtricArmorRecipy);
        AllRecipies.Add(Lvl2ExtricArmorRecipy);
        AllRecipies.Add(Lvl3ExtricArmorRecipy);
        AllRecipies.Add(ExtricAmourRecipy);
    }

    private void Update() {
        if (craftedSlot.Item != null) return;

        craftingItem1 = craftingSlot1.Item;
        craftingItem2 = craftingSlot2.Item;
        craftingItem3 = craftingSlot3.Item;
        craftingItem4 = craftingSlot4.Item;

        if (craftingItem1 is ResourceItem resourceItem1) {
            craftingAmountItem1 = resourceItem1.Amount;
        } else if (craftingItem1 != null) {
            craftingAmountItem1 = 1;
        } else {
            craftingAmountItem1 = 0;
        }

        if (craftingItem2 is ResourceItem resourceItem2) {
            craftingAmountItem2 = resourceItem2.Amount;
        } else if (craftingItem2 != null) {
            craftingAmountItem2 = 1;
        } else {
            craftingAmountItem2 = 0;
        }

        if (craftingItem3 is ResourceItem resourceItem3) {
            craftingAmountItem3 = resourceItem3.Amount;
        } else if (craftingItem3 != null) {
            craftingAmountItem3 = 1;
        } else {
            craftingAmountItem3 = 0;
        }

        if (craftingItem4 is ResourceItem resourceItem4) {
            craftingAmountItem4 = resourceItem4.Amount;
        } else if (craftingItem4 != null) {
            craftingAmountItem4 = 1;
        } else {
            craftingAmountItem4 = 0;
        }

        var recipe = AllRecipies.FirstOrDefault(recipe => recipe.checkRecipy(craftingItem1, craftingItem2,
            craftingItem3, craftingItem4, craftingAmountItem1,
            craftingAmountItem2, craftingAmountItem3, craftingAmountItem4));

        if (recipe == null) return;

        craftingSlot1.ClearSlot();
        craftingSlot2.ClearSlot();
        craftingSlot3.ClearSlot();
        craftingSlot4.ClearSlot();

        var typ = recipe.CraftedItem.GetType();

        // Prüfen, ob der Typ von ResourceItem abgeleitet ist
        if (typeof(ResourceItem).IsAssignableFrom(typ)) {
            var instance = Activator.CreateInstance(typ, recipe.CraftedAmount);
            if (instance is not ResourceItem resourceItem) return;

            Debug.Log("Falsches Item");
            craftedSlot.FillSlot(resourceItem);
        } else {
            var instance = Activator.CreateInstance(typ);
            if (instance is not Item item) return;

            Debug.Log("Sollte funktionieren");
            craftedSlot.FillSlot(item);
            EventManager.Instance.Trigger(new PlayerItemEvent(item, craftedSlot));
            SoundManager.Instance.PlaySound(SoundEffect.CraftingFinish);
        }
    }
}