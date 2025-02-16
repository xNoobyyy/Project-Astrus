using UnityEngine;

[System.Serializable]
public class Quest: MonoBehaviour  {
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
    /// Aktualisiert den Fortschritt der Quest.
    /// </summary>
    /// <param name="amount">Die Menge, um die der Fortschritt erhöht werden soll.</param>
    public void UpdateProgress(int amount) {
        if (isCompleted) return;
        
        currentProgress += amount;
        
        if (currentProgress >= requiredProgress) {
            currentProgress = requiredProgress;
            isCompleted = true;
        }
    }
    
    /// <summary>
    /// Gibt den Fortschritt als Prozentsatz (0 bis 1) zurück.
    /// </summary>
    public float GetProgressPercentage() {
        if (requiredProgress == 0)
            return 1f;
        return (float)currentProgress / requiredProgress;
    }
}
