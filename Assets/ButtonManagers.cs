using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonManagers : MonoBehaviour {
    public TMP_InputField inputField;

    private void Start() {
        inputField.text = PlayerPrefs.GetString("PlayerName", "Unbekannt");
    }

    public void OnButtonClick() {
        PlayerPrefs.SetString("PlayerName", GetPlayerName());
        PlayerPrefs.Save();
        SceneManager.LoadScene("Scenes/Map");
    }

    private string GetPlayerName() {
        return inputField.text;
    }
}