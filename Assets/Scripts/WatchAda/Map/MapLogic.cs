using UnityEngine;

public class MapLogic : MonoBehaviour {
    public GameObject mapApp;
    public Transform player;
    
    public bool[,] visited;
    public int unlockRadius = 1;
    public int gridWidth = 100;
    public int gridHeight = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        visited = new bool[gridWidth, gridHeight];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                visited[i, j] = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Hier nehmen wir an, dass die Weltkoordinaten des Spielers direkt auf das Grid abgebildet werden.
            // Eventuell musst du einen Offset bzw. eine Skalierung berücksichtigen, falls dein Level nicht bei (0,0) beginnt.
            int gridX = Mathf.FloorToInt((gridWidth * (player.position.x + 290)) / 680 );
            int gridY = Mathf.FloorToInt((gridHeight * (player.position.y + 35 + 166)) / 680 );
            
            // Markiere die Zelle, in der sich der Spieler befindet, sowie angrenzende Zellen im unlockRadius
            for (int i = gridX - unlockRadius; i <= gridX + unlockRadius; i++)
            {
                for (int j = gridY - unlockRadius; j <= gridY + unlockRadius; j++)
                {
                    if (i >= 0 && i < gridWidth && j >= 0 && j < gridHeight)
                    {
                        if (!visited[i, j]) {
                            visited[i, j] = true;
                        }
                    }
                }
            }
        }
    }
}
