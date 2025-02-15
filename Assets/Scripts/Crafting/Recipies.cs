using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

public class Recipies : MonoBehaviour {
    public Item Item1;
    public Item Item2;
    public Item Item3;
    public Item Item4;
    public int? NumberOfItem1; // nullable int, damit auch null als Wert möglich ist
    public int? NumberOfItem2;
    public int? NumberOfItem3;
    public int? NumberOfItem4;
    public Item CraftedItem;
    public int CraftedAmount;
    
    // Konstruktor: Hier werden nullable ints als Parameter erwartet
    public Recipies(Item item1, Item item2, Item item3, Item item4,
                    int? numberOfItem1, int? numberOfItem2, int? numberOfItem3, int? numberOfItem4,
                    Item craftedItem, int craftedAmount) {
        this.Item1 = item1;
        this.Item2 = item2;
        this.Item3 = item3;
        this.Item4 = item4;
        this.NumberOfItem1 = numberOfItem1;
        this.NumberOfItem2 = numberOfItem2;
        this.NumberOfItem3 = numberOfItem3;
        this.NumberOfItem4 = numberOfItem4;
        this.CraftedItem = craftedItem;
        this.CraftedAmount = craftedAmount;
    }

    /// <summary>
    /// Prüft, ob die (Item, Anzahl)-Paare des Rezepts exakt mit den (Item, Anzahl)-Paaren aus den Slots übereinstimmen.
    /// Dabei werden null-Werte (sowohl für Items als auch für Zahlen) korrekt berücksichtigt.
    /// Die Reihenfolge der Paare spielt dabei keine Rolle.
    /// </summary>
    public bool checkRecipy(
        Item ItemInSlot1, Item ItemInSlot2, Item ItemInSlot3, Item ItemInSlot4,
        int? NumberInSlot1, int? NumberInSlot2, int? NumberInSlot3, int? NumberInSlot4) {
        
        // Erstelle Listen der (Item, Anzahl)-Paare für Rezept und Slots
        var recipePairs = new List<(Item item, int? count)> {
            (this.Item1, this.NumberOfItem1),
            (this.Item2, this.NumberOfItem2),
            (this.Item3, this.NumberOfItem3),
            (this.Item4, this.NumberOfItem4)
        };
        
        var slotPairs = new List<(Item item, int? count)> {
            (ItemInSlot1, NumberInSlot1),
            (ItemInSlot2, NumberInSlot2),
            (ItemInSlot3, NumberInSlot3),
            (ItemInSlot4, NumberInSlot4)
        };
        
        // Für jedes Rezept-Paar wird in den Slot-Paaren ein passender Eintrag gesucht.
        foreach (var recipePair in recipePairs) {
            bool foundMatch = false;
            for (int i = 0; i < slotPairs.Count; i++) {
                // object.Equals ist null-sicher: Vergleicht sowohl Items als auch die Mengen (nullable ints)
                if (object.Equals(slotPairs[i].item, recipePair.item) && object.Equals(slotPairs[i].count, recipePair.count)) {
                    foundMatch = true;
                    // Entferne den gefundenen Eintrag, um Mehrfachzuordnungen zu vermeiden
                    slotPairs.RemoveAt(i);
                    break;
                }
            }
            if (!foundMatch) {
                return false;
            }
        }
        
        return true;
    }
}
