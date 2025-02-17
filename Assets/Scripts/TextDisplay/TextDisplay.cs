using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


namespace TextDisplay
{
    public class TextDisplay : MonoBehaviour
    {   
        public GameObject textWindow;
        public Text displayText;
        public Button nextButton;
        public Image leftImage;
        public Sprite image0;
        public Sprite image1;
        public Sprite image2;
        public Image sprechblase;
        public Sprite sprechblase0;
        public Sprite sprechblase1;
        public GameObject optionsPanel;
        public Text option1Text;
        public Text option2Text;
        public Button option1Button;
        public Button option2Button;
        public RectTransform textPosition;
        private bool textComplete = false; // Überprüft, ob der Text vollständig angezeigt wurde
        private bool isDialogueActive = false; // Flag, um zu verhindern, dass K während eines laufenden Dialogs funktioniert
        public int storyID = 0;
        public StoryManager storyManager;
        public bool weiter;
        public bool start;
        public Dictionary<string, StoryBlock> storyBlocks;
        
        // Methode zum Anzeigen des Fensters mit Text 

        public void Start() {
            storyManager.LoadStory();
            storyBlocks = storyManager.storyBlocks;
            //SpecificStoryDialogue(0);
        }

        public void ShowText(string text, int? imageIndex = null)
        {
            if (start) {
                isDialogueActive = true; // Dialog wird gestartet
                textWindow.SetActive(true); // Fenster aktivieren
            }

            if (imageIndex.HasValue) { 
                if (imageIndex.Value == 0) {
                    sprechblase.sprite = sprechblase0;
                    sprechblase.rectTransform.anchoredPosition = new UnityEngine.Vector2(32, 0);
                    textPosition.anchoredPosition = new UnityEngine.Vector2(-10, 0); 
                    if (int.Parse(storyBlocks[(storyID+1).ToString()].person) == 1) {
                        leftImage.sprite = image1;
                    }else if (int.Parse(storyBlocks[(storyID + 1).ToString()].person) == 2) {
                        leftImage.sprite = image2;
                    } else {
                        Debug.Log("bildanzeigefehler");
                    }
                }else if (imageIndex.Value == 1) {
                    leftImage.sprite = image1;
                    sprechblase.sprite = sprechblase1;
                    sprechblase.rectTransform.anchoredPosition = new UnityEngine.Vector2(-16, 0);
                    textPosition.anchoredPosition = new UnityEngine.Vector2(30, 0); 
                }else if (imageIndex.Value == 2) {
                    leftImage.sprite = image2;
                    sprechblase.sprite = sprechblase1;
                    sprechblase.rectTransform.anchoredPosition = new UnityEngine.Vector2(-16, 0);
                    textPosition.anchoredPosition = new UnityEngine.Vector2(30, 0); 
                } else {
                    Debug.Log("bildanzeigefehler2");
                }
            }
            List<string> choices = ExtractChoices(text);
            if (choices.Count == 0) {
                StartCoroutine(TypeText(text)); // Text mit Animation anzeigen
            } else {
                ShowChoices(choices);
            }
        }
        // Coroutine zur verzögerten Anzeige des Textes
        private IEnumerator TypeText(string text)
        {
            displayText.text = ""; // Textfeld leeren
            textComplete = false;
            foreach (char c in text)
            {
                displayText.text += c; // Buchstabe für Buchstabe hinzufügen
                yield return new WaitForSeconds(0.01f); // Kurze Verzögerung
            }
            textComplete = true;
            storyID++;
            nextButton.gameObject.SetActive(true); // Next-Button anzeigen
        }
        // Methode wird beim Klicken des Next-Buttons aufgerufen
        public void OnNextButton(int c = 0)
        {
            optionsPanel.SetActive(false);
            option1Button.gameObject.SetActive(false);
            option2Button.gameObject.SetActive(false);
            option1Text.text = "";
            option2Text.text = "";
            
            if (!textComplete && c !=1) return; // Falls der Text noch nicht fertig ist, nichts tun
            nextButton.gameObject.SetActive(false); // Button ausblenden
            if (!bool.Parse(storyBlocks[(storyID - 1).ToString()].weiter)) {
                CloseWindow();
                return;
            }
            if (weiter)
            {
                if (c == 1) {
                    storyID++;
                }
                ShowText(storyManager.ShowStoryBlock(storyID.ToString()),int.Parse(storyBlocks[storyID.ToString()].person));
            }
            else
            {
                ShowText(storyManager.ShowStoryBlock(storyID.ToString()),int.Parse(storyBlocks[storyID.ToString()].person));
            }
        }
        
        private void CloseWindow()
        {
            textWindow.SetActive(false);
            isDialogueActive = false; 
        }
        
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Return) && isDialogueActive) {
                OnNextButton();
            }
        }

        public void SpecificStoryDialogue(int i) {
            storyID = i;
            NextStoryDialogue();
        }
        public void NextStoryDialogue() {
            start = bool.Parse(storyBlocks[storyID.ToString()].start);
            weiter = bool.Parse(storyBlocks[storyID.ToString()].weiter);
            
            if (start && !weiter) {
                Debug.Log("startundnichtweiter");
                ShowText(storyManager.ShowStoryBlock(storyID.ToString()),int.Parse(storyBlocks[storyID.ToString()].person));
                
            } else if (weiter) {
                ShowText(storyManager.ShowStoryBlock(storyID.ToString()),int.Parse(storyBlocks[storyID.ToString()].person));
                
            }else {
                Debug.Log("Fortsetzung folgt!");
            }
        }
        List<string> ExtractChoices(string text)
        {
            List<string> choices = new List<string>();
            MatchCollection matches = Regex.Matches(text, @"\[(.*?)\]");

            foreach (Match match in matches)
            {
                choices.Add(match.Groups[1].Value);
            }
            return choices;
        }
        void ShowChoices(List<string> choices)
        {
            displayText.text = ""; 
            optionsPanel.SetActive(true);
            option1Button.gameObject.SetActive(true);
            option2Button.gameObject.SetActive(true);
            option1Text.text = choices[0];
            option2Text.text = choices[1];
            //option1Button.onClick.RemoveAllListeners();  
           // option1Button.onClick.AddListener(() => OnNextButton(1));
            //option2Button.onClick.RemoveAllListeners(); 
            //option2Button.onClick.AddListener(() => OnNextButton(1));
        }
    }
}