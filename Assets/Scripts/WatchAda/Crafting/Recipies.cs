using System.Collections.Generic;
using Items;

public class Recipies {
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
    public bool advanced; // Found on the plateau

    // Konstruktor
    public Recipies(Item item1, Item item2, Item item3, Item item4,
                    int? numberOfItem1, int? numberOfItem2, int? numberOfItem3, int? numberOfItem4,
                    Item craftedItem, int craftedAmount, bool advanced) {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        Item4 = item4;
        NumberOfItem1 = numberOfItem1;
        NumberOfItem2 = numberOfItem2;
        NumberOfItem3 = numberOfItem3;
        NumberOfItem4 = numberOfItem4;
        CraftedItem = craftedItem;
        CraftedAmount = craftedAmount;
        this.advanced = advanced;
    }

    /// <summary>
    /// Prüft, ob die (Item, Anzahl)-Paare des Rezepts exakt mit den (Item, Anzahl)-Paaren aus den Slots übereinstimmen.
    /// Dabei wird nicht die Referenz, sondern die exakte Klasse der Items verglichen.
    /// Null-Werte werden dabei korrekt berücksichtigt.
    /// Die Reihenfolge der Paare spielt keine Rolle.
    /// </summary>
    public bool checkRecipy(
        Item ItemInSlot1, Item ItemInSlot2, Item ItemInSlot3, Item ItemInSlot4,
        int? NumberInSlot1, int? NumberInSlot2, int? NumberInSlot3, int? NumberInSlot4) {

        // Erstelle Listen der (Item, Anzahl)-Paare für Rezept und Slots
        var recipePairs = new List<(Item item, int? count)> {
            (Item1, NumberOfItem1),
            (Item2, NumberOfItem2),
            (Item3, NumberOfItem3),
            (Item4, NumberOfItem4)
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
                // Prüfe Items anhand ihres Typs (null-sicher):
                bool itemsMatch;
                if (recipePair.item == null && slotPairs[i].item == null) {
                    itemsMatch = true;
                } else if (recipePair.item != null && slotPairs[i].item != null) {
                    itemsMatch = recipePair.item.GetType() == slotPairs[i].item.GetType();
                } else {
                    itemsMatch = false;
                }

                // Vergleiche zusätzlich die Mengen (nullable int, null-sicher)
                if (itemsMatch && Equals(slotPairs[i].count, recipePair.count)) {
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
