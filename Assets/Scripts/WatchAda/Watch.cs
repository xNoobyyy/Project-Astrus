using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UIElements.Image;

public class Watch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject watchbackground;
    public GameObject watchOn;
    public GameObject mapIcon;
    public GameObject inventoryIcon;
    public GameObject questsIcon;
    public GameObject logIcon;
    public Image WatchOff;
    private bool opening = false;
    private bool closing = false;
    
    
    
    void Start()
    {
        mapIcon.SetActive(false);
        inventoryIcon.SetActive(false);
        questsIcon.SetActive(false);
        logIcon.SetActive(false);
        watchbackground.transform.localScale = new Vector3(0, 0, 1);
        gameObject.SetActive(false);
        watchOn.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (opening) {
            watchbackground.transform.localScale = Vector2.Lerp(
                watchbackground.transform.localScale,
                new Vector2((4), (4)),
                Time.deltaTime * 10);
        }

        if (closing) {
            
            watchbackground.transform.localScale = Vector2.Lerp(
                watchbackground.transform.localScale,
                new Vector2((0), (0)),
                Time.deltaTime * 10);
        }
        
        float tolerance = 0.01f; // Toleranzwert

        if (opening) {
            if (Vector2.Distance(watchbackground.transform.localScale, new Vector2(4, 4)) < tolerance) {
                opening = false;
                watchOn.SetActive(true);
                mapIcon.SetActive(true);
                inventoryIcon.SetActive(true);
                questsIcon.SetActive(true);
                logIcon.SetActive(true);
            }
        }
        if (closing) {
            
            if (Vector2.Distance(watchbackground.transform.localScale, new Vector2(0, 0)) < tolerance) {
                
                gameObject.SetActive(false);
                closing = false;
            }
        }

    }


    public void open() {
        gameObject.SetActive(true);
        opening = true;
    }

    public void close() {
        watchOn.SetActive(false);
        mapIcon.SetActive(false);
        inventoryIcon.SetActive(false);
        questsIcon.SetActive(false);
        logIcon.SetActive(false);
        closing = true;
    }
}


