using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Creatures;
using Player.Inventory;
using Items;
using Items.Items;
using Logic.Events;
using TextDisplay;
using Utils;
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
    public Dictionary<QuestCondition, bool> Conditions = new();
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
        QuestLogic.Instance.UpdateMainQuest();
        QuestLogic.Instance.UpdateSideQuests();

        if (currentProgress < requiredProgress) return;

        currentProgress = requiredProgress;
        CompleteQuest();
    }

    public void CompleteCondition(QuestCondition condition) {
        Debug.Log("Completing condition " + condition);
        if (!Conditions.ContainsKey(condition)) return;

        Debug.Log("Completed condition " + condition);
        Conditions[condition] = true;
        UpdateProgress(1);
    }

    /// <summary>
    /// Markiert die Quest als abgeschlossen und ruft das Abschluss-Event auf.
    /// </summary>
    public void CompleteQuest() {
        foreach (var condition in Conditions.Keys.ToList()) {
            Conditions[condition] = true;
        }

        isCompleted = true;
        QuestLogic.Instance.UpdateMainQuest();
        QuestLogic.Instance.UpdateSideQuests();

        if (textbaustein != 1000) TextDisplayManager.Instance.textDisplay.Notification(textbaustein);
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
        Conditions[condition] = false;
    }
}

public abstract class QuestCondition { }

public abstract class GenericQuestCondition<T> : QuestCondition { }

public sealed class ItemCondition : GenericQuestCondition<Item> {
    private readonly Type itemType;

    public ItemCondition(Type itemType) {
        this.itemType = itemType;
    }

    public bool IsMet(Item check) {
        return check.GetType() == itemType;
    }
}

public sealed class EnteredCondition : GenericQuestCondition<Area> {
    private readonly AreaType areaType;

    public EnteredCondition(AreaType areaType) {
        this.areaType = areaType;
    }

    public bool IsMet(Area check) {
        return check.type == areaType;
    }
}

public sealed class InteractingCondition : GenericQuestCondition<CreatureInteractEvent> {
    private readonly CreatureType[] creatureTypes;
    private readonly InteractionType interactionType;

    public InteractingCondition(CreatureType[] creatureTypes, InteractionType interactionType) {
        this.creatureTypes = creatureTypes;
        this.interactionType = interactionType;
    }

    public InteractingCondition(CreatureType creatureType, InteractionType interactionType) {
        this.creatureTypes = new[] { creatureType };
        this.interactionType = interactionType;
    }

    public bool IsMet(CreatureInteractEvent check) {
        return creatureTypes.Contains(check.Creature.type) && check.InteractionType == interactionType;
    }
}