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
        public static TextDisplay Instance { get; private set; }
        
        public GameObject textWindow;
        public Text displayText;
        public Button nextButton;
        public Image leftImage;
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
        private bool textComplete = false; 
        public bool isDialogueActive = false; 
        public int storyID = 0;
        public StoryManager storyManager;
        public bool weiter;
        public bool start;
        public Dictionary<string, StoryBlock> storyBlocks;
        public Coroutine coroutine;
        public String x;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }
        // Methode zum Anzeigen des Fensters mit Text 
        public void Start() {
            Time.timeScale = 1f;
            storyManager.LoadStory();
            storyBlocks = storyManager.storyBlocks;
            SpecificStoryDialogue(0);
        }
        public void ShowText(string text, int? imageIndex = null)
        {
            x = text;
            if (start) {
                isDialogueActive = true; // Dialog wird gestartet
                textWindow.SetActive(true); // Fenster aktivieren
            }

            if (imageIndex.HasValue) { 
                if (imageIndex.Value == 0) {
                    sprechblase.sprite = sprechblase0;
                    sprechblase.rectTransform.anchoredPosition = new UnityEngine.Vector2(36, 0);
                    textPosition.anchoredPosition = new UnityEngine.Vector2(-10, 0); 
                    if (int.Parse(storyBlocks[(storyID+1).ToString()].person) == 1) {
                        leftImage.sprite = image1;
                        RectTransform rectTransform = leftImage.GetComponent<RectTransform>();
                        rectTransform.sizeDelta = new Vector2(228, 228);
                    }else if (int.Parse(storyBlocks[(storyID + 1).ToString()].person) == 2) {
                        leftImage.sprite = image2;
                        RectTransform rectTransform = leftImage.GetComponent<RectTransform>();
                        rectTransform.sizeDelta = new Vector2(228, 455);
                    } else {
                        Debug.Log("bildanzeigefehler");
                    }
                }else if (imageIndex.Value == 1) {
                    leftImage.sprite = image1;
                    sprechblase.sprite = sprechblase1;
                    sprechblase.rectTransform.anchoredPosition = new UnityEngine.Vector2(-18, 0);
                    textPosition.anchoredPosition = new UnityEngine.Vector2(30, 0); 
                    RectTransform rectTransform = leftImage.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(228, 228);
                }else if (imageIndex.Value == 2) {
                    leftImage.sprite = image2;
                    sprechblase.sprite = sprechblase1;
                    sprechblase.rectTransform.anchoredPosition = new UnityEngine.Vector2(-18, 0);
                    textPosition.anchoredPosition = new UnityEngine.Vector2(30, 0); 
                    RectTransform rectTransform = leftImage.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(228, 455);
                } else {
                    Debug.Log("bildanzeigefehler2");
                }
            }
            List<string> choices = ExtractChoices(text);
            
            if (choices.Count == 0) {
                if (coroutine != null) {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
                textComplete = false;
                coroutine = StartCoroutine(TypeText(text)); // Text mit Animation anzeigen
            } else {
                ShowChoices(choices);
            }
        }
        // Coroutine zur verzögerten Anzeige des Textes
        private IEnumerator TypeText(string text)
        {
            textComplete = false;
            displayText.text = ""; // Textfeld leeren
            foreach (char c in text)
            {
                displayText.text += c; // Buchstabe für Buchstabe hinzufüg
                yield return new WaitForSeconds(0.01f); // Kurze Verzögerung
            }
            textComplete = true;
            storyID++;
            nextButton.gameObject.SetActive(true); // Next-Button anzeigen
        }
        public void OnNextButton(int c = 0)
        {
            optionsPanel.SetActive(false);
            option1Button.gameObject.SetActive(false);
            option2Button.gameObject.SetActive(false);
            option1Text.text = "";
            option2Text.text = "";
            nextButton.gameObject.SetActive(true);
            if (!textComplete) return; // Falls der Text noch nicht fertig ist, nichts tun
            nextButton.gameObject.SetActive(false); // Button ausblenden
            if (!bool.Parse(storyBlocks[(storyID - 1).ToString()].weiter)) {
                CloseWindow();
                return;
            }
            if (weiter)
            {
                ShowText(storyManager.ShowStoryBlock(storyID.ToString()),int.Parse(storyBlocks[storyID.ToString()].person));
            }
            else{
                ShowText(storyManager.ShowStoryBlock(storyID.ToString()),int.Parse(storyBlocks[storyID.ToString()].person));
            }
        }
        private void CloseWindow()
        {
            textWindow.SetActive(false);
            isDialogueActive = false; 
        }
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Return) && isDialogueActive && x == displayText.text) {
                OnNextButton(0);
            }else if(Input.GetKeyDown(KeyCode.Space) && isDialogueActive && x != displayText.text) {
                if(coroutine != null) {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
                displayText.text = "";
                displayText.text = x;
                textComplete = true;
                storyID++;
                nextButton.gameObject.SetActive(true);
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
                ShowText(storyManager.ShowStoryBlock(storyID.ToString()),int.Parse(storyBlocks[storyID.ToString()].person));
                
            } else if (weiter) {
                ShowText(storyManager.ShowStoryBlock(storyID.ToString()),int.Parse(storyBlocks[storyID.ToString()].person));
                
            }else {
                Debug.Log("Ende");
            }
        }
        List<string> ExtractChoices(string text)
        {
            List<string> choices = new List<string>();
            MatchCollection matches = Regex.Matches(text, @"\[(.*?)\]");

            foreach (Match match in matches){
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
            textComplete = true;
            storyID++;
        }
    }
}