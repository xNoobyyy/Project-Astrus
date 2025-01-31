using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UIElements.Image;
using System.Collections;

public class Watch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject watchbackground;
    public GameObject watchOn;
    public GameObject mapIcon;
    public GameObject inventoryIcon;
    public GameObject questsIcon;
    public GameObject logIcon;
    public GameObject openedIcon;
    public OpeningIcon openingIconSc;
    public GameObject openingIcon;
    
    public LogicScript logic;
    public Image WatchOff;
    private bool opening = false;
    private bool closing = false;
    private bool opened = false;
    
    
    
    void Start()
    {
        mapIcon.SetActive(false);
        inventoryIcon.SetActive(false);
        questsIcon.SetActive(false);
        logIcon.SetActive(false);
        watchbackground.transform.localScale = new Vector3(0, 0, 1);
        gameObject.SetActive(false);
        watchOn.SetActive(false);
        openedIcon.SetActive(false);
        openingIcon.SetActive(false);
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
                watchOn.SetActive(true);
                mapIcon.SetActive(true);
                inventoryIcon.SetActive(true);
                questsIcon.SetActive(true);
                logIcon.SetActive(true);
                opened = true;
                opening = false;
                logic.WatchOpen = true;

            }
        }
        if (closing) {
            
            if (Vector2.Distance(watchbackground.transform.localScale, new Vector2(0, 0)) < tolerance) {
                
                gameObject.SetActive(false);
                closing = false;
                opened = false;
                logic.WatchOpen = false;
            }
        }

    }


    public void open() {
        
        if (!closing) {
            gameObject.SetActive(true);
            opening = true;
        }
    }

    public void close() {
        if (!opening) {
            watchOn.SetActive(false);
            mapIcon.SetActive(false);
            inventoryIcon.SetActive(false);
            questsIcon.SetActive(false);
            logIcon.SetActive(false);
            closing = true;
        }
    }

    public bool closed() {
        if (!opened) {
            return true;
        } else {
            return false;
        }
    }

    public void ReverseAnimation() {
        openingIconSc.ReverseAnimation();
    }
    
    IEnumerator DelayInMilliseconds(int milliseconds)
    {
        
        yield return new WaitForSeconds(milliseconds / 1000f);

        
    }

    IEnumerator OnOff() {
        watchOn.SetActive(true);
        mapIcon.SetActive(true);
        inventoryIcon.SetActive(true);
        questsIcon.SetActive(true);
        logIcon.SetActive(true);

        yield return new WaitForSeconds(0.05f);
        watchOn.SetActive(false);
        mapIcon.SetActive(false);
        inventoryIcon.SetActive(false);
        questsIcon.SetActive(false);
        logIcon.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        watchOn.SetActive(true);
        mapIcon.SetActive(true);
        inventoryIcon.SetActive(true);
        questsIcon.SetActive(true);
        logIcon.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        watchOn.SetActive(false);
        mapIcon.SetActive(false);
        inventoryIcon.SetActive(false);
        questsIcon.SetActive(false);
        logIcon.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        watchOn.SetActive(true);
        mapIcon.SetActive(true);
        inventoryIcon.SetActive(true);
        questsIcon.SetActive(true);
        logIcon.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        watchOn.SetActive(false);
        mapIcon.SetActive(false);
        inventoryIcon.SetActive(false);
        questsIcon.SetActive(false);
        logIcon.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        watchOn.SetActive(true);
        mapIcon.SetActive(true);
        inventoryIcon.SetActive(true);
        questsIcon.SetActive(true);
        logIcon.SetActive(true);
        opened = true;
        opening = false;
        logic.WatchOpen = true;
    }
}


