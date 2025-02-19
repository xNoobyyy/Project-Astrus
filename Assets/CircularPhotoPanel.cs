using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularPhotoPanel : MonoBehaviour
{
    [Header("Fotos konfigurieren")]
    [Tooltip("Liste der anzuzeigenden Fotos (als Sprite)")]
    public List<Sprite> photos;

    [Header("Referenzen")]
    [Tooltip("Prefab für ein Foto-Element (muss ein Image-Komponente besitzen)")]
    public GameObject photoItemPrefab;
    [Tooltip("Content-Panel (das Kindobjekt des ScrollRect mit Vertical Layout)")]
    public RectTransform contentPanel;

    void Start()
    {
        if (contentPanel == null)
        {
            Debug.LogError("Content Panel nicht zugewiesen!");
            return;
        }
        
        // Bestehende Kinder ggf. löschen
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        
        // Für jedes Foto ein Element instanziieren
        foreach (Sprite photo in photos)
        {
            GameObject item = Instantiate(photoItemPrefab, contentPanel);
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