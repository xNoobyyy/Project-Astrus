using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public TMP_InputField inputField;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick() {
        PlayerPrefs.SetString("PlayerName",GetPlayerName() );
        PlayerPrefs.Save();
        SceneManager.LoadScene("Map");
    }
    
    public String GetPlayerName() {
        return inputField.text;
    }
}
