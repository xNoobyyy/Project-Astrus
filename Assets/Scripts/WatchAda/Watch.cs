using UnityEngine;

public class Watch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject watchbackground;
    private bool opening = false;
    void Start()
    {
        watchbackground.transform.localScale = new Vector3(0, 0, 1);
        watchbackground.SetActive(true);
        
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
    }


    public void open() {
        watchbackground.SetActive(true);
        opening = true;
    }
}
