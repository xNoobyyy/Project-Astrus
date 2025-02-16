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
    public Item StoneAxe;
    public Item IronAxe;
    public Item StonePickaxe;
    public Item IronPickaxe;
    public Item StoneSword;
    public Item IronSword;
    public Item GlomtomSword;

    public Recipies StoneAxeRecipy;
    public Recipies IronAxeRecipy;
    public Recipies StonePickaxeRecipy;
    public Recipies IronPickaxeRecipy;
    public Recipies StoneSwordRecipy;
    public Recipies IronSwordRecipy;
    public Recipies GlomtomSwordRecipy;
    
    void Awake() {
        // Initialisiere die Items hier – zu diesem Zeitpunkt sollten alle Unity-Ressourcen bereitstehen.
        Stone = new Stone(1);
        Stick = new Stick(1);
        Iron = new Iron(1);
        Glomtom = new Glomtom(1);
        StoneAxe = new StoneAxe();
        IronAxe = new IronAxe();
        StonePickaxe = new StonePickaxe();
        IronPickaxe = new IronPickaxe();
        StoneSword = new StoneSword();
        IronSword = new IronSword();
        GlomtomSword = new GlomtomSword();

        // Initialisiere die Recipe-Instanz mit den jetzt vorhandenen Items
        StoneAxeRecipy = new Recipies(Stick, Stone, null, null, 2, 3, 0, 0, StoneAxe, 1);
        IronAxeRecipy = new Recipies(Stick, Iron, null, null, 2, 3, 0, 0, IronAxe, 1);
        StonePickaxeRecipy = new Recipies(Stone, Stick, null, null, 4, 2, 0, 0, StonePickaxe, 1);
        IronPickaxeRecipy = new Recipies(Iron, Stick, null, null, 4, 2, 0, 0, IronPickaxe, 1);
        StoneSwordRecipy = new Recipies(Stone, Stick, null, null, 3, 1, 0, 0, StoneSword, 1);
        IronSwordRecipy = new Recipies(Iron, Stick, null, null, 3, 1, 0, 0, IronSword, 1);
        GlomtomSwordRecipy = new Recipies(Glomtom, Stick, null, null, 3, 1, 0, 0, GlomtomSword, 1);
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
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
