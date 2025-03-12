using Logic;
using UnityEngine;
using Image = UnityEngine.UIElements.Image;

public class Watch : MonoBehaviour {
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
    private bool opening;
    private bool closing;

    private void Start() {
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
    private void Update() {
        if (opening) {
            watchbackground.transform.localScale = Vector2.Lerp(
                watchbackground.transform.localScale,
                new Vector2((4), (4)),
                Time.unscaledDeltaTime * 10);
        }

        if (closing) {
            watchbackground.transform.localScale = Vector2.Lerp(
                watchbackground.transform.localScale,
                new Vector2((0), (0)),
                Time.unscaledDeltaTime * 10);
        }

        const float tolerance = 0.01f; // Toleranzwert

        if (opening) {
            if (Vector2.Distance(watchbackground.transform.localScale, new Vector2(4, 4)) < tolerance) {
                watchOn.SetActive(true);
                mapIcon.SetActive(true);
                inventoryIcon.SetActive(true);
                questsIcon.SetActive(true);
                logIcon.SetActive(true);
                opening = false;
                logic.watchOpen = true;
            }
        }

        if (!closing) return;
        if (!(Vector2.Distance(watchbackground.transform.localScale, new Vector2(0, 0)) < tolerance)) return;

        gameObject.SetActive(false);
        closing = false;
        logic.watchOpen = false;
    }


    public void Open() {
        if (closing) return;

        gameObject.SetActive(true);
        opening = true;
    }

    public void Close() {
        if (opening) return;

        watchOn.SetActive(false);
        mapIcon.SetActive(false);
        inventoryIcon.SetActive(false);
        questsIcon.SetActive(false);
        logIcon.SetActive(false);
        closing = true;
    }

    public void ReverseAnimation() {
        openingIconSc.ReverseAnimation();
    }
}