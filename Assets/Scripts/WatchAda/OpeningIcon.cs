using Logic;
using UnityEngine;

public class OpeningIcon : MonoBehaviour {
    public RectTransform animatedImage;
    public LogicScript logic;
    public GameObject openedIcon;
    public GameObject inventoryScreen;
    public GameObject questScreen;
    public GameObject mapScreen;
    public MapManager mapManager;
    public GameObject logPanel;
    public bool moving;
    public bool resizing;
    private bool reversing;
    public bool animationActive;

    private Vector2 targetSize = new Vector2(206, 204);
    private Vector2 targetPosition = Vector2.zero;
    private Vector2 startSize = new Vector2(47, 47);

    private Vector2 startPosition;

    void Start() {
        animatedImage.sizeDelta = startSize;
        gameObject.SetActive(false);
    }

    void Update() {
        if (moving || resizing) {
            animatedImage.anchoredPosition = Vector2.Lerp(
                animatedImage.anchoredPosition,
                targetPosition,
                Time.unscaledDeltaTime * 10);

            animatedImage.sizeDelta = Vector2.Lerp(
                animatedImage.sizeDelta,
                targetSize,
                Time.unscaledDeltaTime * 10);
        }

        if (reversing) {
            animatedImage.anchoredPosition = Vector2.Lerp(
                animatedImage.anchoredPosition,
                startPosition,
                Time.unscaledDeltaTime * 10);

            animatedImage.sizeDelta = Vector2.Lerp(
                animatedImage.sizeDelta,
                startSize,
                Time.unscaledDeltaTime * 10);
        }

        float tolerance = 0.5f;

        if (moving || resizing) {
            if (Vector2.Distance(animatedImage.anchoredPosition, targetPosition) < tolerance &&
                Vector2.Distance(animatedImage.sizeDelta, targetSize) < tolerance) {
                moving = false;
                resizing = false;
                animationActive = false;
                logic.iconOpened = true;
                openedIcon.SetActive(true);
                if (startPosition == new Vector2(128, 128)) {
                    inventoryScreen.SetActive(true);
                    inventoryScreen.SetActive(false);
                    inventoryScreen.SetActive(true);
                }

                if (startPosition == new Vector2(-128, -128)) {
                    questScreen.SetActive(true);
                }

                if (startPosition == new Vector2(-128, 128)) {
                    mapScreen.SetActive(true);
                }

                if (startPosition == new Vector2(128, -128)) {
                    logPanel.SetActive(true);
                }
            }
        }

        if (reversing) {
            if (Vector2.Distance(animatedImage.anchoredPosition, startPosition) < tolerance &&
                Vector2.Distance(animatedImage.sizeDelta, startSize) < tolerance) {
                reversing = false;
                animationActive = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void StartAnimation(Vector2 startPos) {
        if (!animationActive) {
            gameObject.SetActive(true);
            animatedImage.anchoredPosition = startPos;
            animatedImage.sizeDelta = startSize;
            startPosition = startPos;
            moving = true;
            resizing = true;
            animationActive = true;
        }
    }

    public void ReverseAnimation() {
        if (!animationActive) {
            inventoryScreen.SetActive(false);
            questScreen.SetActive(false);
            mapScreen.SetActive(false);
            logPanel.SetActive(false);
            openedIcon.SetActive(false);
            reversing = true;
            animationActive = true;
            logic.iconOpened = false;
        }
    }
}