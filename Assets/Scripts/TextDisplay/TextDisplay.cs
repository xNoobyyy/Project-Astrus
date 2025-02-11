using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.UI;
public class TextDisplay : MonoBehaviour
{   
    public GameObject textWindow;
    public Text displayText;
    public Button nextButton;
    private int recursionDepth = 0; // Hält die aktuelle Rekursionstiefe fest
    private const int maxRecursion = 3; // Maximale Anzahl an rekursiven Aufrufen
    private bool textComplete = false; // Überprüft, ob der Text vollständig angezeigt wurde
    private bool isDialogueActive = false; // Flag, um zu verhindern, dass K während eines laufenden Dialogs funktioniert
    // Methode zum Anzeigen des Fensters mit Text 
    public void ShowText(string text, int? imageIndex = null)
    {
        if (recursionDepth == 0) {
            if (isDialogueActive) return; // Blockiert mehrfachen Start durch K
            isDialogueActive = true; // Dialog wird gestartet
            textWindow.SetActive(true); // Fenster aktivieren
        }
        StartCoroutine(TypeText(text)); // Text mit Animation anzeigen
    }
    // Coroutine zur verzögerten Anzeige des Textes
    private IEnumerator TypeText(string text)
    {
        displayText.text = ""; // Textfeld leeren
        textComplete = false;
        foreach (char c in text)
        {
            displayText.text += c; // Buchstabe für Buchstabe hinzufügen
            yield return new WaitForSeconds(0.05f); // Kurze Verzögerung
        }
        textComplete = true;
        nextButton.gameObject.SetActive(true); // Next-Button anzeigen
    }
    // Methode wird beim Klicken des Next-Buttons aufgerufen
    public void OnNextButton()
    {
        if (!textComplete) return; // Falls der Text noch nicht fertig ist, nichts tun
        recursionDepth++; 
        nextButton.gameObject.SetActive(false); // Button ausblenden
        if (recursionDepth < maxRecursion)
        {
            ShowText("Test2"); // Nächste Rekursion mit neuem Text starten
        }
        else
        {
            CloseWindow(); // Fenster schließen nach max. Rekursion
        }
    }
    // Methode zum Schließen des Fensters und Zurücksetzen der Rekursionstiefe
    private void CloseWindow()
    {
        textWindow.SetActive(false);
        recursionDepth = 0; 
        isDialogueActive = false; // NEU: Damit K wieder funktioniert!
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            ShowText("jbslgnmTest Text erster Aufruf", 0);
        }

        if (Input.GetKeyDown(KeyCode.Return) && isDialogueActive) {
            OnNextButton();
        }
    }
}