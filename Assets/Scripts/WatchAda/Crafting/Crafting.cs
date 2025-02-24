using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Items.Items;
using JetBrains.Annotations;
using Player.Inventory;
using UnityEngine;

public class Crafting : MonoBehaviour {
    public ItemSlot CraftingSlot1;
    public ItemSlot CraftingSlot2;
    public ItemSlot CraftingSlot3;
    public ItemSlot CraftingSlot4;
    public DragOnlyItemSlot CraftedSlot;

    public GameObject AmountTextItem1;
    public GameObject AmountTextItem2;
    public GameObject AmountTextItem3;
    public GameObject AmountTextItem4;
    

    public Item CraftingItem1; 
    public Item CraftingItem2;
    public Item CraftingItem3;
    public Item CraftingItem4;
    public int CraftingAmountItem1;
    public int CraftingAmountItem2;
    public int CraftingAmountItem3;
    public int CraftingAmountItem4;

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
    public Item Lvl1ExtricArmour;
    public Item Lvl2ExtricArmour;
    public Item Lvl3ExtricArmour;
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
    public Recipies Lvl1ExtricArmourRecipy;
    public Recipies Lvl2ExtricArmourRecipy;
    public Recipies Lvl3ExtricArmourRecipy;
    public Recipies ExtricAmourRecipy;
    public Recipies StoneAxeRecipy;
    public Recipies IronAxeRecipy;
    public Recipies StonePickaxeRecipy;
    public Recipies IronPickaxeRecipy;
    public Recipies StoneSwordRecipy;
    public Recipies IronSwordRecipy;
    public Recipies GlomtomSwordRecipy;
    
    public List<Recipies> AllRecipies  = new List<Recipies>();
    
