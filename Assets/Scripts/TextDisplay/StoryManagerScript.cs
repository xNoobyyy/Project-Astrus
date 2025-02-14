using System.Collections.Generic;
using UnityEngine;
public class StoryManager : MonoBehaviour
{
    private StoryData storyData;
    public Dictionary<string, StoryBlock> storyBlocks;
    
    void Start() {
        LoadStory();
    }
    public void LoadStory(){
            TextAsset jsonFile = Resources.Load<TextAsset>("story");
            if (jsonFile == null)
            {
                Debug.LogError("FEHLER: Die JSON-Datei konnte nicht geladen werden!");
                return;
            }
    
            // JSON in Story-Struktur umwandeln
            storyData = JsonUtility.FromJson<StoryData>(jsonFile.text);
    
            if (storyData == null || storyData.story_blocks == null)
            {
                Debug.LogError("FEHLER");
                return;
            }
    
            // Dictionary für schnelleren Zugriff erstellen
            storyBlocks = new Dictionary<string, StoryBlock>();
            foreach (var block in storyData.story_blocks)
            {
                storyBlocks[block.id] = block;
            }
    }
    // Ausgabe der Textbausteine
    public string ShowStoryBlock(string blockId) {
        if (!storyBlocks.ContainsKey(blockId)) {
            Debug.LogError("Storyblock nicht gefunden " + blockId);
            return "";
        }
        StoryBlock block = storyBlocks[blockId];
        Debug.Log(block.text);
        return block.text;
        
    }
}

/* Ausstehend:
 * STory-Blöcke bekommen eine ID von 0 bis irgendwo im Bereich 100
 * Aktuelle ID wird als Stand irgendwo gespeichert
 * Storyblöcke bekommen boolean Wert ob Ende oder nicht
 * Nächster Storyblock wird auf Enter angezeigt, indem eine Methode aufgerufen wird, bei der StoryID übergeben wird
 */
