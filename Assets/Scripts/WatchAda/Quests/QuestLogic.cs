using System;
using System.Collections.Generic;
using System.Linq;
using Creatures;
using Items.Items;
using Items.Items.ArmorItems;
using Items.Items.BowItems;
using Items.Items.CombatItems;
using Logic.Events;
using Player.Inventory;
using TextDisplay;
using TMPro;
using UnityEngine;
using Utils;

namespace WatchAda.Quests {
    public class QuestLogic : MonoBehaviour {
        public static QuestLogic Instance { get; private set; }

        public TextMeshProUGUI mainTitle;
        public TextMeshProUGUI mainDescription;
        public TextMeshProUGUI mainProgress;
        public TextMeshProUGUI sideText;

        public List<QuestGroup> questGroups = new();
        public int activeGroup;

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

        // public Quest rennenQuest;
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

        // public Quest zombie4Quest;
        public Quest findeDomilitantQuest;
        public Quest tagebuchQuest;

        public Quest domilitantQuest;

        // public Quest rezeptFindenQuest;
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

        // public Quest kontrolleQuest;
        // public Quest befreienQuest;
        public Quest astrusQuest;

        public Quest verlassenQuest;

        public Quest reparierenQuest;
        public Quest nachHauseQuest;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void OnEnable() {
            EventManager.Instance.Subscribe<PlayerItemEvent>(OnItemPickup);
            EventManager.Instance.Subscribe<PlayerAreaEnterEvent>(OnAreaEntered);
            EventManager.Instance.Subscribe<CreatureInteractEvent>(OnCreatureInteraction);
            EventManager.Instance.Subscribe<PlayerItemUseEvent>(OnPlayerItemUse);
        }

        private void OnDisable() {
            EventManager.Instance.Unsubscribe<PlayerItemEvent>(OnItemPickup);
            EventManager.Instance.Unsubscribe<PlayerAreaEnterEvent>(OnAreaEntered);
            EventManager.Instance.Unsubscribe<CreatureInteractEvent>(OnCreatureInteraction);
            EventManager.Instance.Unsubscribe<PlayerItemUseEvent>(OnPlayerItemUse);
        }

        private void OnAreaEntered(PlayerAreaEnterEvent e) {
            foreach (var quest in questGroups[activeGroup].subQuests) {
                foreach (var conditionKey in quest.Conditions.Keys.ToList()) {
                    if (conditionKey is not EnteredCondition enteredCondition) continue;
                    if (quest.Conditions[conditionKey]) continue;

                    if (enteredCondition.IsMet(e.Area)) {
                        quest.CompleteCondition(enteredCondition);
                    }
                }
            }
        }

        private void OnItemPickup(PlayerItemEvent e) {
            foreach (var quest in questGroups[activeGroup].subQuests) {
                foreach (var conditionKey in quest.Conditions.Keys.ToList()) {
                    if (conditionKey is not ItemCondition itemCondition) continue;
                    if (quest.Conditions[conditionKey]) continue;

                    if (itemCondition.IsMet(e.Item)) {
                        quest.CompleteCondition(itemCondition);
                    }
                }
            }
        }

        private void OnCreatureInteraction(CreatureInteractEvent e) {
            foreach (var quest in questGroups[activeGroup].subQuests) {
                foreach (var conditionKey in quest.Conditions.Keys.ToList()) {
                    if (conditionKey is not InteractingCondition interactingCondition) continue;
                    if (quest.Conditions[conditionKey]) continue;

                    if (interactingCondition.IsMet(e)) {
                        quest.CompleteCondition(interactingCondition);
                    }
                }
            }
        }

        private void OnPlayerItemUse(PlayerItemUseEvent e) {
            foreach (var quest in questGroups[activeGroup].subQuests) {
                foreach (var conditionKey in quest.Conditions.Keys.ToList()) {
                    if (conditionKey is not ItemUseCondition itemUseCondition) continue;
                    if (quest.Conditions[conditionKey]) continue;

                    if (itemUseCondition.IsMet(e.Item)) {
                        quest.CompleteCondition(itemUseCondition);
                    }
                }
            }
        }

