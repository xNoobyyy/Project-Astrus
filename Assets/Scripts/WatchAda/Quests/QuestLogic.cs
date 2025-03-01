using System.Collections.Generic;
using Items;
using Items.Items;
using Player.Inventory;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class QuestLogic : MonoBehaviour {
    public TextMeshProUGUI mainTitle;
    public TextMeshProUGUI mainDescription;
    public TextMeshProUGUI mainProgress;
    public TextMeshProUGUI sideText;
    public GameObject sideDescription;
    public List<QuestGroup> questGroups = new List<QuestGroup>();
    public int activeGroup = 0;
    
    public Quest festlandQuest;

    public Quest baueQuest;
    public Quest bootQuest;
    public Quest tierQuest;
    public Quest betreteFestlandQuest;
    
    public Quest craftingQuest;

    public Quest eOSQuest;
    public Quest spitzAxtQuest;
    public Quest schwertQuest;
    
    public Quest erkundenQuest;

    public Quest zweiTiereQuest;
    public Quest plateauQuest;
    
    public Quest überlebenQuest;

    public Quest verfolgenQuest;
    public Quest rennenQuest;
    public Quest schutzQuest;
    
    public Quest höhleQuest;

    public Quest fakelQuest;
    public Quest stormQuest;
    public Quest erzQuest;
    public Quest glomtomSchwertQuest;
    
    public Quest zombie2Quest;
    
    public Quest zombie3Quest;
    public Quest flussQuest;
    
    public Quest plateau2Quest;

    public Quest lianeQuest;
    public Quest erklimmeQuest;
    public Quest schatzkarteQuest;
    public Quest RezepteQuest;
    
    public Quest ExtricQuest;

    public Quest blumeQuest;
    public Quest ressourcenQuest;
    public Quest craftenExtricQuest;
    public Quest rüstungQuest;

    public Quest schatzQuest;

    public Quest bogenQuest;
    public Quest sumpfQuest;
    public Quest zombie4Quest;
    public Quest findeDomilitantQuest;
    public Quest tagebuchQuest;
    
    public Quest domilitantQuest;

    public Quest rezeptFindenQuest;
    public Quest trankQuest;
    
    public Quest stadtQuest;
    
    public Quest crafteSchwertQuest;
    public Quest unsichtbarQuest;
    public Quest wächterQuest;
    
    public Quest LaborQuest;

    public Quest rettenQuest;
    public Quest virusQuest;
    
    public Quest endkampfQuest;
    
    public Quest besiegenQuest;
    public Quest kontrolleQuest;
    public Quest befreienQuest;
    public Quest astrusQuest;
    
    public Quest verlassenQuest;

    public Quest reparierenQuest;
    public Quest nachHauseQuest;
    
    public List<Quest> sideQuests = new List<Quest>();
    public static QuestLogic Instance { get; private set; }

    
    void Start() {
    // Gruppe 1: Festland
        festlandQuest = new Quest("Festland", "Überquere das Meer.", true, 4);
        baueQuest = new Quest("Holz", "Sammle Holz und Äste.", false, 1);
        baueQuest.AddCondition(new ItemCondition("Stick"));
        Debug.Log("Bedingung hinzugefügt");
        bootQuest = new Quest("Boot", "Baue ein Boot.", false, 1);
        tierQuest = new Quest("Tier", "Streichele ein friedliches Tier.", false, 1);
        betreteFestlandQuest = new Quest("Ankunft", "Betrete das Festland.", false, 1);
        QuestGroup group1 = new QuestGroup(festlandQuest, new List<Quest>() { baueQuest, bootQuest, tierQuest, betreteFestlandQuest });

        // Gruppe 2: Crafting
        craftingQuest = new Quest("Werkzeuge", "Erstelle Werkzeuge und Waffen.", true, 3);
        eOSQuest = new Quest("Eisen/Stein", "Finde Eisen oder Stein.", false, 1);
        spitzAxtQuest = new Quest("Spitzhacke", "Baue eine Spitzhacke.", false, 1);
        schwertQuest = new Quest("Schwert", "Erstelle ein Schwert.", false, 1);
        QuestGroup group2 = new QuestGroup(craftingQuest, new List<Quest>() { eOSQuest, spitzAxtQuest, schwertQuest });

        // Gruppe 3: Erkundung
        erkundenQuest = new Quest("Erkundung", "Erkunde den Planeten.", true, 2);
        zweiTiereQuest = new Quest("Tiere", "Treffe zwei friedliche Tiere.", false, 1);
        plateauQuest = new Quest("Plateau", "Finde das Plateau.", false, 1);
        QuestGroup group3 = new QuestGroup(erkundenQuest, new List<Quest>() { zweiTiereQuest, plateauQuest });

        // Gruppe 4: Erste Zombieattacke
        überlebenQuest = new Quest("Zombies", "Überlebe die Zombieattacke.", true, 3);
        verfolgenQuest = new Quest("Verfolgen", "Folge der Gestalt.", false, 1);
        rennenQuest = new Quest("Rennen", "Renne vor Zombies.", false, 1);
        schutzQuest = new Quest("Schutz", "Finde Höhlenschutz.", false, 1);
        QuestGroup group4 = new QuestGroup(überlebenQuest, new List<Quest>() { verfolgenQuest, rennenQuest, schutzQuest });

        // Gruppe 5: Höhle
        höhleQuest = new Quest("Höhle", "Erkunde die Höhle.", true, 4);
        fakelQuest = new Quest("Fackel", "Baue eine Fackel.", false, 1);
        stormQuest = new Quest("Dr. Storm", "Sprich mit Dr. Storm.", false, 1);
        erzQuest = new Quest("Erz", "Baue unbekanntes Erz ab.", false, 1);
        glomtomSchwertQuest = new Quest("Glomtom", "Erstelle Glomtom-Schwert.", false, 1);
        QuestGroup group5 = new QuestGroup(höhleQuest, new List<Quest>() { fakelQuest, stormQuest, erzQuest, glomtomSchwertQuest });

        // Gruppe 6: Zweite Zombieattacke
        zombie2Quest = new Quest("Zombies II", "Entkomme der zweiten Attacke.", true, 2);
        zombie3Quest = new Quest("Kampf", "Nutze das Glomtom-Schwert.", false, 1);
        flussQuest = new Quest("Fluss", "Fliehe über den Fluss.", false, 1);
        QuestGroup group6 = new QuestGroup(zombie2Quest, new List<Quest>() { zombie3Quest, flussQuest });

        // Gruppe 7: Plateau
        plateau2Quest = new Quest("Plateau", "Erkunde das Plateau.", true, 4);
        lianeQuest = new Quest("Liane", "Sammle eine Liane.", false, 1);
        erklimmeQuest = new Quest("Erklimme", "Klettere das Plateau hinauf.", false, 1);
        schatzkarteQuest = new Quest("Karte", "Finde die Schatzkarte.", false, 1);
        RezepteQuest = new Quest("Rezepte", "Entdecke alte Rezepte.", false, 1);
        QuestGroup group7 = new QuestGroup(plateau2Quest, new List<Quest>() { lianeQuest, erklimmeQuest, schatzkarteQuest, RezepteQuest });

        // Gruppe 8: Extric
        ExtricQuest = new Quest("Extric", "Nutze Extric.", true, 4);
        blumeQuest = new Quest("Blume", "Finde die besondere Blume.", false, 1);
        ressourcenQuest = new Quest("Ressourcen", "Sammle alle Ressourcen.", false, 1);
        craftenExtricQuest = new Quest("Extric Bau", "Stelle Extric her.", false, 1);
        rüstungQuest = new Quest("Rüstung", "Erstelle Extric-Rüstung.", false, 1);
        QuestGroup group8 = new QuestGroup(ExtricQuest, new List<Quest>() { blumeQuest, ressourcenQuest, craftenExtricQuest, rüstungQuest });

        // Gruppe 9: Zombieschatz
        schatzQuest = new Quest("Schatz", "Finde den Zombieschatz.", true, 5);
        bogenQuest = new Quest("Bogen", "Baue einen Bogen.", false, 1);
        sumpfQuest = new Quest("Sumpf", "Betrete den Sumpf.", false, 1);
        zombie4Quest = new Quest("Zombies", "Besiege die Zombies.", false, 1);
        findeDomilitantQuest = new Quest("Domilitant", "Finde Domilitant.", false, 1);
        tagebuchQuest = new Quest("Tagebuch", "Lies das Tagebuch.", false, 1);
        QuestGroup group9 = new QuestGroup(schatzQuest, new List<Quest>() { bogenQuest, sumpfQuest, zombie4Quest, findeDomilitantQuest, tagebuchQuest });

        // Gruppe 10: Domilitant
        domilitantQuest = new Quest("Domilitant", "Setze Domilitant ein.", true, 2);
        rezeptFindenQuest = new Quest("Rezept", "Finde ein Rezept.", false, 1);
        trankQuest = new Quest("Trank", "Braue einen Trank.", false, 1);
        QuestGroup group10 = new QuestGroup(domilitantQuest, new List<Quest>() { rezeptFindenQuest, trankQuest });

        // Gruppe 11: Stadt
        stadtQuest = new Quest("Stadt", "Betrete die Stadt.", true, 3);
        crafteSchwertQuest = new Quest("Schwert", "Erstelle ein neues Schwert.", false, 1);
        unsichtbarQuest = new Quest("Unsichtbar", "Trinke den Trank.", false, 1);
        wächterQuest = new Quest("Wächter", "Vermeide Wächter-Zombies.", false, 1);
        QuestGroup group11 = new QuestGroup(stadtQuest, new List<Quest>() { crafteSchwertQuest, unsichtbarQuest, wächterQuest });

        // Gruppe 12: Labor
        LaborQuest = new Quest("Labor", "Erkunde das Labor.", true, 2);
        rettenQuest = new Quest("Rette", "Rette dich ins Labor.", false, 1);
        virusQuest = new Quest("Virus", "Finde Virus-Hinweise.", false, 1);
        QuestGroup group12 = new QuestGroup(LaborQuest, new List<Quest>() { rettenQuest, virusQuest });

        // Gruppe 13: Endkampf
        endkampfQuest = new Quest("Endkampf", "Gewinne den Endkampf.", true, 4);
        besiegenQuest = new Quest("Boss", "Besiege den Endboss.", false, 1);
        kontrolleQuest = new Quest("Kontrolle", "Enthülle Zombie-Kontrolle.", false, 1);
        befreienQuest = new Quest("Befreien", "Befreie die Zombies.", false, 1);
        astrusQuest = new Quest("Astrus", "Finde Astrus.", false, 1);
        QuestGroup group13 = new QuestGroup(endkampfQuest, new List<Quest>() { besiegenQuest, kontrolleQuest, befreienQuest, astrusQuest });

        // Gruppe 14: Abreise
        verlassenQuest = new Quest("Abreise", "Verlasse den Planeten.", true, 2);
        reparierenQuest = new Quest("Raumschiff", "Repariere das Schiff.", false, 1);
        nachHauseQuest = new Quest("Heimkehr", "Fliege nach Hause.", false, 1);
        QuestGroup group14 = new QuestGroup(verlassenQuest, new List<Quest>() { reparierenQuest, nachHauseQuest });

        // Alle Gruppen zur Hauptliste hinzufügen
        questGroups = new List<QuestGroup>() {
            group1, group2, group3, group4, group5, group6, group7, group8,
            group9, group10, group11, group12, group13, group14
        };

        UpdateSideQuests();
        UpdateMainQuest();
        
    }

    // Methode zur Erstellung und Initialisierung einer Quest
    //private Quest CreateQuest(string objectName, string title, string description, bool isMainQuest, int requiredProgress)
    //{
        //GameObject go = new GameObject(objectName);
        //Quest quest = go.AddComponent<Quest>();
        //quest.title = title;
        //quest.description = description;
        //quest.isMainQuest = isMainQuest;
        //quest.requiredProgress = requiredProgress;
        //quest.currentProgress = 0;
        //return quest;
    //}

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            bool shouldStop = false;
            foreach (var quest in questGroups[activeGroup].subQuests) {
                foreach (var condition in quest.conditions) {
                    if (!condition.IsMet()) {
                        shouldStop = true;
                        break;
                    }
                }
                if (shouldStop) {
                    continue;
                }
                FinishSideQuest(quest);
            }
            UpdateSideQuests();
            UpdateMainQuest();
        }
    }

    public void UpdateMainQuest() {
        Quest quest = questGroups[activeGroup].mainQuest;
        quest.currentProgress = 0;
        foreach (var sideQuest in questGroups[activeGroup].subQuests) {
            if (sideQuest.currentProgress == sideQuest.requiredProgress) {
                quest.currentProgress++;
            }
        }
        mainTitle.text = quest.title;
        mainDescription.text = quest.description;
        mainProgress.text = quest.currentProgress.ToString() + "/" + quest.requiredProgress.ToString();
        UpdateSideQuests();
        if (quest.currentProgress == quest.requiredProgress) {
            FinishMainQuest();
        }
    }

    public void FinishMainQuest() {
        activeGroup++;
        UpdateMainQuest();
    }

    public void FinishSideQuest(Quest quest) {
        quest.currentProgress = quest.requiredProgress;
    }

    public void FinishSideQuest() {
        foreach (Quest quest in questGroups[activeGroup].subQuests) {
            if(quest.currentProgress >= quest.requiredProgress) {} else {
                quest.currentProgress = quest.requiredProgress;
                break;
            }
        }
    }

    public void UpdateSideQuests()
    {
        sideText.text = "";
        foreach (Quest quest in questGroups[activeGroup].subQuests)
        {
            sideText.text += quest.title + " - " + quest.description + " (" +
                              quest.currentProgress.ToString() + "/" + quest.requiredProgress.ToString() + ")" + "\n\n" ;
        }
    }
    private GameObject[] allSlots;
    private GameObject slotsContainer;
    public static List<Item> ItemSlots = new List<Item>();
    public void Slots() {
        slotsContainer = GameObject.FindWithTag("Slots");
        if (slotsContainer == null) {
            Debug.LogError("SlotsContainer nicht gefunden!");
            return;
        }
        ItemSlots = GetAllItems(slotsContainer);
        foreach (var item in ItemSlots) {
            Debug.Log(item.Name);
        }
    }
    private List<GameObject> GetAllChildren(GameObject parent) {
        List<GameObject> childrenList = new List<GameObject>();
        foreach (Transform child in parent.transform) {
            childrenList.Add(child.gameObject);
            childrenList.AddRange(GetAllChildren(child.gameObject));
        }
        return childrenList;
    }
    private List<Item> GetAllItems(GameObject parent) {

        List<GameObject> allChildren = GetAllChildren(parent);
        List<Item> allItems = new List<Item>();
        foreach (GameObject child in allChildren) {
            if (child.TryGetComponent(out ItemSlot itemSlot) && itemSlot.Item != null) {
                Debug.Log($"Item gefunden in {child.name}: {itemSlot.Item}");
                allItems.Add(itemSlot.Item);
            }
        }
        return allItems;
    }
}

[System.Serializable]
public class QuestGroup {
    public Quest mainQuest;
    public List<Quest> subQuests;
    
    public QuestGroup(Quest mainQuest, List<Quest> subQuests = null) {
        this.mainQuest = mainQuest;
        this.subQuests = subQuests ?? new List<Quest>();
    }
}
