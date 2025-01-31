using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpeningIcon : MonoBehaviour
{
    public RectTransform animatedImage;
    public LogicScript logic;
    public GameObject openedIcon;
    private bool moving = false;
    private bool resizing = false;
    private bool reversing = false;
    private bool animationActive = false;
    
    private Vector2 targetSize = new Vector2(206, 204);
    private Vector2 targetPosition = Vector2.zero;
    private Vector2 startSize = new Vector2(47, 47);

    private Vector2 startPosition;

    void Start()
    {
        animatedImage.sizeDelta = startSize;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (moving || resizing)
        {
            animatedImage.anchoredPosition = Vector2.Lerp(
                animatedImage.anchoredPosition,
                targetPosition,
                Time.deltaTime * 10);

            animatedImage.sizeDelta = Vector2.Lerp(
                animatedImage.sizeDelta,
                targetSize,
                Time.deltaTime * 10);
        }

        if (reversing)
        {
            animatedImage.anchoredPosition = Vector2.Lerp(
                animatedImage.anchoredPosition,
                startPosition,
                Time.deltaTime * 10);

            animatedImage.sizeDelta = Vector2.Lerp(
                animatedImage.sizeDelta,
                startSize,
                Time.deltaTime * 10);
        }

        float tolerance = 0.5f;

        if (moving || resizing)
        {
            if (Vector2.Distance(animatedImage.anchoredPosition, targetPosition) < tolerance &&
                Vector2.Distance(animatedImage.sizeDelta, targetSize) < tolerance)
            {
                moving = false;
                resizing = false;
                animationActive = false;
                logic.IconOpened = true;
                openedIcon.SetActive(true);
            }
        }

        if (reversing)
        {
            if (Vector2.Distance(animatedImage.anchoredPosition, startPosition) < tolerance &&
                Vector2.Distance(animatedImage.sizeDelta, startSize) < tolerance)
            {
                reversing = false;
                animationActive = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void StartAnimation(Vector2 startPos)
    {
        if (!animationActive)
        {
            gameObject.SetActive(true);
            animatedImage.anchoredPosition = startPos;
            animatedImage.sizeDelta = startSize;
            startPosition = startPos;
            moving = true;
            resizing = true;
            animationActive = true;
        }
    }

    public void ReverseAnimation()
    {
        if (!animationActive)
        {
            openedIcon.SetActive(false);
            reversing = true;
            animationActive = true;
            logic.IconOpened = false;
        }
    }
}
