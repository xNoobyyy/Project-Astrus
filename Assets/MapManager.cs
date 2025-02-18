using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    // Aufteilung der Karte in ein Grid
    public int gridWidth = 200;
    public int gridHeight = 200;
    public bool[,] visited;

    public MapLogic mapManager;

    // Optional: Sprite für das Cover (Nebel), im Inspector zuweisen
    public Sprite coverSprite;

    // Container, in dem die Map angezeigt wird (z. B. ein Panel mit RectTransform)
    public RectTransform mapContainer;

    // Spielerreferenz – das Objekt, dessen Position für das Freischalten genutzt wird
    public Transform player;

    // Unlocked wird ein Bereich um den Spieler herum freigeschaltet (Anzahl der Zellen als Radius)
    public int unlockRadius = 100;

    // Cover-Größe in Pixeln (wird dynamisch aus der Containergröße berechnet)
    private float coverWidth;
    private float coverHeight;

    void Start()
    {
        // Berechne die Größe der einzelnen Zellen basierend auf der Container-Größe
        coverWidth = mapContainer.rect.width / gridWidth;
        coverHeight = mapContainer.rect.height / gridHeight;

        // Initialisiere das visited-Array
        //visited = new bool[gridWidth, gridHeight];
        //for (int i = 0; i < gridWidth; i++)
        //{
         //   for (int j = 0; j < gridHeight; j++)
         //   {
         //       visited[i, j] = false;
         //   }
        //}
        UpdateMapDisplay();
    }

    void OnEnable() {
        UpdateMapDisplay();
    }
    public void Updated()
    {
        // Wenn ein Spieler zugewiesen ist, berechne seine Position im Grid
        if (player != null)
        {
            // Hier nehmen wir an, dass die Weltkoordinaten des Spielers direkt auf das Grid abgebildet werden.
            // Eventuell musst du einen Offset bzw. eine Skalierung berücksichtigen, falls dein Level nicht bei (0,0) beginnt.
            int gridX = Mathf.FloorToInt((40 * (player.position.x + 290)) / 680 );
            int gridY = Mathf.FloorToInt((40 * (player.position.y + 35)) / 680 );
            
            Debug.Log(gridX + "," + gridY);

            // Markiere die Zelle, in der sich der Spieler befindet, sowie angrenzende Zellen im unlockRadius
            for (int i = gridX - unlockRadius; i <= gridX + unlockRadius; i++)
            {
                for (int j = gridY - unlockRadius; j <= gridY + unlockRadius; j++)
                {
                    if (i >= 0 && i < gridWidth && j >= 0 && j < gridHeight)
                    {
                        if (!visited[i, j])
                        {
                            VisitArea(i, j);
                        }
                    }
                }
            }
        }
    }

    // Markiert eine bestimmte Zelle als besucht und aktualisiert die Darstellung
    public void VisitArea(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            visited[x, y] = true;
            UpdateMapDisplay();
        }
    }
    

    // Aktualisiert die Map-Darstellung: Für alle unbesuchten Bereiche wird ein Cover (Nebel) angezeigt.
    public void UpdateMapDisplay()
    {
        // Entferne alle bisherigen Cover aus dem mapContainer, um Duplikate zu vermeiden
        foreach (Transform child in mapContainer)
        {
            if (child.name.StartsWith("Cover_"))
            {
                Destroy(child.gameObject);
            }
        }
        
        
        float gridTotalWidth = gridWidth * coverWidth;
        float gridTotalHeight = gridHeight * coverHeight;
        Vector2 gridCenter = new Vector2(gridTotalWidth / 2f, gridTotalHeight / 2f);
        // Der Kreis hat einen Durchmesser gleich der Gesamtbreite des Grids
        float radius = gridTotalWidth / 2f;

        // Erzeuge Cover (Nebel) für alle Zellen, die noch nicht besucht wurden
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (!mapManager.visited[i, j])
                {
                    Debug.Log("false");
                    Vector2 squareCenter = new Vector2(i * coverWidth + coverWidth / 2f, j * coverHeight + coverHeight / 2f);
                    if (Vector2.Distance(squareCenter, gridCenter) <= radius) {

                        // Erzeuge ein neues UI-Element mit RectTransform, CanvasRenderer und Image
                        GameObject newCover = new GameObject("Cover_" + i + "_" + j,
                            typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));

                        // Setze das Cover als Kind des mapContainer
                        newCover.transform.SetParent(mapContainer, false);
                        newCover.transform.localScale = Vector3.one;

                        // Hole den RectTransform und passe Position und Größe an
                        RectTransform rt = newCover.GetComponent<RectTransform>();
                        rt.anchoredPosition = new Vector2(i * 0.9f * coverWidth -20.75f + 2.5f,
                            j * 0.9f * coverHeight - 20.275f + 0.725f * 2 + 0.15f);
                        rt.sizeDelta = new Vector2(coverWidth * 0.9f, coverHeight * 0.9f);

                        // Konfiguriere den Image-Komponenten
                        Image img = newCover.GetComponent<Image>();
                        img.preserveAspect = false;
                        if (coverSprite != null) {
                            img.sprite = coverSprite;
                        }
                    }
                }//else { Debug.Log("true"); }
            }
        }
    }
}
