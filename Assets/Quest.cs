using UnityEngine;
using System;
using System.Collections.Generic;
using Player.Inventory;
using Items;
using System.Reflection;
using Items.Items;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class Quest  {
    // Der Titel der Quest
    public string title;
    
    // Eine ausführliche Beschreibung der Quest
    public string description;
    
    // Gibt an, ob es sich um die Hauptquest handelt (diese wird vorrangig angezeigt)
    public bool isMainQuest;
    
    // Flag, ob die Quest bereits abgeschlossen wurde
    public bool isCompleted;
    
    // Aktueller Fortschritt (z.B. Anzahl erledigter Aufgaben)
    public int currentProgress;
    
    // Erforderlicher Fortschritt, um die Quest abzuschließen
    public int requiredProgress;
    
    // Optional: Ein Icon, das zur Darstellung der Quest genutzt wird
    public Sprite icon;
    public List<QuestCondition> conditions = new List<QuestCondition>();
    
    /// <summary>
    /// Konstruktor für eine neue Quest.
    /// </summary>
    /// <param name="title">Titel der Quest</param>
    /// <param name="description">Beschreibung der Quest</param>
    /// <param name="isMainQuest">True, wenn es sich um die Hauptquest handelt</param>
    /// <param name="requiredProgress">Wie viel Fortschritt nötig ist, um die Quest abzuschließen</param>
    /// <param name="icon">Optionales Icon für die Quest</param>
    public Quest(string title, string description, bool isMainQuest, int requiredProgress, Sprite icon = null) {
        this.title = title;
        this.description = description;
        this.isMainQuest = isMainQuest;
        this.requiredProgress = requiredProgress;
        this.icon = icon;
        
        // Initialwerte setzen
        currentProgress = 0;
        isCompleted = false;
    }
    
    /// <summary>
    /// Aktualisiert den Fortschritt der Quest und prüft ob sie abgeschlossen wurde
    /// </summary>
    /// <param name="amount">Die Menge, um die der Fortschritt erhöht werden soll.</param>
    public void UpdateProgress(int amount) {
        if (isCompleted) return;
        
        currentProgress += amount;
        
        if (currentProgress >= requiredProgress) {
            currentProgress = requiredProgress;
            CompleteQuest();
        }
    }
    /// <summary>
    /// Markiert die Quest als abgeschlossen und ruft das Abschluss-Event auf.
    /// </summary>
    private void CompleteQuest() 
    {
        isCompleted = true;
    }
    
    /// <summary>
    /// Gibt den Fortschritt als Prozentsatz (0 bis 1) zurück.
    /// </summary>
    public float GetProgressPercentage() {
        if (requiredProgress == 0)
            return 1f;
        return (float)currentProgress / requiredProgress;
    }
    public void AddCondition(QuestCondition condition)
    {
        conditions.Add(condition);
    }

    public bool IsCompleted()
    {
        foreach (QuestCondition condition in conditions) {
            if (condition.IsMet()) {
            } else {
                return false;
            }
        }
        return true;
    }
}

public abstract class QuestCondition {
    public abstract bool IsMet();
}

public class CraftingCondition : QuestCondition {
    public string itemName;
    public static Item craftedItem = new Astrus(1);
    public override bool IsMet(){
        if (craftedItem.Name == itemName) {
            
            return true;
        }
        return false;
    }
    public CraftingCondition(string itemName) {
        this.itemName = itemName;
    }
}

public class EventM {
    public void Subscribe(EventCrafting publisher) {
        publisher.OnCrafting += HandleEvent; // Registriere die Methode für das Event
    }

    private void HandleEvent() {
        Debug.Log("hbvdsj");
        foreach (var questG in QuestLogic.questGroups) {
            foreach (var quest in questG.subQuests) {
                foreach (var condition in quest.conditions) {
                    if (condition.GetType() == typeof(CraftingCondition)) {
                        if (condition.IsMet()) {
                            quest.currentProgress = quest.requiredProgress;
                        }
                    }
                }
            }
        }
    }
}
public class ItemCondition : QuestCondition {
    public string itemName;
    
    public ItemCondition(String itemName) {
            Debug.Log($"ItemCondition erstellt mit Item: {itemName}");
        this.itemName = itemName;
    }

    public override bool IsMet() {
        Debug.Log(itemName);
        return ContainsItem(itemName);
    }
    public bool ContainsItem(string name) {
        if (string.IsNullOrEmpty(name)) {
            Debug.LogError("Fehler: Der übergebene String ist null oder leer!");
            return false;
        }
        foreach (Item obj in QuestLogic.ItemSlots) {
            if (obj != null && obj.Name == name) { 
                return true;
            }
        }
        return false;
    }
}
public class EventCrafting {
    public event Action OnCrafting;

    public void TriggerEvent() {
        OnCrafting?.Invoke(); 
    }
}