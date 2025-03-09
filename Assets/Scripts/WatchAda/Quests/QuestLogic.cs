using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Items;
using Items.Items;
using Logic.Events;
using Player.Inventory;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using Debug = UnityEngine.Debug;

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

    public Quest eQuest;
    public Quest sQuest;
    public Quest spitzAxtQuest;
    public Quest schwertQuest;

    public Quest erkundenQuest;

    public Quest zweiTiereQuest;
    public Quest plateauQuest;

    public Quest überlebenQuest;

    public Quest angreifen2Quest;
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
    public Quest laborVerlassenQuest;

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
    public List<string> orte = new List<string>();
    public List<string> interaktionen = new List<string>();

    private void OnEnable() {
        EventManager.Instance.Subscribe<PlayerItemEvent>(OnItemPickup);
        EventManager.Instance.Subscribe<PlayerAreaEnterEvent>(OnAreaEntered);
        EventManager.Instance.Subscribe<CreatureDamageEvent>(OnCreatureDamage);
    }

    private void OnDisable() {
        EventManager.Instance.Unsubscribe<PlayerItemEvent>(OnItemPickup);
        EventManager.Instance.Unsubscribe<PlayerAreaEnterEvent>(OnAreaEntered);
        EventManager.Instance.Unsubscribe<CreatureDamageEvent>(OnCreatureDamage);
    }

    private void OnAreaEntered(PlayerAreaEnterEvent e) {
        orte.Add(e.AreaType.ToString());
        foreach (var q in questGroups[activeGroup].subQuests) {
                foreach (var condition in q.conditions) {
                    if (condition.GetType() == typeof(EnteredCondition)) {
                        condition.IsMet();
                    }
                }
        }
    }

    private void OnItemPickup(PlayerItemEvent e) {

        foreach (var q in questGroups[activeGroup].subQuests) {
            foreach (var condition in q.conditions) {
                if (condition.GetType() == typeof(ItemCondition)) {
                    condition.IsMet();
                }
            }

            foreach (var condition in from questGroup in questGroups
                     from quest in questGroup.subQuests
                     from condition in quest.conditions
                     where condition.GetType() == typeof(ItemCondition)
                     select condition) {
                condition.IsMet();
            }
        }
    }

    private void OnCreatureDamage(CreatureDamageEvent e) {
        interaktionen.Add(e.CreatureType.ToString());
        foreach (var q in questGroups[activeGroup].subQuests) {
            foreach (var condition in q.conditions) {
                if (condition.GetType() == typeof(InteractingCondition)) {
                    condition.IsMet();
                }
            }
        }
    }
    private void HandleAreaEntered(string area) {
        orte.Add(area);
    }

    void Start() {
        // Gruppe 1: Festland FERTIG
        festlandQuest = new Quest("id_festland_main","Festland", "Überquere das Meer.", true, 4);
        baueQuest = new Quest("","id_festland_holz", "Sammle Holz und Äste.", false, 2, 8);
        baueQuest.AddCondition(new ItemCondition("Stick"));
        baueQuest.AddCondition(new ItemCondition("Wood"));
        bootQuest = new Quest("id_festland_boot","Boot", "Baue ein Boot.", false, 1, 11);
        bootQuest.AddCondition(new CraftingCondition("Boat"));
        tierQuest = new Quest("id_festland_tier","Tier", "Streichele ein Tier.", false, 1); 
        tierQuest.AddCondition(new InteractingCondition("Tier", 1));
        betreteFestlandQuest = new Quest("id_festland_ankunft","Ankunft", "Betrete das Festland.", false, 1, 12);
        betreteFestlandQuest.AddCondition(new EnteredCondition("Beach"));
        QuestGroup group1 = new QuestGroup(festlandQuest,
            new List<Quest>() { baueQuest, bootQuest, tierQuest, betreteFestlandQuest });

        // Gruppe 2: Crafting FERTIG
        craftingQuest = new Quest("id_crafting_main","Werkzeuge", "Erstelle Werkzeuge und Waffen.", true, 4, 15);
        eQuest = new Quest("id_crafting_stein","Stein", "Finde Stein", false, 1, 14);
        eQuest.AddCondition(new ItemCondition("Stone"));
        sQuest = new Quest("id_crafting_eisen","Eisen", "Finde Eisen", false, 1);
        sQuest.AddCondition(new ItemCondition("Iron"));
        spitzAxtQuest = new Quest("id_crafting_spitzhacke","Spitzhacke", "Baue eine Spitzhacke aus Stein.", false, 1);
        spitzAxtQuest.AddCondition(new CraftingCondition("StonePickaxe"));
        schwertQuest = new Quest("id_crafting_schwert", "Schwert", "Erstelle ein Schwert aus Eisen.", false, 1);
        schwertQuest.AddCondition(new CraftingCondition("IronSword"));
        var group2 = new QuestGroup(
            craftingQuest,
            new List<Quest> { eQuest, sQuest, spitzAxtQuest, schwertQuest }
        );
        // Gruppe 3: Erkundung
        erkundenQuest = new Quest("id_erkundung_main", "Erkundung", "Erkunde den Planeten.", true, 2);
        zweiTiereQuest = new Quest("id_erkundung_tiere", "Tiere", "Treffe zwei friedliche Tiere.", false, 1);
        zweiTiereQuest.AddCondition(new InteractingCondition("Tier", 2));
        plateauQuest = new Quest("id_erkundung_plateau", "Plateau", "Finde das Plateau.", false, 1, 16);
        plateauQuest.AddCondition(new EnteredCondition("Plateauumgebung"));///////////////////////////////////////////////////////
        var group3 = new QuestGroup(
            erkundenQuest,
            new List<Quest> { zweiTiereQuest, plateauQuest }
        );
        
        // Gruppe 4: Erste Zombieattacke FERTIG
        überlebenQuest = new Quest("id_zombies_main","Zombies", "Überlebe die Zombieattacke.", true, 2);
        angreifen2Quest = new Quest("id_zombies_angriff","Angreifen", "Greife einen Zombie an", false, 1, 21);
        angreifen2Quest.AddCondition(new InteractingCondition("Zombie", 1));
        schutzQuest = new Quest("id_zombies_schutz","Schutz", "Bringe dich in Sicherheit.", false, 1, 26);
        schutzQuest.AddCondition(new EnteredCondition("Cave"));
        var group4 = new QuestGroup(
            überlebenQuest,
            new List<Quest> { angreifen2Quest, schutzQuest }
        );
        // Gruppe 5: Höhle
        höhleQuest = new Quest("id_hoehle_main","Höhle", "Erkunde die Höhle.", true, 4, 52);
        fakelQuest = new Quest("id_hoehle_fackel","Fackel", "Baue eine Fackel.", false, 1, 28);
        fakelQuest.AddCondition(new CraftingCondition("Torch"));
        stormQuest = new Quest("id_hoehle_storm","Dr. Storm", "Sprich mit Dr. Storm.", false, 1, 48); //!
        erzQuest = new Quest("id_hoehle_erz","Erz", "Baue unbekanntes Erz ab.", false, 1, 49);
        erzQuest.AddCondition(new ItemCondition("Glomtom"));
        glomtomSchwertQuest = new Quest("id_hoehle_glomtom","Glomtom", "Erstelle Glomtom-Schwert.", false, 1, 50);
        glomtomSchwertQuest.AddCondition(new CraftingCondition("GlomtomSword"));
        var group5 = new QuestGroup(
            höhleQuest,
            new List<Quest> { fakelQuest, stormQuest, erzQuest, glomtomSchwertQuest }
        );
            
        // Gruppe 6: Zweite Zombieattacke
        zombie2Quest = new Quest("id_zombies_second_main","Zombies II", "Entkomme der zweiten Attacke.", true, 2, 53);
        zombie3Quest = new Quest("id_zombies_kampf","Kampf", "Nutze das Glomtom-Schwert.", false, 1); //!
        flussQuest = new Quest("id_zombies_fluss","Fluss", "Überquere die Seerosen.", false, 1);
        flussQuest.AddCondition(new EnteredCondition("Fluss")); ////////////////////
        var group6 = new QuestGroup(
            zombie2Quest,
            new List<Quest> { zombie3Quest, flussQuest }
        );

        // Gruppe 7: Plateau
        plateau2Quest = new Quest("id_plateau_main", "Plateau", "Erkunde das Plateau.", true, 4);
        lianeQuest = new Quest("id_plateau_liane", "Liane", "Sammle eine Liane.", false, 1);
        lianeQuest.AddCondition(new ItemCondition("Liana"));
        erklimmeQuest = new Quest("id_plateau_erklimme","Erklimme", "Klettere das Plateau hinauf.", false, 1, 56);
        erklimmeQuest.AddCondition(new EnteredCondition("Plateau"));
        schatzkarteQuest = new Quest("id_plateau_karte","Karte", "Entdecke die Geheimnisse der Farm.", false, 1, 58); //!
        RezepteQuest = new Quest("id_plateau_rezepte","Rezepte", "Entdecke alte Rezepte.", false, 1, 60); //!
        var group7 = new QuestGroup(
            plateau2Quest,
            new List<Quest> { lianeQuest, erklimmeQuest, schatzkarteQuest, RezepteQuest }
        );
        
        // Gruppe 8: Extric
        ExtricQuest = new Quest("id_extric_main","Extric", "Nutze Extric.", true, 4, 62);
        blumeQuest = new Quest("id_extric_blume","Blume", "Finde die besondere Blume.", false, 1);
        blumeQuest.AddCondition(new ItemCondition("SpecialFlower"));
        ressourcenQuest = new Quest("id_extric_ressourcen", "Ressourcen", "Sammle alle Ressourcen.", false, 7);
        ressourcenQuest.AddCondition(new ItemCondition("SpecialFlower"));
        ressourcenQuest.AddCondition(new ItemCondition("Wood"));
        ressourcenQuest.AddCondition(new ItemCondition("Stick"));
        ressourcenQuest.AddCondition(new ItemCondition("Coal"));
        ressourcenQuest.AddCondition(new ItemCondition("Stone"));
        ressourcenQuest.AddCondition(new ItemCondition("Iron"));
        ressourcenQuest.AddCondition(new ItemCondition("Liana"));
        craftenExtricQuest = new Quest("id_extric_bau", "Extric Bau", "Stelle Extric her.", false, 1);
        craftenExtricQuest.AddCondition(new CraftingCondition("Extric"));
        rüstungQuest = new Quest("id_extric_ruestung","Rüstung", "Erstelle Extric-Rüstung.", false, 1, 61);
        rüstungQuest.AddCondition(new CraftingCondition("ExtricAmor"));
        var group8 = new QuestGroup(
            ExtricQuest,
            new List<Quest> { blumeQuest, ressourcenQuest, craftenExtricQuest, rüstungQuest }
        );

        // Gruppe 9: Zombieschatz
        schatzQuest = new Quest("id_schatz_main","Schatz", "Finde den Zombieschatz.", true, 5, 71);
        bogenQuest = new Quest("id_schatz_bogen","Bogen", "Baue einen Bogen aus Eisen.", false, 1);
        bogenQuest.AddCondition(new CraftingCondition("IronBow"));
        sumpfQuest = new Quest("id_schatz_sumpf","Sumpf", "Betrete den Sumpf.", false, 1, 63);
        sumpfQuest.AddCondition(new EnteredCondition("Swamp"));
        findeDomilitantQuest = new Quest("id_schatz_domilitant","Domilitant", "Finde Domilitant.", false, 1, 66);
        findeDomilitantQuest.AddCondition(new ItemCondition("Domilitant"));
        tagebuchQuest = new Quest("id_schatz_tagebuch","Tagebuch", "Finde den verborgenen Hinweis.", false, 1, 69); //!
        var group9 = new QuestGroup(
            schatzQuest,
            new List<Quest> { bogenQuest, sumpfQuest, zombie4Quest, findeDomilitantQuest, tagebuchQuest }
        );
        
        // Gruppe 10: Domilitant
        domilitantQuest = new Quest("id_domilitant_main","Domilitant", "Setze Domilitant ein.", true, 2);
        rezeptFindenQuest = new Quest("id_domilitant_rezept","Rezept", "Finde ein Rezept.", false, 1); //!
        trankQuest = new Quest("id_domilitant_trank","Trank", "Braue einen Trank.", false, 1, 74);
        trankQuest.AddCondition(new CraftingCondition("InvisibilityPotion"));
        var group10 = new QuestGroup(
            domilitantQuest,
            new List<Quest> { rezeptFindenQuest, trankQuest }
        );

        // Gruppe 11: Stadt
        stadtQuest = new Quest("id_stadt_main","Stadt", "Betrete die Stadt.", true, 3);
        crafteSchwertQuest = new Quest("id_stadt_schwert","Schwert", "Erstelle ein neues Schwert aus Glomtom.", false, 1, 77);
        crafteSchwertQuest.AddCondition(new CraftingCondition("GlomtomSword"));
        unsichtbarQuest = new Quest("id_stadt_unsichtbar","Unsichtbar", "Trinke den Trank.", false, 1); //!
        wächterQuest = new Quest("id_stadt_waechter","Wächter", "Betrete die Stadt.", false, 1, 80); 
        wächterQuest.AddCondition(new EnteredCondition("City"));
        var group11 = new QuestGroup(
            stadtQuest,
            new List<Quest> { crafteSchwertQuest, unsichtbarQuest, wächterQuest }
        );
        
        // Gruppe 12: Labor
        LaborQuest = new Quest("id_labor_main","Labor", "Erkunde das Labor.", true, 2);
        rettenQuest = new Quest("id_labor_rette","Rette", "Rette dich ins Labor.", false, 1, 82);
        rettenQuest.AddCondition(new EnteredCondition("Labor")); 
        virusQuest = new Quest("id_labor_virus","Virus", "Finde Virus-Hinweise.", false, 1, 84); //!
        laborVerlassenQuest = new Quest("id_labor_verlassen","Labor Verlassen", "Stelle dich dem Bosszombie", false, 1, 86);//!
        var group12 = new QuestGroup(
            LaborQuest,
            new List<Quest> { rettenQuest, virusQuest, laborVerlassenQuest }
        );
        
        // Gruppe 13: Endkampf
        endkampfQuest = new Quest("id_endkampf_main","Endkampf", "Gewinne den Endkampf.", true, 2, 96);
        besiegenQuest = new Quest("id_endkampf_boss","Boss", "Besiege den Endboss.", false, 1, 90); //1
        astrusQuest = new Quest("id_endkampf_astrus","Astrus", "Finde Astrus.", false, 1);
        astrusQuest.AddCondition(new ItemCondition("Astrus"));
        var group13 = new QuestGroup(
            endkampfQuest,
            new List<Quest> { besiegenQuest, astrusQuest }
        );
        // Gruppe 14: Abreise
        verlassenQuest = new Quest("id_abreise_main","Abreise", "Verlasse den Planeten.", true, 2);
        reparierenQuest = new Quest("id_abreise_raumschiff","Raumschiff", "Repariere das Schiff.", false, 1, 97); //!
        nachHauseQuest = new Quest("id_abreise_heimkehr","Heimkehr", "Fliege nach Hause.", false, 1); //!
        var group14 = new QuestGroup(
            verlassenQuest,
            new List<Quest> { reparierenQuest, nachHauseQuest }
        );
        // Alle Gruppen zur Hauptliste hinzufügen
        questGroups = new List<QuestGroup> {
            group1, group2, group3, group4, group5, group6,
            group7, group8, group9, group10, group11,
            group12, group13, group14
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
    /*void Update() {
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
                    shouldStop = false;
                    continue;
                }
                FinishSideQuest(quest);
            }
            UpdateSideQuests();
            UpdateMainQuest();
        }
    }*/

    public void UpdateMainQuest() {
        var quest = questGroups[activeGroup].mainQuest;
        quest.currentProgress = questGroups[activeGroup].subQuests
            .Where(sideQuest => sideQuest.currentProgress >= sideQuest.requiredProgress).ToArray().Length;

        mainTitle.text = quest.title;
        mainDescription.text = quest.description;
        mainProgress.text = quest.currentProgress + "/" + quest.requiredProgress;
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
        foreach (var quest in questGroups[activeGroup].subQuests) {
            if (quest.currentProgress >= quest.requiredProgress) { } else {
                quest.currentProgress = quest.requiredProgress;
                break;
            }
        }
    }

    public void UpdateSideQuests() {
        sideText.text = "";
        foreach (var quest in questGroups[activeGroup].subQuests) {
            sideText.text += quest.title + " - " + quest.description + " (" +
                             quest.currentProgress.ToString() + "/" + quest.requiredProgress.ToString() + ")" + "\n\n";
        }
    }

    private GameObject[] allSlots;
    private GameObject slotsContainer;
    public List<Item> ItemSlots = new List<Item>();

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
        var childrenList = new List<GameObject>();
        foreach (Transform child in parent.transform) {
            childrenList.Add(child.gameObject);
            childrenList.AddRange(GetAllChildren(child.gameObject));
        }

        return childrenList;
    }

    private List<Item> GetAllItems(GameObject parent) {
        var allChildren = GetAllChildren(parent);
        var allItems = new List<Item>();
        foreach (var child in allChildren) {
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