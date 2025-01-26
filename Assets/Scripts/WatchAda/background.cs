using UnityEngine;
using UnityEngine.UI;

public class background : MonoBehaviour
{
    public Sprite newSprite; // Ziehe das neue Sprite hier hinein
    private Image imageComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeImage() {
        if (imageComponent != null && newSprite != null) {
            // Setze das neue Sprite
            imageComponent.sprite = newSprite;
        }
    }
}
