using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonManagers : MonoBehaviour
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
