using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public OpeningIcon openingIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Image image = gameObject.GetComponent<Image>();

        Color color = image.color;
        color.a = 0.5f;
        image.color = color;
    }

    // Update is called once per frame
    void Update() { }

    public void OnPointerEnter(PointerEventData eventData) {
        gameObject.transform.localScale = new Vector2(4.5f, 4.5f);

        Image image = gameObject.GetComponent<Image>();

        Color color = image.color;
        color.a = 1f;
        image.color = color;
    }

    public void OnPointerExit(PointerEventData eventData) {
        gameObject.transform.localScale = new Vector2(4, 4);

        Image image = gameObject.GetComponent<Image>();

        Color color = image.color;
        color.a = 0.5f;
        image.color = color;
    }

    public void OnPointerClick(PointerEventData eventData) {
        openingIcon.StartAnimation(new Vector2(128, 128));
    }
}