    void Awake() {
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
        IronAmour = new IronAmour();
        Lvl1ExtricArmour = new lvl1ExtricAmour();
        Lvl2ExtricArmour = new lvl2ExtricAmour();
        Lvl3ExtricArmour = new lvl3ExtricAmour();
        ExtricAmour = new ExtricAmour();

        // Rezepte-Initialisierung
        // Basisrezepte (Werkzeuge & Waffen)
        // 2 Sticks + 3 Steine/Eisen = Axt
        StoneAxeRecipy = new Recipies(Stick, Stone, null, null, 2, 3, 0, 0, StoneAxe, 1);
        IronAxeRecipy = new Recipies(Stick, Iron, null, null, 2, 3, 0, 0, IronAxe, 1);
        
        
        // 4 Steine/Eisen (Pickaxe-Rezepte)
        StonePickaxeRecipy = new Recipies(Stone, Stick, null, null, 4, 2, 0, 0, StonePickaxe, 1);
        IronPickaxeRecipy = new Recipies(Iron, Stick, null, null, 4, 2, 0, 0, IronPickaxe, 1);
        
        // Schwertrezepte
        // 1 Stick + 3 Steine = Schwert lvl. 1
        StoneSwordRecipy = new Recipies(Stone, Stick, null, null, 3, 1, 0, 0, StoneSword, 1);
        // 1 Stick + 3 Eisen = Schwert lvl. 2
        IronSwordRecipy = new Recipies(Iron, Stick, null, null, 3, 1, 0, 0, IronSword, 1);
        // 1 Stick + 3 Glomtom = Schwert lvl. 3
        GlomtomSwordRecipy = new Recipies(Glomtom, Stick, null, null, 3, 1, 0, 0, GlomtomSword, 1);
        
        // -------------------
        // Weitere Rezepte (Basis)
        // Boot: 4 Holz + 2 Sticks = Boot
        // (Hier wird – analog zu den Axt-Rezepten – zuerst Stick (2) und dann Holz (4) übergeben)
        BoatRecipy = new Recipies(Stick, Wood, null, null, 2, 4, 0, 0, Boat, 1);
        
        // Feuerstein: 1 Kohle + 1 Stein = Feuerstein
        FirestoneRecipy = new Recipies(Coal, Stone, null, null, 1, 1, 0, 0, Firestone, 1);
        
        // Lagerfeuer: 1 Feuerstein + 1 Feuerstelle = Lagerfeuer
        FireRecipy = new Recipies(Firestone, Fireplace, null, null, 1, 1, 0, 0, Fire, 1);
        
        // Feuerstelle: 5 Sticks + 5 Steine = Feuerstelle
        FireplaceRecipy = new Recipies(Stick, Stone, null, null, 5, 5, 0, 0, Fireplace, 1);
        
        // Fackel: 1 Stick + 1 Feuerstein = Fackel
        TorchRecipy = new Recipies(Stick, Firestone, null, null, 1, 1, 0, 0, Torch, 1);
        
        // -------------------
        // Rezepte (auf dem Plateau gefundene)
        // Unsichtbarkeitstrank: 1 Glas mit Wasser + 4 Domilitant = Unsichtbarkeitstrank
        InvisibilityPotionRecipy = new Recipies(BottleOfWater, Domilitant, null, null, 1, 4, 0, 0, InvisibilityPotion, 1);
        
        // 1 Holz + 1 Glomtom + 1 Eisen + 1 Besondere Blume = 10 Extric
        ExtricRecipy = new Recipies(Wood, Glomtom, Iron, SpecialFlower, 1, 1, 1, 1, Extric, 10);
        
        // Rüstungsrezepte:
        // 4 Eisen = Schwache Rüstung
        IronAmourRecipy = new Recipies(Iron, null, null, null, 4, 0, 0, 0, IronAmour, 1);
        // (4-X) Eisen + X Extric = lvl. X Rüstung (als Beispiel hier: 2 Eisen + 2 Extric = gemischte Rüstung lvl.2)
        Lvl1ExtricArmourRecipy = new Recipies(Iron,Extric, null, null, 3, 1, 0, 0, Lvl1ExtricArmour, 1);
        Lvl2ExtricArmourRecipy = new Recipies(Iron, Extric, null, null, 2, 2, 0, 0, Lvl2ExtricArmour, 1);
        Lvl3ExtricArmourRecipy = new Recipies(Iron, Extric, null, null, 1, 3, 0, 0, Lvl3ExtricArmour, 1);
        // 4 Extric = Full Rüstung
        ExtricAmourRecipy = new Recipies(Extric, null, null, null, 4, 0, 0, 0, ExtricAmour, 1);
        
        // -------------------
        // Rezepte (Bögen)
        // Bogen lvl. 1: 1 Holz + 2 Steine + 1 Liane
        StoneBowRecipy = new Recipies(Wood, Stone, Liana, null, 1, 2, 1, 0, StoneBow, 1);
        // Bogen lvl. 2: 1 Holz + 2 Eisen + 1 Liane
        IronBowRecipy = new Recipies(Wood, Iron, Liana, null, 1, 2, 1, 0, IronBow, 1);
        // Bogen lvl. 3: 1 Holz + 2 Glomtom + 1 Liane
        GlomtomBowRecipy = new Recipies(Wood, Glomtom, Liana, null, 1, 2, 1, 0, GlomtomBow, 1);
        // Feuerbogen: 1 Bogen + 1 Feuerstein = Feuerbogen
        FireBowRecipy = new Recipies(StoneBow, Firestone, null, null, 1, 1, 0, 0, FireBow, 1);

        
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
        AllRecipies.Add(FirestoneRecipy);
        AllRecipies.Add(BoatRecipy);
        AllRecipies.Add(FirestoneRecipy);
        AllRecipies.Add(FireplaceRecipy);
        AllRecipies.Add(TorchRecipy);
        AllRecipies.Add(InvisibilityPotionRecipy);
        AllRecipies.Add(ExtricRecipy);
        AllRecipies.Add(FireRecipy);
        AllRecipies.Add(IronAmourRecipy);
        AllRecipies.Add(Lvl1ExtricArmourRecipy);
        AllRecipies.Add(Lvl2ExtricArmourRecipy);
        AllRecipies.Add(Lvl3ExtricArmourRecipy);
        AllRecipies.Add(ExtricAmourRecipy);
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Los");
    }