        private void Start() {
            // Gruppe 1: Festland FERTIG
            festlandQuest = new Quest("id_festland_main", "Festland", "Überquere das Meer.", true, 4);

            baueQuest = new Quest("id_festland_holz", "Holz & Äste",
                "Sammle Holz und Äste, indem du die Bäume zerstörst.",
                false, 2, 8);
            baueQuest.AddCondition(new ItemCondition(typeof(Stick)));
            baueQuest.AddCondition(new ItemCondition(typeof(Wood)));

            bootQuest = new Quest("id_festland_boot", "Boot", "Baue ein Boot in deinem Inventar.", false, 1, 11);
            bootQuest.AddCondition(new ItemCondition(typeof(Boat)));

            tierQuest = new Quest("id_festland_tier", "Quokkas", "Streichele ein Quokka durch anklicken.", false, 1);
            tierQuest.AddCondition(new InteractingCondition(CreatureType.Quokka, InteractionType.Pet));

            betreteFestlandQuest = new Quest("id_festland_ankunft", "Ankunft",
                "Betrete den Strand des Festlands, indem du Richtung Norden fährst.", false, 1, 12);
            betreteFestlandQuest.AddCondition(new EnteredCondition(AreaType.Beach));

            var group1 = new QuestGroup(festlandQuest,
                new List<Quest> { baueQuest, tierQuest, bootQuest, betreteFestlandQuest });


            // Gruppe 2: Crafting FERTIG
            craftingQuest = new Quest("id_crafting_main", "Werkzeuge", "Erstelle Werkzeuge und Waffen.", true, 4, 15);

            eQuest = new Quest("id_crafting_stein", "Stein", "Finde Stein", false, 1, 14);
            eQuest.AddCondition(new ItemCondition(typeof(Stone)));

            sQuest = new Quest("id_crafting_eisen", "Eisen", "Finde Eisen", false, 1);
            sQuest.AddCondition(new ItemCondition(typeof(Iron)));

            spitzAxtQuest = new Quest("id_crafting_spitzhacke", "Spitzhacke", "Baue eine Spitzhacke aus Stein.", false,
                1);
            spitzAxtQuest.AddCondition(new ItemCondition(typeof(StonePickaxe)));

            schwertQuest = new Quest("id_crafting_schwert", "Schwert", "Erstelle ein Schwert aus Eisen.", false, 1);
            schwertQuest.AddCondition(new ItemCondition(typeof(IronSword)));

            var group2 = new QuestGroup(
                craftingQuest,
                new List<Quest> { eQuest, sQuest, spitzAxtQuest, schwertQuest }
            );


            // Gruppe 3: Erkundung
            erkundenQuest = new Quest("id_erkundung_main", "Erkundung", "Erkunde den Planeten.", true, 2);

            zweiTiereQuest = new Quest("id_erkundung_tiere", "Tiere", "Streichele ein Dodo.", false, 1);
            zweiTiereQuest.AddCondition(new InteractingCondition(CreatureType.Dodo, InteractionType.Pet));

            plateauQuest = new Quest("id_erkundung_plateau", "Plateau", "Finde das Plateau.", false, 1, 16);
            plateauQuest.AddCondition(
                new EnteredCondition(AreaType.PlateauSight)); ///////////////////////////////////////////////////////

            var group3 = new QuestGroup(
                erkundenQuest,
                new List<Quest> { zweiTiereQuest, plateauQuest }
            );


            // Gruppe 4: Erste Zombieattacke FERTIG
            überlebenQuest = new Quest("id_zombies_main", "Zombies", "Überlebe die Zombieattacke.", true, 2);

            angreifen2Quest = new Quest("id_zombies_angriff", "Angreifen", "Greife einen Zombie an", false, 1, 21);
            angreifen2Quest.AddCondition(
                new InteractingCondition(new[] { CreatureType.Zombie, CreatureType.ZombieBoss },
                    InteractionType.Attack));

            schutzQuest = new Quest("id_zombies_schutz", "Schutz", "Bringe dich hinter dem Wasserfall in Sicherheit.",
                false, 1, 26);
            schutzQuest.AddCondition(new EnteredCondition(AreaType.JungleCave));

            var group4 = new QuestGroup(
                überlebenQuest,
                new List<Quest> { angreifen2Quest, schutzQuest }
            );


            // Gruppe 5: Höhle
            höhleQuest = new Quest("id_hoehle_main", "Höhle", "Erkunde die Höhle.", true, 4, 52);

            fakelQuest = new Quest("id_hoehle_fackel", "Fackel", "Baue eine Fackel.", false, 1, 28);
            fakelQuest.AddCondition(new ItemCondition(typeof(Torch)));

            stormQuest = new Quest("id_hoehle_storm", "Person?", "???", false, 1, 48); //?

            erzQuest = new Quest("id_hoehle_erz", "Erz", "Baue unbekanntes Erz ab.", false, 1, 49);
            erzQuest.AddCondition(new ItemCondition(typeof(Glomtom)));

            glomtomSchwertQuest =
                new Quest("id_hoehle_glomtom", "Glomtom", "Erstelle ein Glomtom-Schwert.", false, 1, 50);
            glomtomSchwertQuest.AddCondition(new ItemCondition(typeof(GlomtomSword)));

            var group5 = new QuestGroup(
                höhleQuest,
                new List<Quest> { fakelQuest, stormQuest, erzQuest, glomtomSchwertQuest }
            );


            // Gruppe 6: Zweite Zombieattacke
            zombie2Quest = new Quest("id_zombies_second_main", "Zombies II", "Entkomme der zweiten Attacke.", true, 2,
                53);

            zombie3Quest = new Quest("id_zombies_kampf", "Kampf", "Schlage mit dem Glomtomschwert.", false, 1); //?
            zombie3Quest.AddCondition(new ItemUseCondition(typeof(GlomtomSword)));

            flussQuest = new Quest("id_zombies_fluss", "Fluss", "Überquere die Seerosen.", false, 1);
            flussQuest.AddCondition(new EnteredCondition(AreaType.LilyPads)); ////////////////////

            var group6 = new QuestGroup(
                zombie2Quest,
                new List<Quest> { zombie3Quest, flussQuest }
            );


            // Gruppe 7: Plateau
            plateau2Quest = new Quest("id_plateau_main", "Plateau", "Erkunde das Plateau.", true, 4);

            lianeQuest = new Quest("id_plateau_liane", "Liane", "Sammle eine Liane.", false, 1);
            lianeQuest.AddCondition(new ItemCondition(typeof(Liana)));

            erklimmeQuest = new Quest("id_plateau_erklimme", "Erklimme", "Klettere das Plateau hinauf.", false, 1, 56);
            erklimmeQuest.AddCondition(new EnteredCondition(AreaType.Plateau));

            schatzkarteQuest =
                new Quest("id_plateau_karte", "Karte", "Entdecke die Geheimnisse der Farm.", false, 1, 58); //?
            schatzkarteQuest.AddCondition(new EnteredCondition(AreaType.Farm));

            RezepteQuest = new Quest("id_plateau_rezepte", "Rezepte", "Entdecke alte Rezepte.", false, 1, 60); //?

            var group7 = new QuestGroup(
                plateau2Quest,
                new List<Quest> { lianeQuest, erklimmeQuest, schatzkarteQuest, RezepteQuest }
            );


            // Gruppe 8: Extric
            ExtricQuest = new Quest("id_extric_main", "Extric", "Nutze Extric.", true, 4, 62);

            blumeQuest = new Quest("id_extric_blume", "Blume", "Finde die besondere Blume.", false, 1);
            blumeQuest.AddCondition(new ItemCondition(typeof(SpecialFlower)));

            craftenExtricQuest = new Quest("id_extric_bau", "Extric Bau", "Stelle Extric her.", false, 1);
            craftenExtricQuest.AddCondition(new ItemCondition(typeof(Extric)));

            rüstungQuest = new Quest("id_extric_ruestung", "Rüstung", "Erstelle eine vollständige Extric-Rüstung.",
                false, 1, 61);
            rüstungQuest.AddCondition(new ItemCondition(typeof(ExtricArmor)));

            var group8 = new QuestGroup(
                ExtricQuest,
                new List<Quest> { blumeQuest, ressourcenQuest, craftenExtricQuest, rüstungQuest }
            );


            // Gruppe 9: Zombieschatz
            schatzQuest = new Quest("id_schatz_main", "Schatz", "Finde den Zombieschatz.", true, 5, 71);

            bogenQuest = new Quest("id_schatz_bogen", "Bogen", "Baue einen Bogen aus Eisen.", false, 1);
            bogenQuest.AddCondition(new ItemCondition(typeof(IronBow)));

            sumpfQuest = new Quest("id_schatz_sumpf", "Sumpf", "Betrete den Sumpf.", false, 1, 63);
            sumpfQuest.AddCondition(new EnteredCondition(AreaType.Swamp));

            findeDomilitantQuest = new Quest("id_schatz_domilitant", "Domilitant", "Finde Domilitant.", false, 1, 66);
            findeDomilitantQuest.AddCondition(new ItemCondition(typeof(Domilitant)));

            tagebuchQuest = new Quest("id_schatz_tagebuch", "Tagebuch", "Finde den verborgenen Hinweis.", false, 1,
                69); //!

            var group9 = new QuestGroup(
                schatzQuest,
                new List<Quest> { bogenQuest, sumpfQuest, /*zombie4Quest,*/ findeDomilitantQuest, tagebuchQuest }
            );


            // Gruppe 10: Domilitant
            domilitantQuest = new Quest("id_domilitant_main", "Domilitant", "Setze Domilitant ein.", true, 2);

            //rezeptFindenQuest = new Quest("id_domilitant_rezept", "Rezept", "Finde ein Rezept.", false, 1); //!

            trankQuest = new Quest("id_domilitant_trank", "Trank",
                "Braue dir einen Unsichtbarkeitstrank, sodass dich die Zombies nicht entdecken.", false, 1, 74);
            trankQuest.AddCondition(new ItemCondition(typeof(InvisibilityPotion)));

            var group10 = new QuestGroup(
                domilitantQuest,
                new List<Quest> {
                    /*rezeptFindenQuest,*/ trankQuest
                }
            );


            // Gruppe 11: Stadt
            stadtQuest = new Quest("id_stadt_main", "Stadt", "Betrete die Stadt.", true, 3);

            crafteSchwertQuest = new Quest("id_stadt_schwert", "Schwert", "Erstelle ein neues Schwert aus Glomtom.",
                false,
                1, 77);
            crafteSchwertQuest.AddCondition(new ItemCondition(typeof(GlomtomSword)));

            unsichtbarQuest = new Quest("id_stadt_unsichtbar", "Unsichtbar", "Trinke den Unsichtbarkeitstrank.", false,
                1); //!

            wächterQuest = new Quest("id_stadt_waechter", "Wächter", "Betrete die Stadt.", false, 1, 80);
            wächterQuest.AddCondition(new EnteredCondition(AreaType.City));

            var group11 = new QuestGroup(
                stadtQuest,
                new List<Quest> { crafteSchwertQuest, unsichtbarQuest, wächterQuest }
            );


            // Gruppe 12: Labor
            LaborQuest = new Quest("id_labor_main", "Geschichte", "Was ist hier passiert?", true, 2);

            rettenQuest = new Quest("id_labor_rette", "Labor", "Finde das alte, zerstörte Labor.", false, 1, 82);
            rettenQuest.AddCondition(new EnteredCondition(AreaType.Labor));

            virusQuest = new Quest("id_labor_virus", "Virus?", "Klicke auf den eingeschalteten Computerbildschirm.",
                false, 1, 84); //?  

            laborVerlassenQuest = new Quest("id_labor_verlassen", "Was .. ist ... das", "Stelle dich dem ???.",
                false, 1,
                86); //?

            var group12 = new QuestGroup(
                LaborQuest,
                new List<Quest> { rettenQuest, virusQuest, laborVerlassenQuest }
            );


            // Gruppe 13: Endkampf
            endkampfQuest = new Quest("id_endkampf_main", "Endkampf", "Gewinne den Endkampf.", true, 2, 96);

            besiegenQuest = new Quest("id_endkampf_boss", "Boss", "Besiege den Endboss.", false, 1, 90); //1

            astrusQuest = new Quest("id_endkampf_astrus", "Astrus", "Finde Astrus.", false, 1);
            astrusQuest.AddCondition(new ItemCondition(typeof(Astrus)));

            var group13 = new QuestGroup(
                endkampfQuest,
                new List<Quest> { besiegenQuest, astrusQuest }
            );


            // Gruppe 14: Abreise
            verlassenQuest = new Quest("id_abreise_main", "Abreise", "Verlasse den Planeten.", true, 2);

            reparierenQuest =
                new Quest("id_abreise_raumschiff", "Raumschiff", "Repariere das Schiff.", false, 1, 97); //!

            nachHauseQuest = new Quest("id_abreise_heimkehr", "Heimkehr", "Fliege nach Hause.", false, 1); //!

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
            TextDisplayManager.Instance.textDisplay.notifications.Clear();
            TextDisplayManager.Instance.textDisplay.notificationButton.gameObject.SetActive(false);
            TextDisplayManager.Instance.textDisplay.panel.gameObject.SetActive(false);

            foreach (var quest in questGroups[activeGroup].subQuests) {
                foreach (var conditionKey in quest.Conditions.Keys.ToList()) {
                    if (conditionKey is not ItemCondition itemCondition) continue;
                    if (quest.Conditions[conditionKey]) continue;

                    foreach (var item in PlayerInventory.Instance.Slots.Select(slot => slot.Item)) {
                        if (itemCondition.IsMet(item)) {
                            quest.CompleteCondition(itemCondition);
                        }
                    }
                }
            }

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
                                 quest.currentProgress + "/" + quest.requiredProgress + ")" +
                                 "\n\n";
            }

            sideText.text = sideText.text[..^2];
        }
    }

    [Serializable]
    public class QuestGroup {
        public Quest mainQuest;
        public List<Quest> subQuests;

        public QuestGroup(Quest mainQuest, List<Quest> subQuests = null) {
            this.mainQuest = mainQuest;
            this.subQuests = subQuests ?? new List<Quest>();
        }
    }
}