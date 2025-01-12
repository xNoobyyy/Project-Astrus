using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ItemSlot : MonoBehaviour {
    
    private Image icon;

    void Start() {
        icon = GetComponent<Image>();
    }

    
    
}