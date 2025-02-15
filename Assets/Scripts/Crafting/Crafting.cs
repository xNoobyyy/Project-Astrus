using Items;
using JetBrains.Annotations;
using UnityEngine;

public class Crafting : MonoBehaviour {
    public GameObject CraftingSlot1;
    public GameObject CraftingSlot2;
    public GameObject CraftingSlot3;
    public GameObject CraftingSlot4;

    public Item CraftingItem1; 
    public Item CraftingItem2;
    public Item CraftingItem3;
    public Item CraftingItem4;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        CraftingItem1 = CraftingSlot1.GetComponent<Item>();
        CraftingItem2 = CraftingSlot2.GetComponent<Item>();
        CraftingItem3 = CraftingSlot3.GetComponent<Item>();
        CraftingItem4 = CraftingSlot4.GetComponent<Item>();
        
        
    }
}
