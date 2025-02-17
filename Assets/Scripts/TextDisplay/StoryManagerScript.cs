using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
public class StoryManager : MonoBehaviour
{
    private StoryData storyData;
    public Dictionary<string, StoryBlock> storyBlocks;
    public static string playerName = "Spieler";
    
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
        block.text = ReplaceVariables(block.text);
        
        Debug.Log(block.text);
        return block.text;
        
    }
    string ReplaceVariables(string text)
    {
        return text.Replace("{playerName}", playerName);
    }
    
}