    void Update() {
        
        //Debug.Log("Los");
        
        CraftingItem1 = CraftingSlot1.Item;
        CraftingItem2 = CraftingSlot2.Item;
        CraftingItem3 = CraftingSlot3.Item;
        CraftingItem4 = CraftingSlot4.Item;
        
        if (CraftingItem1 is ResourceItem resourceItem1) {
            CraftingAmountItem1 = resourceItem1.Amount;
        }else if (CraftingItem1 != null) {
            CraftingAmountItem1 = 1;
        } else {
            CraftingAmountItem1 = 0;}

        if (CraftingItem2 is ResourceItem resourceItem2) {
            CraftingAmountItem2 = resourceItem2.Amount;
        }else if (CraftingItem2 != null) {
            CraftingAmountItem2 = 1;
            
        } else {
            CraftingAmountItem2 = 0;
        }

        if (CraftingItem3 is ResourceItem resourceItem3) {
            CraftingAmountItem3 = resourceItem3.Amount;
        }else if (CraftingItem3 != null) {
            CraftingAmountItem3 = 1;
        } else {
            CraftingAmountItem3 = 0;
        }

        if (CraftingItem4 is ResourceItem resourceItem4) {
            CraftingAmountItem4 = resourceItem4.Amount;
        }else if (CraftingItem4 != null) {
            CraftingAmountItem4 = 1;
        } else {
            CraftingAmountItem4 = 0;
        }
        
        foreach (Recipies recipy in AllRecipies)
        {
            if (recipy.checkRecipy(CraftingItem1, CraftingItem2, CraftingItem3, CraftingItem4, CraftingAmountItem1,
                    CraftingAmountItem2, CraftingAmountItem3, CraftingAmountItem4)) {
                CraftingSlot1.clearSlot();
                CraftingSlot2.clearSlot();
                CraftingSlot3.clearSlot();
                CraftingSlot4.clearSlot();
                Debug.Log("Check");
                
                Type typ = recipy.CraftedItem.GetType();

                // Prüfen, ob der Typ von ResourceItem abgeleitet ist
                if (typeof(ResourceItem).IsAssignableFrom(typ))
                {
                    object instance = Activator.CreateInstance(typ, new object[] { recipy.CraftedAmount });
                    if (instance is ResourceItem resourceItem)
                    {
                        Debug.Log("Falsches Item");
                        CraftedSlot.fillSlot(resourceItem);
                    }
                } else {
                    object instance = Activator.CreateInstance(typ);
                    if (instance is Item item)
                    {
                        Debug.Log("Sollte funktionieren");
                        CraftedSlot.fillSlot(item);
                    }
                }
                
                
            
            }
        }
    }

