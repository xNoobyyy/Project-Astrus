using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularPhotoPanel : MonoBehaviour {
    public bool plateuGefunden;
    [Header("Fotos konfigurieren")]
    [Tooltip("Liste der anzuzeigenden Fotos (als Sprite)")]
    public List<Sprite> photos;
    public List<Sprite> photoslate;

    [Header("Referenzen")]
    [Tooltip("Prefab für ein Foto-Element (muss eine Image-Komponente besitzen)")]
    public GameObject photoItemPrefab;
    public GameObject photoItemPrefabsmall;

    [Tooltip("Content-Panel (das Kindobjekt des ScrollRect mit Vertical Layout)")]
    public RectTransform contentPanel;

    void Awake()
    {
        if (contentPanel == null)
        {
            Debug.LogError("Content Panel nicht zugewiesen!");
            return;
        }

        if (plateuGefunden) {
            photos.AddRange(photoslate);
        }
        
        // Bestehende Kinder ggf. löschen
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        
        // Für jedes Foto ein Element instanziieren und die Höhe prüfen
        foreach (Sprite photo in photos)
        {
            // Wähle das Prefab basierend auf der Höhe des Sprites (32 oder anders)
            GameObject prefabToUse = (photo.rect.height == 32f) ? photoItemPrefab : photoItemPrefabsmall;
            GameObject item = Instantiate(prefabToUse, contentPanel);
            
            Image img = item.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = photo;
            }
            else
            {
                Debug.LogWarning("Das Prefab besitzt keine Image-Komponente!");
            }
        }
        
        // Optional: Layout neu berechnen (falls sich die Größe ändert)
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel);
    }
}