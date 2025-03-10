using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Player.Inventory;
using Items;
using Items.Items;
using WatchAda.Quests;

[System.Serializable]
public class Quest {
    // ID der Quest
    public string id;

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
    public int textbaustein;

    /// <summary>
    /// Konstruktor für eine neue Quest.
    /// </summary>
    /// <param name="id">idfk id?</param>
    /// <param name="title">Titel der Quest</param>
    /// <param name="description">Beschreibung der Quest</param>
    /// <param name="isMainQuest">True, wenn es sich um die Hauptquest handelt</param>
    /// <param name="requiredProgress">Wie viel Fortschritt nötig ist, um die Quest abzuschließen</param>
    /// <param name="textbaustein">welcher textbaustein oder 1000 wenn nicht defined ig</param>
    /// <param name="icon">Optionales Icon für die Quest</param>
    public Quest(string id, string title, string description, bool isMainQuest, int requiredProgress,
        int textbaustein = 1000, Sprite icon = null) {
        this.id = id;
        this.title = title;
        this.description = description;
        this.isMainQuest = isMainQuest;
        this.requiredProgress = requiredProgress;
        this.icon = icon;
        this.textbaustein = textbaustein;

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
    private void CompleteQuest() {
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

    public void AddCondition(QuestCondition condition) {
        conditions.Add(condition);
    }

    public bool IsCompleted() {
        foreach (var condition in conditions) {
            if (condition.IsMet()) { } else {
                return false;
            }
        }

        return true;
    }
}

public abstract class QuestCondition {
    protected readonly QuestLogic Ql = GameObject.FindWithTag("QuestLogic").GetComponent<QuestLogic>();
    public abstract bool IsMet();
}

public class CraftingCondition : QuestCondition {
    private readonly Type itemType;
    public static Item CraftedItem = new Astrus(1);

    public override bool IsMet() {
        return CraftedItem.GetType() == itemType;
    }

    public CraftingCondition(Type itemType) {
        this.itemType = itemType;
    }
}

public class EventM {
    private QuestLogic Ql;

    private void Start() {
        Ql = GameObject.FindWithTag("QuestLogic").GetComponent<QuestLogic>();
    }

    public void Subscribe(EventCrafting publisher) {
        Ql = GameObject.FindWithTag("QuestLogic").GetComponent<QuestLogic>();
        publisher.OnCrafting += HandleEvent; // Registriere die Methode für das Event
    }

    private void HandleEvent() {
        foreach (var quest in Ql.questGroups.SelectMany(questG =>
                     from quest in questG.subQuests
                     from condition in quest.conditions
                         .Where(condition => condition.GetType() == typeof(CraftingCondition))
                         .Where(condition => condition.IsMet())
                     select quest)) {
            quest.currentProgress = quest.requiredProgress;
        }
    }
}

public class ItemCondition : QuestCondition {
    private readonly Type itemType;

    public ItemCondition(Type itemType) {
        this.itemType = itemType;
    }

    public override bool IsMet() {
        return PlayerInventory.Instance.Slots.Select(slot => slot.Item)
            .Any(obj => obj != null && obj.GetType() == itemType);
    }
}

public class EventCrafting {
    public event Action OnCrafting;

    public void TriggerEvent() {
        OnCrafting?.Invoke();
    }
}

public class EnteredCondition : QuestCondition {
    private string area;

    public EnteredCondition(string a) {
        area = a;
    }

    public override bool IsMet() {
        return Ql.orte.Any(ort => ort == area);
    }
}

public class InteractingCondition : QuestCondition {
    private string art;
    private int anzahl;
    int erreichteAnzahl;

    public InteractingCondition(string a, int i) {
        art = a;
        anzahl = i;
    }

    public override bool IsMet() {
        erreichteAnzahl = 0;
        foreach (var interaction in Ql.interaktionen) {
            if (art == "Tier" && (interaction == "Dodo" || interaction == "Quokka" || interaction == "Golem")) {
                erreichteAnzahl++;
            }

            if (art == "Zombie" && (interaction == "Zombie") || interaction == "ZombieBoss") { }
        }

        if (erreichteAnzahl >= anzahl) {
            return true;
        }

        return false;
    }
}