    // Update is called once per frame
    void Updated() {
        CraftingItem1 = CraftingSlot1.Item;
        CraftingItem2 = CraftingSlot2.Item;
        CraftingItem3 = CraftingSlot3.Item;
        CraftingItem4 = CraftingSlot4.Item;

        if (CraftingItem1 is ResourceItem resourceItem1) {
            CraftingAmountItem1 = resourceItem1.Amount;
        }else if (CraftingItem1 != null) {
            CraftingAmountItem1 = 1;
        } else {
            CraftingAmountItem1 = 0;}

        if (CraftingItem2 is ResourceItem resourceItem2) {
            CraftingAmountItem2 = resourceItem2.Amount;
        }else if (CraftingItem2 != null) {
            CraftingAmountItem2 = 1;
            
        } else {
            CraftingAmountItem2 = 0;
        }

        if (CraftingItem3 is ResourceItem resourceItem3) {
            CraftingAmountItem3 = resourceItem3.Amount;
        }else if (CraftingItem3 != null) {
            CraftingAmountItem3 = 1;
        } else {
            CraftingAmountItem3 = 0;
        }

        if (CraftingItem4 is ResourceItem resourceItem4) {
            CraftingAmountItem4 = resourceItem4.Amount;
        }else if (CraftingItem4 != null) {
            CraftingAmountItem4 = 1;
        } else {
            CraftingAmountItem4 = 0;
        }

        if (StoneAxeRecipy.checkRecipy(CraftingItem1, CraftingItem2, CraftingItem3, CraftingItem4, CraftingAmountItem1,
                CraftingAmountItem2, CraftingAmountItem3, CraftingAmountItem4)) {
            CraftingSlot1.clearSlot();
            CraftingSlot2.clearSlot();
            CraftingSlot3.clearSlot();
            CraftingSlot4.clearSlot();
            CraftedSlot.fillSlot(new StoneAxe());
            
        }

        if (IronAxeRecipy.checkRecipy(CraftingItem1, CraftingItem2, CraftingItem3, CraftingItem4, CraftingAmountItem1,
                CraftingAmountItem2, CraftingAmountItem3, CraftingAmountItem4)) {
            CraftedSlot.fillSlot(new IronAxe());
            CraftingSlot1.clearSlot();
            CraftingSlot2.clearSlot();
            CraftingSlot3.clearSlot();
            CraftingSlot4.clearSlot();
        }

        if (StonePickaxeRecipy.checkRecipy(CraftingItem1, CraftingItem2, CraftingItem3, CraftingItem4,
                CraftingAmountItem1, CraftingAmountItem2, CraftingAmountItem3, CraftingAmountItem4)) {
            CraftedSlot.fillSlot(new StonePickaxe());
            CraftingSlot1.clearSlot();
            CraftingSlot2.clearSlot();
            CraftingSlot3.clearSlot();
            CraftingSlot4.clearSlot();
        }
        
        if (IronPickaxeRecipy.checkRecipy(CraftingItem1, CraftingItem2, CraftingItem3, CraftingItem4,
                CraftingAmountItem1, CraftingAmountItem2, CraftingAmountItem3, CraftingAmountItem4)) {
            CraftedSlot.fillSlot(new IronPickaxe());
            CraftingSlot1.clearSlot();
            CraftingSlot2.clearSlot();
            CraftingSlot3.clearSlot();
            CraftingSlot4.clearSlot();
        }
        
        if (StoneSwordRecipy.checkRecipy(CraftingItem1, CraftingItem2, CraftingItem3, CraftingItem4,
                CraftingAmountItem1, CraftingAmountItem2, CraftingAmountItem3, CraftingAmountItem4)) {
            CraftedSlot.fillSlot(new StoneSword());
            CraftingSlot1.clearSlot();
            CraftingSlot2.clearSlot();
            CraftingSlot3.clearSlot();
            CraftingSlot4.clearSlot();
        }
        
        if (IronSwordRecipy.checkRecipy(CraftingItem1, CraftingItem2, CraftingItem3, CraftingItem4,
                CraftingAmountItem1, CraftingAmountItem2, CraftingAmountItem3, CraftingAmountItem4)) {
            CraftedSlot.fillSlot(new IronSword());
            CraftingSlot1.clearSlot();
            CraftingSlot2.clearSlot();
            CraftingSlot3.clearSlot();
            CraftingSlot4.clearSlot();
        }
        
        if (GlomtomSwordRecipy.checkRecipy(CraftingItem1, CraftingItem2, CraftingItem3, CraftingItem4,
                CraftingAmountItem1, CraftingAmountItem2, CraftingAmountItem3, CraftingAmountItem4)) {
            CraftedSlot.fillSlot(new GlomtomSword());
            CraftingSlot1.clearSlot();
            CraftingSlot2.clearSlot();
            CraftingSlot3.clearSlot();
            CraftingSlot4.clearSlot();
        }
        
    }
}
