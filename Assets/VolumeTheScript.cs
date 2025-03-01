using UnityEngine;
using UnityEngine.UI;

public class VolumeTheScript : MonoBehaviour
{
    public Slider slider;

    public float theVolume;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theVolume = slider.value;
        AudioListener.volume = theVolume;
    }
}
